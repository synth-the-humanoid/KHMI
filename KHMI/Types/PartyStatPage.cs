namespace KHMI.Types
{
    public class PartyStatPage : KHMIType
    {
        public PartyStatPage(DataInterface di, IntPtr address) : base(di, address) { }

        public byte Level
        {
            get
            {
                return memoryInterface.readByte(address);
            }
            set
            {
                memoryInterface.writeByte(address, value);
            }
        }
        public byte CurrentHP
        {
            get
            {
                return memoryInterface.readByte(address + 0x1);
            }
            set
            {
                memoryInterface.writeByte(address + 0x1, value);
            }
        }
        public byte MaxHP
        {
            get
            {
                return memoryInterface.readByte(address + 0x2);
            }
            set
            {
                memoryInterface.writeByte(address + 0x2, value);
            }
        }
        public byte CurrentMP
        {
            get
            {
                return memoryInterface.readByte(address + 0x3);
            }
            set
            {
                memoryInterface.writeByte(address + 0x3, value);
            }
        }
        public byte MaxMP
        {
            get
            {
                return memoryInterface.readByte(address + 0x4);
            }
            set
            {
                memoryInterface.writeByte(address + 0x4, value);
            }
        }
        public byte MaxAP
        {
            get
            {
                return memoryInterface.readByte(address + 0x5);
            }
            set
            {
                memoryInterface.writeByte(address + 0x5, value);
            }
        }
        public byte Strength
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
        public byte Defense
        {
            get
            {
                return memoryInterface.readByte(address + 0x7);
            }
            set
            {
                memoryInterface.writeByte(address + 0x7, value);
            }
        }

        public byte EquipmentLimit
        {
            get
            {
                return memoryInterface.readByte(address + 0x18);
            }
            set
            {
                memoryInterface.writeByte(address + 0x18, value);
            }
        }

        public byte[] Equipment
        {
            get
            {
                return memoryInterface.readBytes(address + 0x19, EquipmentLimit);
            }
            set
            {
                memoryInterface.writeBytes(address + 0x19, value);
            }
        }

        public byte ItemLimit
        {
            get
            {
                return memoryInterface.readByte(address + 0x21);
            }
            set
            {
                memoryInterface.writeByte(address + 0x21, value);
            }
        }

        public byte[] Items
        {
            get
            {
                return memoryInterface.readBytes(address + 0x22, ItemLimit);
            }
            set
            {
                memoryInterface.writeBytes(address + 0x22, value);
            }
        }

        public byte Weapon
        {
            get
            {
                return memoryInterface.readByte(address + 0x32);
            }
            set
            {
                memoryInterface.writeByte(address + 0x32, value);
            }
        }

        public int EXP
        {
            get
            {
                return memoryInterface.readInt(address + 0x3C);
            }
            set
            {
                memoryInterface.writeInt(address + 0x3C, value);
            }
        }

        public byte[] Abilities
        {
            get
            {
                return memoryInterface.readBytes(address + 0x40, 48);
            }
            set
            {
                memoryInterface.writeBytes(address + 0x40, value);
            }
        }

        public byte MagicFlag
        {
            get
            {
                return memoryInterface.readByte(address + 0x70);
            }
            set
            {
                memoryInterface.writeByte(address + 0x70, value);
            }
        }

        public override string ToString()
        {
            string baseString = string.Format("PartyStatPage:\nHP: {0:D}/{1:D}\nMP: {2:D}/{3:D}\nMax AP: {4:D}\nStrength: {5:D}\nDefense: {6:D}\n", CurrentHP, MaxHP, CurrentMP, MaxMP, MaxAP, Strength, Defense);
            byte[] equips = Equipment;
            string eqStr = "";
            foreach(byte b in equips)
            {
                eqStr = string.Format("{0} {1:X}", eqStr, b);
            }
            eqStr = string.Format("Equipment: {0}\n", eqStr);
            byte[] items = Items;
            string itemStr = "";
            foreach(byte b in items)
            {
                itemStr = string.Format("{0} {1:X}", itemStr, b);
            }
            itemStr = string.Format("Items: {0}\n", itemStr);
            string abStr = "";
            byte[] abilities = Abilities;
            foreach(byte b in abilities)
            {
                abStr = string.Format("{0} {1:X}", abStr, b);
            }
            abStr = string.Format("Abilities: {0}\n", abStr);

            return string.Format("{0}{1}{2}Weapon: {3:X}\nEXP: {4:D}\n{5}Magic Flag: {6:X}\n", baseString, eqStr, itemStr, Weapon, EXP, abStr, MagicFlag);
        }
    }
}
