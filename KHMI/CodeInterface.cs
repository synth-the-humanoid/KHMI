using System.Runtime.InteropServices;

namespace KHMI
{
    // interface for replacing assembly code
    public class CodeInterface
    {
        [DllImport("kernel32.dll")]
        private static extern bool DebugActiveProcess(int dwProcessId);
        [DllImport("kernel32.dll")]
        private static extern bool DebugActiveProcessStop(int dwProcessId);
        [DllImport("kernel32.dll")]
        private static extern bool DebugBreakProcess(int hProcess);
        [DllImport("kernel32.dll")]
        private static extern bool WaitForDebugEventEx(int[] lpDebugEvent, int dwMilliseconds);
        [DllImport("kernel32.dll")]
        private static extern bool ContinueDebugEvent(int dwProcessId, int dwThreadId, long dwContinueStatus);
        private const long DBG_CONTINUE = 0x0010002L;

        private MemoryInterface memInterface;
        private IntPtr[] openMemory = Array.Empty<IntPtr>(); // pointers to allocated memory that is freed on close
        public CodeInterface(MemoryInterface mi) // creates a code interface, linked to an already linked memory interface
        {
            if (mi != null)
            {
                memInterface = mi;
                StartDebug();
            }
        }

        internal bool DebugPause()
        {
            return DebugBreakProcess(memInterface.processHandle);
        }

        internal bool DebugUnpause()
        {
            int[] debugEvent = new int[32];
            bool result = WaitForDebugEventEx(debugEvent, 0);
            return result && ContinueDebugEvent(debugEvent[1], debugEvent[2], DBG_CONTINUE);
        }

        private byte[] assembleHook(byte[] originalCode, byte[] payload, bool originalCodeFirst=true) // creates a byte array of assembly code that includes code we've overwritten to jump to this array, the payload, and a template to preserve registers and return
        {
            byte[] prefix = { 0x58, 0xFF, 0x34, 0x24, 0x48, 0x89, 0x44, 0x24, 0x08, 0x58 };
            byte[] postfix = { 0xC3 };
            byte[] hook = new byte[prefix.Length + originalCode.Length + payload.Length + postfix.Length];
            int i = 0;
            foreach (byte b in prefix)
            {
                hook[i++] = b;
            }
            if (originalCodeFirst)
            {
                foreach (byte b in originalCode)
                {
                    hook[i++] = b;
                }
                foreach (byte b in payload)
                {
                    hook[i++] = b;
                }
            }
            else
            {
                foreach (byte b in payload)
                {
                    hook[i++] = b;
                }
                foreach (byte b in originalCode)
                {
                    hook[i++] = b;
                }
            }
            foreach(byte b in postfix)
            {
                hook[i++] = b;
            }

            return hook;
        }

        private byte[] allocHook(byte[] originalCode, byte[] payload, bool originalCodeFirst=true) // calls assemble hook, allocates memory for the result and stores it. returns the address in a byte[] to be inserted into a jmp/call statement
        {
            byte[] hook = assembleHook(originalCode, payload, originalCodeFirst);
            IntPtr hookAddress = MemoryInterface.VirtualAllocEx(memInterface.processHandle, 0, hook.Length, MemoryInterface.MEM_COMMIT, MemoryInterface.PAGE_EXECUTE_READWRITE);
            memInterface.writeBytes(hookAddress, hook);
            openMemory.Append<IntPtr>(hookAddress);
            return BitConverter.GetBytes(hookAddress);
        }

        private byte[] assembleReplacement(byte[] originalCode, byte[] payload, bool originalCodeFirst = true) // returns a byte[] of code that can be used to overwrite originalCode. if originalcode is less than 13 bytes, this array will be too large
        {
            byte[] prefix = { 0x50, 0x48, 0xB8 };
            byte[] hookAddress = allocHook(originalCode, payload, originalCodeFirst);
            byte[] postfix = { 0xFF, 0xD0 };

            byte[] replacement = new byte[prefix.Length + hookAddress.Length + postfix.Length];
            int i = 0;
            foreach(byte b in prefix)
            {
                replacement[i++] = b;
            }
            foreach (byte b in hookAddress)
            {
                replacement[i++] = b;
            }
            foreach (byte b in postfix)
            {
                replacement[i++] = b;
            }

            if (replacement.Length >= originalCode.Length)
            {
                return replacement;
            }
            byte[] paddedReplacement = new byte[originalCode.Length];
            for(int i2 = 0; i2 < paddedReplacement.Length; i2++)
            {
                if (i2 < replacement.Length)
                {
                    paddedReplacement[i2] = replacement[i2];
                }
                else
                {
                    paddedReplacement[i2] = 0x90;
                }
            }

            return paddedReplacement;
        }

