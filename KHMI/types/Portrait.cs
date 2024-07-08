namespace KHMI.types
{
    public class Portrait
    {
        private IntPtr address;
        private DataInterface dataInterface;

        public Portrait(DataInterface di, IntPtr portraitAddress)
        {
            dataInterface = di;
            address = portraitAddress;
        }

        public string Name
        {
            get
            {
                byte[] bytes = dataInterface.modInterface.memoryInterface.readBytes(address + 0x14, 32);
                char[] chars = new char[bytes.Length];
                for(int i = 0; i < bytes.Length; i++)
                {
                    chars[i] = (char)bytes[i];
                }
                return new string(chars);
            }
        }

        public int ImageID
        {
            get
            {
                IntPtr skipPtr = (IntPtr)dataInterface.modInterface.memoryInterface.readLong(address + 0x70);
                return dataInterface.modInterface.memoryInterface.readInt(skipPtr + 0x40);
            }

            set
            {
                IntPtr skipPtr = (IntPtr)dataInterface.modInterface.memoryInterface.readLong(address + 0x70);
                dataInterface.modInterface.memoryInterface.writeInt(skipPtr + 0x40, value);
            }
        }

        public string toString()
        {
            return string.Format("Portrait Name: {0}\nImage ID: {1:D}\n", Name, ImageID);
        }
    }
}
