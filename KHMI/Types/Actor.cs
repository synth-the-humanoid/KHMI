namespace KHMI.Types
{
    public class Actor
    {
        private IntPtr address;
        private DataInterface dataInterface;

        public Actor(DataInterface di, IntPtr miAddress)
        {
            dataInterface = di;
            address = miAddress;
        }

        public string Name
        {
            get
            {
                byte[] data = dataInterface.modInterface.memoryInterface.readBytes(address + 0x68, 16);
                char[] cData = new char[data.Length];
                for (int i = 0; i<cData.Length; i++)
                {
                    cData[i] = (char)data[i];
                }
                return new string(cData);
            }
        }

        public float Movability
        {
            get
            {
                return dataInterface.modInterface.memoryInterface.readFloat(address + 0x18);
            }
            set
            {
                dataInterface.modInterface.memoryInterface.writeFloat(address + 0x18, value);
            }
        }
        
        public string MDLS
        {
            get
            {
                int offset = dataInterface.modInterface.memoryInterface.readInt(address + 0x60);
                IntPtr mdlsAddress = dataInterface.convert4to8(offset);
                byte[] mdlsData = dataInterface.modInterface.memoryInterface.readBytes(mdlsAddress, 32);
                char[] cData = new char[mdlsData.Length];
                for (int i = 0; i < cData.Length && i < mdlsData.Length ; i++)
                {
                    cData[i] = (char)mdlsData[i];
                }
                return new string(cData);
            }

            set
            {
                int offset = dataInterface.modInterface.memoryInterface.readInt(address + 0x60);
                IntPtr mdlsAddress = dataInterface.convert4to8(offset);
                char[] cData = value.ToCharArray();
                byte[] mdlsData = new byte[32];
                for (int i = 0; i < mdlsData.Length && i < cData.Length; i++)
                {
                    mdlsData[i] = (byte)cData[i];
                }
                dataInterface.modInterface.memoryInterface.writeBytes(mdlsAddress, mdlsData);
            }
        }

        public string MSET
        {
            get
            {
                int offset = dataInterface.modInterface.memoryInterface.readInt(address + 0x64);
                IntPtr msetAddress = dataInterface.convert4to8(offset);
                byte[] msetData = dataInterface.modInterface.memoryInterface.readBytes(msetAddress, 32);
                char[] cData = new char[msetData.Length];
                for (int i = 0; i < cData.Length && i < msetData.Length; i++)
                {
                    cData[i] = (char)msetData[i];
                }
                return new string(cData);
            }

            set
            {
                int offset = dataInterface.modInterface.memoryInterface.readInt(address + 0x64);
                IntPtr msetAddress = dataInterface.convert4to8(offset);
                char[] cData = value.ToCharArray();
                byte[] msetData = new byte[32];
                for (int i = 0; i < msetData.Length && i < cData.Length; i++)
                {
                    msetData[i] = (byte)cData[i];
                }
                dataInterface.modInterface.memoryInterface.writeBytes(msetAddress, msetData);
            }
        }

        public string toString()
        {
            return string.Format("Actor Name: {0}\nMovability: {1:F2}\nMDLS: {2}\nMSET: {3}\n", Name, Movability, MDLS, MSET);
        }
    }
}
