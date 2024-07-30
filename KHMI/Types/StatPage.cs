namespace KHMI.Types
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
                if(PartyStatPage != null)
                {
                    PartyStatPage.CurrentHP = (byte)value;
                }
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
                if (PartyStatPage != null)
                {
                    PartyStatPage.MaxHP = (byte)value;
                }
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
                if (PartyStatPage != null)
                {
                    PartyStatPage.CurrentMP = (byte)value;
                }
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
                if (PartyStatPage != null)
                {
                    PartyStatPage.MaxMP = (byte)value;
                }
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
                if (PartyStatPage != null)
                {
                    PartyStatPage.Strength = (byte)value;
                }
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
                if (PartyStatPage != null)
                {
                    PartyStatPage.Defense = (byte)value;
                }
            }
        }

        public PartyStatPage PartyStatPage
        {
            get
            {
                IntPtr pspPtr = (IntPtr)memoryInterface.readLong(address + 0xC8);
                if (pspPtr == 0)
                {
                    return null;
                }
                return new PartyStatPage(dataInterface, pspPtr);
            }
        }

        public bool IsEnvironment
        {
            get
            {
                IntPtr firstStatPage = memoryInterface.nameToAddress("StatPageArrayBase");
                return address == firstStatPage;
            }
        }

        public override string ToString()
        {
            string pspStr = "";
            if (PartyStatPage != null)
            {
                pspStr = PartyStatPage.ToString();
            }
            return string.Format("StatPage:\nHP: {0:D}/{1:D}\nMP: {2:D}/{3:D}\nStrength: {4:D}\nDefense: {5:D}\n{6}", CurrentHP, MaxHP, CurrentMP, MaxMP, Strength, Defense, pspStr);
        }
    }
}
