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

        public byte CurrentAP
        {
            get
            {
                byte spentAP = 0;
                foreach(Ability a in EquippedAbilities)
                {
                    spentAP += a.APCost;
                }
                return (byte)(MaxAP - spentAP);
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

        public byte[] ItemBytes
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

        public Item[] Items
        {
            get
            {
                byte[] itemBytes = ItemBytes;
                Item[] items = new Item[itemBytes.Length];
                for(int i = 0; i < items.Length; i++)
                {
                    items[i] = Item.FromID(dataInterface, itemBytes[i]);
                }
                return items;
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

        public byte[] AbilityBytes
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

        public int AbilityCount
        {
            get
            {
                int i = 0;
                foreach(byte b in AbilityBytes)
                {
                    if (b == 0)
                    {
                        break;
                    }
                    i++;
                }
                return i;
            }
        }

        public int EquippedAbilityCount
        {
            get
            {
                int i = 0;
                foreach(byte b in AbilityBytes)
                {
                    if (b == 0)
                    {
                        break;
                    }
                    if(b < 0x80)
                    {
                        i++;
                    }
                }
                return i;
            }
        }

        public Ability[] Abilities
        {
            get
            {
                byte[] bytes = AbilityBytes;
                Ability[] abilities = new Ability[AbilityCount];
                for(int i = 0; i < abilities.Length; i++)
                {
                    abilities[i] = Ability.FromID(dataInterface, bytes[i]);
                }

                return abilities;
            }
        }

        public Ability[] EquippedAbilities
        {
            get
            {
                byte[] abilityBytes = AbilityBytes;
                Ability[] equippedAbilities = new Ability[EquippedAbilityCount];
                int i = 0;
                foreach(byte b in abilityBytes)
                {
                    if(b < 0x80)
                    {
                        equippedAbilities[i++] = Ability.FromID(dataInterface, b);
                    }
                }
                return equippedAbilities;
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
            eqStr = string.Format("Equipment:\n{0}\n", eqStr);
            Item[] items = Items;
            string itemStr = "";
            foreach(Item item in items)
            {
                itemStr = string.Format("{0}{1}", itemStr, item.ToString());
            }
            itemStr = string.Format("Items:\n{0}\n", itemStr);
            string abStr = "";
            Ability[] abilities = Abilities;
            foreach(Ability a in abilities)
            {
                abStr = string.Format("{0}{1}", abStr, a.ToString());
            }
            abStr = string.Format("Abilities: {0}\n", abStr);

            return string.Format("{0}{1}{2}Weapon: {3:X}\nEXP: {4:D}\n{5}Magic Flag: {6:X}\n", baseString, eqStr, itemStr, Weapon, EXP, abStr, MagicFlag);
        }
    }
}
