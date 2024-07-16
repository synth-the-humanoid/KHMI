using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;

namespace KHMI
{
    // object to interface with kingdom hearts' game memory. after constructing, call locateProcess() until true
    public class MemoryInterface
    {
        [DllImport("kernel32.dll")]
        internal static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);
        internal const int PROCESS_VM_READ = 0x10;
        internal const int PROCESS_VM_WRITE = 0x20;
        internal const int PROCESS_VM_OPERATION = 0x8;
        internal int PROCESS_VM_ALL = PROCESS_VM_OPERATION | PROCESS_VM_READ | PROCESS_VM_WRITE;

        [DllImport("kernel32.dll")]
        internal static extern bool ReadProcessMemory(int hProcess, long lpBaseAddress, byte[] lpBuffer, int nSize, ref int lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        internal static extern bool WriteProcessMemory(int hProcess, long lpBaseAddress, byte[] lpBuffer, int nSize, ref int lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        internal static extern bool CloseHandle(int hObject);
        [DllImport("kernel32.dll")]
        internal static extern IntPtr VirtualAllocEx(int hProcess, long lpAddress, int dwSize, int flAllocationType, int flProtect);
        internal const int MEM_COMMIT = 0x1000;
        internal const int PAGE_EXECUTE_READWRITE = 0x40;
        [DllImport("kernel32.dll")]
        internal static extern bool VirtualFreeEx(int hProcess, long lpAddress, int dwSize, int dwFreeType);
        internal const int MEM_RELEASE = 0x8000;
        [DllImport("kernel32.dll")]
        internal static extern bool VirtualProtectEx(int hProcess, long lpAddress, int dwSize, int flNewProtect, ref int lpflOldProtect);

        internal const int INT_SIZE = 4;
        internal const int LONG_SIZE = 8;
        internal const int FLOAT_SIZE = 4;
        internal const int DOUBLE_SIZE = 8;

        private OffsetHandler oh; // used to calculate addresses independent of version
        private IntPtr procHandle; // handle to game process
        private IntPtr baseAddress; // base address of KINGDOM HEARTS FINAL MIX.exe
        private int procId; // process id of the game

        public MemoryInterface(Provider p, string version, string relPath= "./offsets.csv")
        {
            oh = new OffsetHandler(relPath, p, version);
        }

        public bool locateProcess() // locates and attaches to game process, returns true if found - false otherwise
        {
            Process[] processes = Process.GetProcessesByName("KINGDOM HEARTS FINAL MIX");
            if (processes.Length != 0)
            {
                procId = processes[0].Id;
                procHandle = OpenProcess(PROCESS_VM_ALL, false, procId);
                if (procHandle != IntPtr.Zero)
                {
                    baseAddress = processes[0].MainModule.BaseAddress;
                    int oldProtect = 0;
                    if (VirtualProtectEx((int)procHandle, nameToAddress("CodeSeg"), 0x2AE000, PAGE_EXECUTE_READWRITE, ref oldProtect))
                    {
                        return true;
                        
                    }
                }
            }
            return false;
        }

        public IntPtr offsetToAddress(int offset) // converts offset relative to .exe to memory address
        {
            return baseAddress + offset;
        }

        public IntPtr nameToAddress(string name) // converts offset name as defined in offsets.csv to memory address
        {
            int offset = oh.getOffset(name);
            if (offset != 0)
            {
                return offsetToAddress(offset);
            }
            return IntPtr.Zero;
        }

        public byte readByte(IntPtr address)
        {
            return readBytes(address, 1)[0];
        }

        public byte[] readBytes(IntPtr address, int byteCount)
        {
            byte[] data = new byte[byteCount];
            int bytesRead = 0;
            ReadProcessMemory((int)procHandle, (long)address, data, byteCount, ref bytesRead);
            return data;
        }

        public short readShort(IntPtr address)
        {
            return BitConverter.ToInt16(readBytes(address, 2));
        }

        public int readInt(IntPtr address)
        {
            byte[] data = readBytes(address, INT_SIZE);
            return BitConverter.ToInt32(data);
        }

        public long readLong(IntPtr address)
        {
            byte[] data = readBytes(address, LONG_SIZE);
            return BitConverter.ToInt64(data);
        }

        public float readFloat(IntPtr address)
        {
            byte[] data = readBytes(address, FLOAT_SIZE);
            return BitConverter.ToSingle(data);
        }

        public double readDouble(IntPtr address)
        {
            byte[] data = readBytes(address, DOUBLE_SIZE);
            return BitConverter.ToDouble(data);
        }

        public bool writeByte(IntPtr address, byte value)
        {
            return writeBytes(address, [value]);
        }

        public bool writeBytes(IntPtr address, byte[] data)
        {
            int bytesWritten = 0;
            return WriteProcessMemory((int)procHandle, (long)address, data, data.Length, ref bytesWritten);
        }

        public bool writeShort(IntPtr address, short data)
        {
            return writeBytes(address, BitConverter.GetBytes(data));
        }

        public bool writeInt(IntPtr address, int value)
        {
            return writeBytes(address, BitConverter.GetBytes(value));
        }

        public bool writeLong(IntPtr address, long value)
        {
            return writeBytes(address, BitConverter.GetBytes(value));
        }

        public bool writeFloat(IntPtr address, float value)
        {
            return writeBytes(address, BitConverter.GetBytes(value));
        }

        public bool writeDouble(IntPtr address, double value)
        {
            return writeBytes(address, BitConverter.GetBytes(value));
        }

        public Vector3 readVector3(IntPtr address)
        {
            float x = readFloat(address);
            float y = readFloat(address + 0x4);
            float z = readFloat(address + 0x8);
            return new Vector3(x, y, z);
        }

        public bool writeVector3(IntPtr address, Vector3 v)
        {
            bool result = true;
            result = result && writeFloat(address, v.X);
            result = result && writeFloat(address + 0x4, v.Y);
            result = result && writeFloat(address + 0x8, v.Z);
            return result;
        }

        public bool close() // closes open memory + handles, returns true if it worked
        {
            if (procHandle != IntPtr.Zero)
            {
                CloseHandle((int)procHandle);
                procHandle = IntPtr.Zero;
                return true;
            }
            return false;
        }

        internal int processHandle
        {
            get
            {
                return (int)procHandle;
            }
        }

        internal int processId
        {
            get
            {
                return procId;
            }
        }

        public bool isLinked
        {
            get
            {
                return procHandle != IntPtr.Zero;
            }
        }
    }
}
