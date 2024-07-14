namespace KHMI.Types
{
    public class Ability : KHMIType
    {
        public Ability(DataInterface di, IntPtr address) : base(di, address) { }

        public static Ability FromID(DataInterface di, byte id)
        {
            byte normalID = id;
            if(normalID > 0x80)
            {
                normalID -= 0x80;
            }
            int offset = (((normalID - 1) * 3) * 4);
            IntPtr abilityInfoPtr = di.modInterface.memoryInterface.nameToAddress("AbilityInfoPtr");
            IntPtr address = (IntPtr)di.modInterface.memoryInterface.readLong(abilityInfoPtr) + offset;
            return new Ability(di, address);
        }

        public byte APCost
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

        public KHString Name
        {
            get
            {
                int offset = memoryInterface.readInt(address + 0x4);
                return new KHString(dataInterface, dataInterface.convert4to8(offset));
            }
        }

        public KHString Description
        {
            get
            {
                int offset = memoryInterface.readInt(address + 0x8);
                return new KHString(dataInterface, dataInterface.convert4to8(offset));
            }
        }

        public override string ToString()
        {
            return string.Format("Ability:\nName: {0}\nCost: {1:D}\nDescription: {2:D}\n", Name, APCost, Description);
        }
    }
}
