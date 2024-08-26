namespace KHMI.Types
{
    public class WeaponStatPage : KHMIType
    {
        public WeaponStatPage(DataInterface di, IntPtr address) : base(di, address) { }

        private static int GetOffsetForPartyMember(DataInterface di, PartyMemberID partyID)
        {
            IntPtr offsetTableBase = di.modInterface.memoryInterface.nameToAddress("PartyWeaponOffsetTable");
            int offset = di.modInterface.memoryInterface.readInt(offsetTableBase + (int)partyID * 4);
            return offset;
        }
        public static WeaponStatPage FromID(DataInterface di, PartyMemberID partyID, byte weaponID)
        {
            int offset = GetOffsetForPartyMember(di, partyID);
            if (offset == 0)
            {
                return null;
            }
            IntPtr weaponTableBase = di.modInterface.memoryInterface.nameToAddress("WeaponInfoTable");
            weaponTableBase += offset;
            return new WeaponStatPage(di, weaponTableBase + ((int)weaponID-1) * 0x58);
        }

        public static WeaponStatPage[] GetAllWeaponsForParty(DataInterface di, PartyMemberID partyID)
        {
            int partySize = Enum.GetValues(typeof(PartyMemberID)).Length;
            int weaponCount = 0;
            int currentOffset = GetOffsetForPartyMember(di, partyID);
            if(currentOffset == 0)
            {
                return new WeaponStatPage[0];
            }
            IntPtr weaponTableBase = di.modInterface.memoryInterface.nameToAddress("WeaponInfoTable");
            IntPtr startAddress = weaponTableBase + currentOffset;
            // if we are on the last party member(beast by default)
            // we need to handle the calculation differently by checking for
            // the last string instead of the distance from the current member to the next
            if ((int)partyID == partySize-1) {
                char nextChar;
                do
                {
                    nextChar = (char)di.modInterface.memoryInterface.readByte(startAddress + (weaponCount++ + 1) * 0x58);
                }
                while (nextChar == 'x');
            }
            else
            {
                int nextOffset;
                int skippedOffsets = 1;
                do
                {
                    nextOffset = GetOffsetForPartyMember(di, partyID + skippedOffsets++);
                }
                while (nextOffset == 0 && (int)partyID + skippedOffsets < partySize);
                if (nextOffset == 0)
                {
                    return new WeaponStatPage[0];
                }
                weaponCount = (nextOffset - currentOffset) / 0x58;
            }

            WeaponStatPage[] weapons = new WeaponStatPage[weaponCount];
            for(int i = 0; i < weapons.Length; i++)
            {
                weapons[i] = new WeaponStatPage(di, startAddress + i * 0x58);
            }
            return weapons;
        }

        public static int GetWeaponCountForPartyMember(DataInterface di, PartyMemberID partyID)
        {
            return GetAllWeaponsForParty(di, partyID).Length;
        }


        public string ModelName
        {
            get
            {
                byte[] nameData = memoryInterface.readBytes(address, 10);
                char[] nameChars = new char[nameData.Length];
                for(int i = 0; i < nameChars.Length; i++)
                {
                    nameChars[i] = (char)nameData[i];
                }
                return new string(nameChars);
            }
            set
            {
                char[] nameChars = value.ToCharArray();
                byte[] nameData = new byte[nameChars.Length];
                for(int i = 0; i < nameData.Length; i++)
                {
                    nameData[i] = (byte)nameChars[i];
                }
                memoryInterface.writeBytes(address, nameData);
            }
        }

        public byte Strength
        {
            get
            {
                return memoryInterface.readByte(address + 0x30);
            }
            set
            {
                memoryInterface.writeByte(address + 0x30, value);
            }
        }

        public byte MP
        {
            get
            {
                return memoryInterface.readByte(address + 0x38);
            }
            set
            {
                memoryInterface.writeByte(address + 0x38, value);
            }
        }
    }
}
