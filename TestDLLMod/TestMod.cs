using KHMI;
using KHMI.Types;

namespace TestDLLMod
{
    public class TestMod : KHMod
    {
        public TestMod(ModInterface mi) : base(mi) { }

        public override void playerLockOn(Entity target)
        {
            Actor actor = target.Actor;
            Item reward = actor.Reward;
            if (reward != null)
            {
                Console.WriteLine("The reward inside of {0} is {1}.", actor.Name, reward.Name);
            }
        }
    }
}