        public IntPtr allocDataRegion(int size) // allocates a region of size bytes in the process. returns a pointer to the data
        {
            IntPtr address = MemoryInterface.VirtualAllocEx(memInterface.processHandle, 0, size, MemoryInterface.MEM_COMMIT, MemoryInterface.PAGE_EXECUTE_READWRITE);
            openMemory.Append(address);
            return address;
        }

        public bool insertHook(IntPtr address, byte[] payload, int originalCodeSize=0, bool originalCodeFirst = true) // inserts a jump statement at address which preserves originalCode of the length of originalCodeSize, or the size of payload if it is 0. size should be 13 or greater
        {
            if(originalCodeSize == 0)
            {
                originalCodeSize = payload.Length;
            }

            byte[] originalCode = memInterface.readBytes(address, originalCodeSize);
            byte[] replacementCode = assembleReplacement(originalCode, payload, originalCodeFirst);

            bool result = DebugPause();
            memInterface.writeBytes(address, replacementCode);
            return result && DebugUnpause();
        }

        public bool insertDataHook(IntPtr address, byte[] payload, IntPtr data, int originalCodeSize = 0, bool originalCodeFirst = true) // inserts a hook, but sets the RAX register to hold the IntPtr data
        {
            byte[] prefix = { 0x50, 0x48, 0xB8 };
            byte[] dataBytes = BitConverter.GetBytes(data);
            byte[] postfix = { 0x58 };

            byte[] newPayload = new byte[prefix.Length + dataBytes.Length + payload.Length + postfix.Length];

            int i = 0;
            foreach (byte b in prefix)
            {
                newPayload[i++] = b;
            }
            foreach (byte b in dataBytes)
            {
                newPayload[i++] = b;
            }
            foreach (byte b in payload)
            {
                newPayload[i++] = b;
            }
            foreach (byte b in postfix)
            {
                newPayload[i++] = b;
            }

            return insertHook(address, newPayload, originalCodeSize, originalCodeFirst);
        }

        public bool insertMultiHook(IntPtr address, byte[][] payloads, int originalCodeSize=0, bool originalCodeFirst = true) // inserts multiple payloads at one address. runs them in order of appearance in the array
        {
            byte[] data = new byte[IntPtr.Size * (payloads.Length + 1)];
            int iData = 0;
            int bytesWritten = 0;
            foreach (byte[] bArray in payloads)
            {
                byte[] currentPostfix = { 0xC3 };
                byte[] currentPayload = new byte[bArray.Length + currentPostfix.Length];

                int i = 0;
                foreach(byte b in bArray)
                {
                    currentPayload[i++] = b;
                }
                foreach (byte b in currentPostfix)
                {
                    currentPayload[i++] = b;
                }

                IntPtr payloadAddress = allocDataRegion(currentPayload.Length);
                MemoryInterface.WriteProcessMemory(memInterface.processHandle, payloadAddress, currentPayload, currentPayload.Length, ref bytesWritten);
                byte[] addressBytes = BitConverter.GetBytes(payloadAddress);
                foreach(byte b in addressBytes)
                {
                    data[iData++] = b;
                }
            }

            while(iData < data.Length)
            {
                data[iData++] = 0;
            }

            IntPtr jumpTableAddress = allocDataRegion(data.Length);
            MemoryInterface.WriteProcessMemory(memInterface.processHandle, jumpTableAddress, data, data.Length, ref bytesWritten);
            byte[] multiLoader = { 0x48, 0x83, 0x38, 0x00, 0x0F, 0x84, 0x08, 0x00, 0x00, 0x00, 0xFF, 0x10, 0x48, 0x83, 0xC0, 0x08, 0xEB, 0xEE };
            return insertDataHook(address, multiLoader, jumpTableAddress, originalCodeSize, originalCodeFirst);
        }

        public bool isLinked
        {
            get
            {
                return memInterface.isLinked || openMemory.Length > 0;
            }
        }

        public bool close() // closes open memory in hooks
        {
            bool result = isLinked;
            foreach (IntPtr eachPtr in openMemory)
            {
                if(!MemoryInterface.VirtualFreeEx(memInterface.processHandle, (long)eachPtr, 0, MemoryInterface.MEM_RELEASE))
                {
                    result = false;
                }
            }
            if (memInterface.processId != 0)
            {
                result = result && StopDebug();
            }
            return result;
        }

        public bool StartDebug()
        {
            return DebugActiveProcess(memInterface.processId);
        }

        public bool StopDebug()
        {
            return DebugActiveProcessStop(memInterface.processId);
        }

        public bool ReloadDebug()
        {
            return StopDebug();
        }

        public MemoryInterface memoryInterface
        {
            get
            {
                return memInterface;
            }
        }
    }
}
