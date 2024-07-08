namespace KHMI.Types
{
    public class PortraitInfo
    {
        private IntPtr address;
        private DataInterface dataInterface;

        public PortraitInfo(DataInterface di, IntPtr piAddress)
        {
            dataInterface = di;
            address = piAddress;
        }

        public int Size
        {
            get
            {
                return dataInterface.modInterface.memoryInterface.readInt(address);
            }
            
            set
            {
                dataInterface.modInterface.memoryInterface.writeInt(address, value);
            }
        }

        public int Position
        {
            get
            {
                return dataInterface.modInterface.memoryInterface.readInt(address + 0x4);
            }

            set
            {
                dataInterface.modInterface.memoryInterface.writeInt(address + 0x4, value);
            }
        }

        public Portrait Portrait
        {
            get
            {
                int offset = dataInterface.modInterface.memoryInterface.readInt(address + 0x8);
                return new Portrait(dataInterface, dataInterface.convert4to8(offset));
            }
        }

        public string toString()
        {
            return string.Format("Size: {0:D}\nPosition: {1:D}\n\nPortrait Info:\n{2}\n", Size, Position, Portrait.toString());
        }
    }
}
