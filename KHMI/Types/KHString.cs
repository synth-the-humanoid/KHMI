namespace KHMI.Types
{
    public class KHString : KHMIType
    {
        private Dictionary<byte, char> asciiChars = new Dictionary<byte, char>();

        public KHString(DataInterface di, IntPtr address) : base(di, address)
        {
            loadDictionary();
        }

        private void loadStringToDict(byte dictStart, string data)
        {
            char[] chars = data.ToCharArray();
            byte i = dictStart;
            byte i2 = 0;
            while(i2 < chars.Length)
            {
                asciiChars[i++] = chars[i2++];
            }
        }

        private void loadDictionary()
        {
            string asciiCharsFirst = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!?&%+-";
            string asciiCharsSecond = "/*.,";
            string asciiCharsThird = ":;";
            string asciiCharsFourth = "~'\"";
            string asciiCharsFifth = "()[]<>";

            loadStringToDict(0x21, asciiCharsFirst);
            loadStringToDict(0x66, asciiCharsSecond);
            loadStringToDict(0x6B, asciiCharsThird);
            loadStringToDict(0x70, asciiCharsFourth);
            loadStringToDict(0x74, asciiCharsFifth);

            asciiChars[0x01] = ' ';
            asciiChars[0x02] = '\n';
        }

        public int Size
        {
            get
            {
                int i = 0;
                while(memoryInterface.readByte(address + i) != 0)
                {
                    i++;
                }
                return i + 1;
            }
        }

        public byte[] Data
        {
            get
            {
                return memoryInterface.readBytes(address, Size);
            }
        }
        public string ASCII
        {
            get
            {
                string ascii = "";
                byte[] data = Data;
                foreach(byte b in data)
                {
                    if (b != 0 && asciiChars.ContainsKey(b))
                    {
                        ascii = ascii + asciiChars[b];
                    }
                }

                return ascii;
            }
            set
            {
                Dictionary<char, byte> inverse = new Dictionary<char, byte>();
                foreach(byte b in asciiChars.Keys)
                {
                    inverse[asciiChars[b]] = b;
                }
                byte[] data = new byte[value.Length + 1];
                for(int i = 0; i < value.Length; i++)
                {
                    if (inverse.ContainsKey(value[i]))
                    {
                        data[i] = inverse[value[i]];
                    }
                }
                data[value.Length] = 0;
                memoryInterface.writeBytes(address, data);
            }
        }

        public override string ToString()
        {
            return ASCII;
        }
    }
}
