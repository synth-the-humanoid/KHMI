namespace KHMI.Types
{
    public class Weapon : Item
    {
        public Weapon(DataInterface di, IntPtr address) : base(di, address) { }

        public static Weapon FromID(DataInterface di, byte id)
        {
            int offset = ((id - 1) * 5) * 4;
            IntPtr baseAddressPtr = di.modInterface.memoryInterface.nameToAddress("ItemInfoPtr");
            IntPtr baseAddress = (IntPtr)di.modInterface.memoryInterface.readLong(baseAddressPtr);
            return new Weapon(di, baseAddress + offset);
        }


        public byte WeaponID
        {
            get
            {
                return memoryInterface.readByte(address + 0x6);
            }
            set
            {
                memoryInterface.writeByte(address + 0x6, value);
            }
        }

        public PartyMemberID Owner
        {
            get
            {
                int baseWeaponID = 0x51; // kingdom key id
                int displacement = ItemID - baseWeaponID;
                foreach(PartyMemberID partyID in Enum.GetValues(typeof(PartyMemberID)))
                {
                    int weaponCount = WeaponStatPage.GetWeaponCountForPartyMember(dataInterface, partyID);
                    if(displacement < weaponCount)
                    {
                        return partyID;
                    }
                    displacement -= Math.Max(weaponCount,1);
                }
                return PartyMemberID.POOH;
            }
        }

        public WeaponStatPage WeaponStatPage
        {
            get
            {
                return WeaponStatPage.FromID(dataInterface, Owner, WeaponID);
            }
        }

        

        public override string ToString()
        {
            return string.Format("Weapon:\n{0}\nWeapon ID: {1:D}\n", base.ToString(), WeaponID);
        }
    }
}
