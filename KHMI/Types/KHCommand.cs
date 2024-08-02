namespace KHMI.Types
{
    public class KHCommand : KHMIType
    {
        public KHCommand(DataInterface di, IntPtr address) : base(di, address) { }

        public static KHCommand FromID(DataInterface di, int id)
        {
            IntPtr baseArrayPtr = di.modInterface.memoryInterface.nameToAddress("CommandBasePtr");
            IntPtr baseAddress = (IntPtr)di.modInterface.memoryInterface.readLong(baseArrayPtr);
            return new KHCommand(di.modInterface.dataInterface, baseAddress + id * 0x10);
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
