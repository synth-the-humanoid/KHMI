using KHMI;
using KHMI.Types;

namespace TestDLLMod
{
    public class Class1 : KHMod
    {
        public Class1(ModInterface mi) : base(mi) { }

        public override void playerLoaded(Entity newPlayer)
        {
            IntPtr firstEntity = modInterface.memoryInterface.nameToAddress("FirstEntity");
            IntPtr lastEntityPtr = modInterface.memoryInterface.nameToAddress("FinalEntityPtr");
            IntPtr lastEntity = (IntPtr)modInterface.memoryInterface.readLong(lastEntityPtr);
            EntityTable entityTable = new EntityTable(modInterface.dataInterface, firstEntity, lastEntity);
            Entity[] entities = entityTable.Entities;
            foreach(Entity e in entities)
            {
                e.Position = newPlayer.Position;
            }
        }

    }
}
