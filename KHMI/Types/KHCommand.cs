namespace KHMI.Types
{
    public class KHCommand : KHMIType
    {
        public KHCommand(DataInterface di, IntPtr address) : base(di, address) { }

        public KHCommand FromID(int id)
        {
            IntPtr baseArrayPtr = memoryInterface.nameToAddress("CommandBasePtr");
            IntPtr baseAddress = (IntPtr)memoryInterface.readLong(baseArrayPtr);
            return new KHCommand(dataInterface, baseAddress + id * 0x10);
        }

        public short ActionID
        {
            get
            {
                return memoryInterface.readShort(address + 0x4);
            }
            set
            {
                memoryInterface.writeShort(address + 0x4, value);
            }
        }

        public KHAction Action
        {
            get
            {
                return KHAction.FromID(dataInterface, ActionID);
            }
        }
    }
}
