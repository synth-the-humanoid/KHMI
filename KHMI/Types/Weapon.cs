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


        public byte ModelID
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

        public override string ToString()
        {
            return string.Format("Weapon:\n{0}\nModel ID: {1:D}\n", base.ToString(), ModelID);
        }
    }
}
