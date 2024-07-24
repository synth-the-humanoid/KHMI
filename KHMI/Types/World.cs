namespace KHMI.Types
{
    public class World : KHMIType
    {
        private int worldID;
        public World(DataInterface di, int id) : base(di, IntPtr.Zero)
        {
            worldID = id;
        }

        public static World Current(DataInterface di)
        {
            IntPtr idAddress = di.modInterface.memoryInterface.nameToAddress("WorldID");
            int worldID = di.modInterface.memoryInterface.readInt(idAddress);
            return new World(di, worldID);
        }

        public IntPtr SceneTable
        {
            get
            {
                IntPtr sceneTableBasePtr = memoryInterface.nameToAddress("SceneTableBasePtr");
                IntPtr sceneTableBase = (IntPtr)memoryInterface.readLong(sceneTableBasePtr);
                IntPtr roomOffsetTableBase = memoryInterface.nameToAddress("RoomOffsetTableBase");
                int offset = 0x6C;
                for (int i = 0; i < worldID; i++)
                {
                    offset += memoryInterface.readByte(roomOffsetTableBase + i);
                }
                return sceneTableBase + offset;
            }
        }

        public Room getRoom(int id)
        {
            return new Room(dataInterface, this, id);
        }

        public int WorldID
        {
            get
            {
                return worldID;
            }
        }

        public int RoomCount
        {
            get
            {
                IntPtr roomOffsetTableBase = memoryInterface.nameToAddress("RoomOffsetTableBase");
                return memoryInterface.readByte(roomOffsetTableBase + worldID);
            }
        }

        public Room[] Rooms
        {
            get
            {
                Room[] rooms = new Room[RoomCount];
                for(int i = 0; i < rooms.Length; i++)
                {
                    rooms[i] = new Room(dataInterface, this, i);
                }
                return rooms;
            }
        }
    }
}
