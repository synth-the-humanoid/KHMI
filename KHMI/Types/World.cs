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
    }
}
