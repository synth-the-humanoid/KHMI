using KHMI;
using KHMI.Types;

namespace TestDLLMod
{
    public class TestMod : KHMod
    {
        public TestMod(ModInterface mi) : base(mi)
        {
            for(int i = 0; i < 256; i++)
            {
                Item current = Item.FromID(mi.dataInterface, (byte)i);
                if (current != null)
                {
                    current.InventoryAmount = 0;
                }
            }
        }
    }
}
