namespace KHMI
{

    // class to wrap data management of commonly used variables
    public class DataInterface
    {
        private MemoryInterface memInterface;

        public DataInterface(MemoryInterface mi)
        {
            if (mi != null)
            {
                memInterface = mi;
            }
        }

        public int WorldID
        {
            get
            {
                return memInterface.readInt(memInterface.nameToAddress("WorldID"));
            }
            set
            {
                memInterface.writeInt(memInterface.nameToAddress("WorldID"), value);
            }
        }

        public int RoomID
        {
            get
            {
                return memInterface.readInt(memInterface.nameToAddress("RoomID"));
            }
            set
            {
                memInterface.writeInt(memInterface.nameToAddress("RoomID"), value);
            }
        }

        public int SceneID
        {
            get
            {
                return memInterface.readInt(memInterface.nameToAddress("SceneID"));
            }
            set
            {
                memInterface.writeInt(memInterface.nameToAddress("SceneID"), value);
            }
        }

        public int WarpID
        {
            get
            {
                return memInterface.readInt(memInterface.nameToAddress("WarpID"));
            }
            set
            {
                memInterface.writeInt(memInterface.nameToAddress("WarpID"), value);
            }
        }
    }
}
