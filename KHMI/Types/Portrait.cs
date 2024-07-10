namespace KHMI.Types
{
    public class Portrait : KHMIType
    {
        public Portrait(DataInterface di, IntPtr address) : base(di, address) { }

        public string Name
        {
            get
            {
                byte[] data = memoryInterface.readBytes(address + 0x14, 32);
                char[] cData = new char[data.Length];
                for(int i = 0; i < cData.Length; i++)
                {
                    cData[i] = (char)data[i];
                }

                return new string(cData);
            }
        }

        public int IconID
        {
            get
            {
                IntPtr jumpAddress = (IntPtr)memoryInterface.readLong(address + 0x70);
                return memoryInterface.readInt(jumpAddress + 0x40);
            }
            set
            {
                IntPtr jumpAddress = (IntPtr)memoryInterface.readLong(address + 0x70);
                memoryInterface.writeInt(jumpAddress + 0x40, value);
            }
        }

        public override string ToString()
        {
            return string.Format("Portrait:\nName: {0}\nIconID: {1:D}\n", Name, IconID);
        }
    }
}
