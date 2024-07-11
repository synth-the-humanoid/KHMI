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
