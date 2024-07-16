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
            Item weapon = psp.Weapon;
            weapon.Name.ASCII = "bonkstick";
            weapon.Description.ASCII = "the true bonk blade";
        }
    }
}
