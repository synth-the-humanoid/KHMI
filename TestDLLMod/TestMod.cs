using KHMI;
using KHMI.Types;
using System.Numerics;

namespace TestDLLMod
{
    public class TestMod : KHMod
    {
        public TestMod(ModInterface mi) : base(mi) { }


        public override void playerLoaded(Entity newPlayer)
        {
            Room r = Room.Current(modInterface.dataInterface);
            Console.WriteLine("New room: {0}", r.Name);
        }
    }
}
