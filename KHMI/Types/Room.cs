namespace KHMI.Types
{
    public class Room : KHMIType
    {
        private World world;
        private int roomID;

        public Room(DataInterface di, World wrld, int id) : base(di, IntPtr.Zero)
        {
            world = wrld;
            roomID = id;
        }

        public static Room Current(DataInterface di)
        {
            World w = World.Current(di);
            IntPtr roomAddress = di.modInterface.memoryInterface.nameToAddress("RoomID");
            int roomID = di.modInterface.memoryInterface.readInt(roomAddress);
            return new Room(di, w, roomID);
        }

        public KHWString Name
        {
            get
            {
                IntPtr nameStringBase = memoryInterface.nameToAddress("RoomNameStringTable");
                IntPtr firstCheck = nameStringBase + (world.WorldID * 8) + 0x10;
                short firstOffset = memoryInterface.readShort(firstCheck);
                IntPtr secondCheck = (nameStringBase + firstOffset) + (roomID * 8) + 0x10;
                short secondOffset = memoryInterface.readShort(secondCheck);
                return new KHWString(dataInterface, nameStringBase + firstOffset + secondOffset);
            }
        }

        public int RoomID
        {
            get
            {
                return roomID;
            }
        }


        public World World
        {
            get
            {
                return world;
            }
        }

        public byte SceneID
        {
            get
            {
                IntPtr sceneTable = world.SceneTable;
                return memoryInterface.readByte(sceneTable + roomID);
            }
            set
            {
                IntPtr sceneTable = world.SceneTable;
                memoryInterface.writeByte(sceneTable + roomID, value);
            }
        }
    }
}
