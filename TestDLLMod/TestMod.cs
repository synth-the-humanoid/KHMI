using KHMI;
using KHMI.Types;

namespace TestDLLMod
{
    public class TestMod : KHMod
    {
        private IntPtr camTargetPtr;
        public TestMod(ModInterface mi) : base(mi)
        {
            camTargetPtr = modInterface.memoryInterface.nameToAddress("CameraTargetPtr");
        }

        public override void playerLoaded(Entity newPlayer)
        {
            Random r = new Random();
            Entity camTargetEnt = Entity.getParty(modInterface.dataInterface, (r.Next(2) + 1));
            if(camTargetEnt != null)
            {
                modInterface.memoryInterface.writeLong(camTargetPtr, camTargetEnt.EntityPtr);
            }
        }

    }
}
