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

            StatPage sp = newPlayer.StatPage;
            PartyStatPage psp = sp.PartyStatPage;
            newPlayer.Position = new Vector3(0, 0, 0);
            Console.WriteLine(psp);
        }
    }
}
