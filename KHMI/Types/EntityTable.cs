namespace KHMI.Types
{
    public class EntityTable : KHMITypeTable
    {
        public EntityTable(DataInterface di, IntPtr addressStart, IntPtr addressEnd) : base(di, addressStart, addressEnd, 0x4B0) { }

        public Entity[] Entities
        {
            get
            {
                IntPtr[] addresses = Addresses;
                Entity[] entities = new Entity[addresses.Length];
                for(int i = 0; i < entities.Length; i++)
                {
                    entities[i] = new Entity(dataInterface, addresses[i]);
                }

                return entities;
            }
        }

        public static EntityTable Current(DataInterface di)
        {
            IntPtr firstEnt = di.modInterface.memoryInterface.nameToAddress("FirstEntity");
            IntPtr lastEntPtr = di.modInterface.memoryInterface.nameToAddress("FinalEntityPtr");
            IntPtr lastEnt = (IntPtr)di.modInterface.memoryInterface.readLong(lastEntPtr);
            return new EntityTable(di, firstEnt, lastEnt);
        }

        public override string ToString()
        {
            string data = string.Format("Entity Count: {0:D}\n", Size);
            Entity[] entities = Entities;
            foreach(Entity e in entities)
            {
                data += e.ToString();
            }

            return data;
        }
    }
}
