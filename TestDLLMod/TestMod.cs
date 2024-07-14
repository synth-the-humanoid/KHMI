using KHMI;
using KHMI.Types;
using System.Numerics;

namespace TestDLLMod
{
    public class TestMod : KHMod
    {
        public TestMod(ModInterface mi) : base(mi) { }

        private void setMaxHP(Entity e, int newHP)
        {
            StatPage sp = e.StatPage;
            sp.MaxHP = newHP;
            sp.CurrentHP = Math.Min(sp.MaxHP, sp.CurrentHP);
            PartyStatPage psp = sp.PartyStatPage;
            if (psp != null)
            {
                psp.MaxHP = (byte)sp.MaxHP;
            }
        }

        public override void onHPChange(Entity target)
        {
            if (target.IsPartyMember)
            {
                int newHP = (target.StatPage.MaxHP + target.StatPage.CurrentHP) / 2;
                setMaxHP(target, newHP);
            }
            else
            {
                Entity player = Entity.getPlayer(modInterface.dataInterface);
                if (player != null)
                {
                    setMaxHP(player, player.StatPage.MaxHP + 1);
                }
            }
        }

        public override void onEntityDeath(Entity deceased)
        {
            if (deceased.IsPartyMember)
            {
                if (!deceased.IsPlayer)
                {
                    Entity player = Entity.getPlayer(modInterface.dataInterface);
                    setMaxHP(player, player.StatPage.MaxHP * 2 / 3);
                }
            }
        }

        private void setIsoView()
        {
            IntPtr cameraStyle = modInterface.memoryInterface.nameToAddress("CameraStyle");
            modInterface.memoryInterface.writeInt(cameraStyle, 10);
        }

        public override void playerLockOff()
        {
            setIsoView();
        }

        public override void playerLoaded(Entity newPlayer)
        {
            setIsoView();
        }
    }
}
