using KHMI;
using KHMI.Types;
using System.Numerics;

namespace TestDLLMod
{
    public class TestMod : KHMod
    {
        public TestMod(ModInterface mi) : base(mi) { }


        public override void partyLoaded(Entity newParty, int partySlot)
        {

            Console.WriteLine("{0}: {1:D}", newParty.Actor.Name, newParty.StatPage.PartyStatPage.Weapon.ToString());
        }
    }
}
