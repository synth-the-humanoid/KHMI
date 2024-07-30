using KHMI;
using KHMI.Types;

namespace TestDLLMod
{
    public class TestMod : KHMod
    {
        private IntPtr cameraTargetPtr;

        public TestMod(ModInterface mi) : base(mi)
        {
            cameraTargetPtr = modInterface.memoryInterface.nameToAddress("CameraTargetPtr");
        }


        private void updateCamera(Entity target)
        {
            modInterface.memoryInterface.writeLong(cameraTargetPtr, target.EntityPtr);
        }

        public override void playerLockOn(Entity target)
        {
            updateCamera(target);
        }

        public override void playerLockOff()
        {
            updateCamera(Entity.getPlayer(modInterface.dataInterface));
        }

        public override void onDamage(Entity target, int damage)
        {
            EntityTable et = EntityTable.Current(modInterface.dataInterface);
            Entity[] enemies = et.Enemies;

            if (target.IsPartyMember)
            {
                target.StatPage.MaxHP -= (damage / 2);

                foreach (Entity e in enemies)
                {
                    e.StatPage.MaxHP += (damage / 2);
                    e.StatPage.CurrentHP += (damage / 2);
                }
            }

            else if(target.IsEnemy)
            {
                foreach(Entity e in enemies)
                {
                    if(e.EntityPtr != target.EntityPtr)
                    {
                        e.StatPage.MaxHP += (damage / 2);
                        e.StatPage.CurrentHP += (damage / 2);
                    }
                }
            }
        }

        public override void onEntityDeath(Entity deceased)
        {
            if(deceased.IsPartyMember && !deceased.IsPlayer)
            {
                Entity player = Entity.getPlayer(modInterface.dataInterface);
                if (player != null)
                {
                    player.StatPage.MaxHP = (player.StatPage.MaxHP * 9 / 10);
                    player.StatPage.CurrentHP = (player.StatPage.CurrentHP * 9 / 10);

                }
            }
            else if(deceased.IsEnemy)
            {
                EntityTable et = EntityTable.Current(modInterface.dataInterface);
                Entity[] party = et.Party;
                foreach(Entity e in party)
                {
                    e.StatPage.MaxHP = e.StatPage.MaxHP + (deceased.StatPage.MaxHP * 1 / 100);
                }
            }
        }
    }
}