﻿namespace KHMI.Types
{
    public class StatPage : KHMIType
    {
        public StatPage(DataInterface di, IntPtr address) : base(di, address) { }

        public int CurrentHP
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

        public int MaxHP
        {
            get
            {
                return memoryInterface.readInt(address + 0x40);
            }
            set
            {
                memoryInterface.writeInt(address + 0x40, value);
            }
        }

        public int CurrentMP
        {
            get
            {
                return memoryInterface.readInt(address + 0x44);
            }
            set
            {
                memoryInterface.writeInt(address + 0x44, value);
            }
        }

        public int MaxMP
        {
            get
            {
                return memoryInterface.readInt(address + 0x48);
            }
            set
            {
                memoryInterface.writeInt(address + 0x48, value);
            }
        }

        public int Strength
        {
            get
            {
                return memoryInterface.readInt(address + 0x4C);
            }
            set
            {
                memoryInterface.writeInt(address + 0x4C, value);
            }
        }

        public int Defense
        {
            get
            {
                return memoryInterface.readInt(address + 0x50);
            }
            set
            {
                memoryInterface.writeInt(address + 0x50, value);
            }
        }

        public override string ToString()
        {
            return string.Format("StatPage:\nHP: {0:D}/{1:D}\nMP: {2:D}/{3:D}\nStrength: {4:D}\nDefense: {5:D}\n", CurrentHP, MaxHP, CurrentMP, MaxMP, Strength, Defense);
        }
    }
}
