using KHMI;
using KHMI.Types;

namespace TestDLLMod
{
    public class TestMod : KHMod
    {
        public TestMod(ModInterface mi) : base(mi)
        {
            shuffleCommands();
        }

        private void shuffleCommands()
        {
            KHCommand[] commands = new KHCommand[0x7C];
            for (int i = 0; i < 0x7C; i++)
            {
                commands[i] = KHCommand.FromID(modInterface.dataInterface, i);
            }
            Random r = new Random();
            KHCommand[] originalOrder = new KHCommand[commands.Length];
            Array.Copy(commands, originalOrder, originalOrder.Length);
            r.Shuffle(commands);
            for(int i = 0; i < originalOrder.Length; i++)
            {
                originalOrder[i].ActionID = commands[i].ActionID;
                originalOrder[i].CommandCode = commands[i].CommandCode;
                originalOrder[i].NextMenuID = commands[i].NextMenuID;
            }
        }

        public override void warpUpdate(int newWarpID)
        {
            shuffleCommands();
        }
    }
}