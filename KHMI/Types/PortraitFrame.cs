namespace KHMI.Types
{
    public class PortraitFrame : KHMIType
    {
        public PortraitFrame(DataInterface di, IntPtr address) : base(di, address) { }

        public int Size
        {
            get
            {
                return memoryInterface.readInt(address);
            }
            set
            {
                memoryInterface.writeInt(address, value);
            }
        }

        public int Position
        {
            get
            {
                return memoryInterface.readInt(address + 0x4);
            }
            set
            {
                memoryInterface.writeInt(address + 0x4, value);
            }
        }

        public Portrait Portrait
        {
            get
            {
                int offset = memoryInterface.readInt(address + 0x8);
                return new Portrait(dataInterface, dataInterface.convert4to8(offset));
            }
        }

        public override string ToString()
        {
            return string.Format("PortraitFrame:\nSize: {0:D}\nPosition: {1:D}\n{2}\n", Size, Position, Portrait);
        }
    }
}
