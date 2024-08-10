using KHMI;
using KHMI.Types;

namespace TestDLLMod
{
    public class TestMod : KHMod
    {
        public TestMod(ModInterface mi) : base(mi) { }


        public override void playerLockOn(Entity target)
        {
            KHScript[] scripts = KHScriptTable.Current(modInterface.dataInterface).Scripts;
            for(int i = 0; i < scripts.Length; i++)
            {
                Console.WriteLine("Script {0:D}\n - Event ID: {1:X}", i, scripts[i].EventID);
                if (scripts[i].Entity != null)
                {
                    Console.WriteLine(" - Entity: {0}", scripts[i].Entity.Actor.Name);
                }
                Console.WriteLine();
            }
        }
    }
}