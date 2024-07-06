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
            if (mi != null && mi.isLinked)
            {
                memInterface = mi;
                DebugActiveProcess(memInterface.processId);

            }
        }

        private byte[] assembleHook(byte[] originalCode, byte[] payload) // creates a byte array of assembly code that includes code we've overwritten to jump to this array, the payload, and a template to preserve registers and return
        {
            byte[] prefix = { 0x58, 0xFF, 0x34, 0x24, 0x48, 0x89, 0x44, 0x24, 0x08, 0x58 };
            byte[] postfix = { 0xC3 };
            byte[] hook = new byte[prefix.Length + originalCode.Length + payload.Length + postfix.Length];
            int i = 0;
            foreach (byte b in prefix)
            {
                hook[i++] = b;
            }
            foreach (byte b in originalCode)
            {
                hook[i++] = b;
            }
            foreach(byte b in payload)
            {
                hook[i++] = b;
            }
            foreach(byte b in postfix)
            {
                hook[i++] = b;
            }

            return hook;
        }

        private byte[] allocHook(byte[] originalCode, byte[] payload) // calls assemble hook, allocates memory for the result and stores it. returns the address in a byte[] to be inserted into a jmp/call statement
        {
            byte[] hook = assembleHook(originalCode, payload);
            IntPtr hookAddress = MemoryInterface.VirtualAllocEx(memInterface.processHandle, 0, hook.Length, MemoryInterface.MEM_COMMIT, MemoryInterface.PAGE_EXECUTE_READWRITE);
            memInterface.writeBytes(hookAddress, hook);
            openMemory.Append<IntPtr>(hookAddress);
            return BitConverter.GetBytes(hookAddress);
        }

        private byte[] assembleReplacement(byte[] originalCode, byte[] payload) // returns a byte[] of code that can be used to overwrite originalCode. if originalcode is less than 13 bytes, this array will be too large
        {
            byte[] prefix = { 0x50, 0x48, 0xB8 };
            byte[] hookAddress = allocHook(originalCode, payload);
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

        public bool insertCode(IntPtr address, byte[] payload, int originalCodeSize=0) // inserts a jump statement at address which preserves originalCode of the length of originalCodeSize, or the size of payload if it is 0. size should be 13 or greater
        {
            if(originalCodeSize == 0)
            {
                originalCodeSize = payload.Length;
            }

            byte[] originalCode = memInterface.readBytes(address, originalCodeSize);
            byte[] replacementCode = assembleReplacement(originalCode, payload);

            bool result = DebugBreakProcess(memInterface.processHandle);
            memInterface.writeBytes(address, replacementCode);
            int[] debugEvent = new int[32];
            result = result && WaitForDebugEventEx(debugEvent, 0);
            result = result && ContinueDebugEvent(debugEvent[1], debugEvent[2], DBG_CONTINUE);
            return result;
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
                result = result && DebugActiveProcessStop(memInterface.processId);
            }
            return result;
        }
    }
}
