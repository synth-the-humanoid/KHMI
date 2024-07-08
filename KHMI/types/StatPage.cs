namespace KHMI.Types
{
   public class StatPage
    {
        private IntPtr address;
        private DataInterface dataInterface;

        public StatPage(DataInterface di, IntPtr statpageAddress)
        {
            dataInterface = di;
            address = statpageAddress;
        }

        public int CurrentHP
        {
            get
            {
                return dataInterface.modInterface.memoryInterface.readInt(address + 0x3C);
            }
            set
            {
                dataInterface.modInterface.memoryInterface.writeInt(address + 0x3C, value);
            }
        }

        public int MaxHP
        {
            get
            {
                return dataInterface.modInterface.memoryInterface.readInt(address + 0x40);
            }
            set
            {
                dataInterface.modInterface.memoryInterface.writeInt(address + 0x40, value);
            }
        }

        public int CurrentMP
        {
            get
            {
                return dataInterface.modInterface.memoryInterface.readInt(address + 0x44);
            }
            set
            {
                dataInterface.modInterface.memoryInterface.writeInt(address + 0x44, value);
            }
        }

        public int MaxMP
        {
            get
            {
                return dataInterface.modInterface.memoryInterface.readInt(address + 0x48);
            }
            set
            {
                dataInterface.modInterface.memoryInterface.writeInt(address + 0x48, value);
            }
        }

        public int Strength
        {
            get
            {
                return dataInterface.modInterface.memoryInterface.readInt(address + 0x4C);
            }
            set
            {
                dataInterface.modInterface.memoryInterface.writeInt(address + 0x4C, value);
            }
        }

        public int Defense
        {
            get
            {
                return dataInterface.modInterface.memoryInterface.readInt(address + 0x50);
            }
            set
            {
                dataInterface.modInterface.memoryInterface.writeInt(address + 0x50, value);
            }
        }

        public string toString()
        {
            return string.Format("HP: {0:D}/{1:D}\nMP: {2:D}/{3:D}\nStrength: {4:D}\nDefense: {5:D}\n", CurrentHP, MaxHP, CurrentMP, MaxMP, Strength, Defense);
        }
    }
}
