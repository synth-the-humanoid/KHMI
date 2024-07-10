namespace KHMI
{

    // class to wrap data management of commonly used variables
    public class DataInterface
    {
        private ModInterface mInterface;

        public DataInterface(ModInterface mi)
        {
            if (mi != null)
            {
                mInterface = mi;                
            }
        }

        public IntPtr convert4to8(int offset)
        {
            if (offset == 0)
            {
                return 0;
            }
            IntPtr baseAddressArray = mInterface.memoryInterface.nameToAddress("4to8Base");
            int selectedBase = (offset & 0x7fffffff) >> 0x19;
            IntPtr baseAddress = (IntPtr)mInterface.memoryInterface.readLong(baseAddressArray + (8 * selectedBase));
            IntPtr lowerBits = offset & 0x1ffffff;
            return baseAddress | lowerBits;
        }

        public int WarpID
        {
            get
            {
                return mInterface.memoryInterface.readInt(mInterface.memoryInterface.nameToAddress("WarpID"));
            }
            set
            {
                mInterface.memoryInterface.writeInt(mInterface.memoryInterface.nameToAddress("WarpID"), value);
            }
        }

        public int RoomID
        {
            get
            {
                return mInterface.memoryInterface.readInt(mInterface.memoryInterface.nameToAddress("RoomID"));
            }
            set
            {
                mInterface.memoryInterface.writeInt(mInterface.memoryInterface.nameToAddress("RoomID"), value);
            }
        }

        public int SceneID
        {
            get
            {
                return mInterface.memoryInterface.readInt(mInterface.memoryInterface.nameToAddress("SceneID"));
            }
            set
            {
                mInterface.memoryInterface.writeInt(mInterface.memoryInterface.nameToAddress("SceneID"), value);
            }
        }

        public int WorldID
        {
            get
            {
                return mInterface.memoryInterface.readInt(mInterface.memoryInterface.nameToAddress("WorldID"));
            }
            set
            {
                mInterface.memoryInterface.writeInt(mInterface.memoryInterface.nameToAddress("WorldID"), value);
            }
        }

        public ModInterface modInterface
        {
            get
            {
                return mInterface;
            }
        }
    }
}
