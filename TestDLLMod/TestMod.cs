using KHMI;
using KHMI.Types;
using System.Collections;

namespace TestDLLMod
{
    public class TestMod : KHMod
    {
        public TestMod(ModInterface mi) : base(mi)
        {
            ShuffleWeapons();
        }

        public override void worldUpdate(World newWorld)
        {
            ShuffleWeapons();
        }

        private void ShuffleWeapons()
        {
            ArrayList weapons = new ArrayList();
            for (int i = 0; i < 256; i++)
            {
                Item current = Item.FromID(modInterface.dataInterface, (byte)i);
                if (current.ItemType == ItemType.WEAPON)
                {
                    weapons.Add(new Weapon(modInterface.dataInterface, current.Address));
                }
            }
            short[] names = new short[weapons.Count];
            int[] descs = new int[weapons.Count];
            string[] models = new string[weapons.Count];
            byte[] strs = new byte[weapons.Count];
            byte[] mps = new byte[weapons.Count];
            for (int i = 0; i < weapons.Count; i++)
            {
                Weapon current = (Weapon)weapons[i];
                names[i] = current.ActionID;
                descs[i] = current.DescOffset;
                models[i] = current.WeaponStatPage.ModelName;
                strs[i] = current.WeaponStatPage.Strength;
                mps[i] = current.WeaponStatPage.MP;
            }
            Random r = new Random();
            r.Shuffle(names);
            r.Shuffle(descs);
            r.Shuffle(models);
            r.Shuffle(strs);
            r.Shuffle(mps);
            for (int i = 0; i < weapons.Count; i++)
            {
                Weapon current = (Weapon)weapons[i];
                current.ActionID = names[i];
                current.DescOffset = descs[i];
                current.WeaponStatPage.ModelName = models[i];
                current.WeaponStatPage.Strength = strs[i];
                current.WeaponStatPage.MP = mps[i];
            }
        }
    }
}