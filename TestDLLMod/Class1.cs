using KHMI;
using KHMI.Types;
using System.Numerics;

namespace TestDLLMod
{
    public class Class1 : KHMod
    {
        public Class1(ModInterface mi) : base(mi) { }


        public override void playerLoaded(Entity newPlayer)
        {
            IntPtr entityTableStart = modInterface.memoryInterface.nameToAddress("FirstEntity");
            IntPtr entityTableEndPtr = modInterface.memoryInterface.nameToAddress("FinalEntityPtr");
            IntPtr entityTableEnd = (IntPtr)modInterface.memoryInterface.readLong(entityTableEndPtr);
            EntityTable et = new EntityTable(modInterface.dataInterface, entityTableStart, entityTableEnd);
            Entity[] entities = et.Entities;
            Vector3[] positions = new Vector3[entities.Length];
            Random r = new Random();
            for(int i = 0; i < positions.Length; i++)
            {
                positions[i] = entities[i].Position;
            }
            r.Shuffle(positions);
            for(int i = 0; i < entities.Length; i++)
            {
                entities[i].Position = positions[i];
            }
        }

    }
}
