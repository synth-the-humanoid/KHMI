namespace KHMI.Types
{
    public class KHWString : KHMIType
    {
        private Dictionary<short, char> asciiChars = new Dictionary<short, char>();

        private enum Charset { SYMBOL=0x81, ASCII=0x82, INTL=0x83 };
        public KHWString(DataInterface di, IntPtr address) : base(di, address)
        {
            loadDictionary();
        }

        private void loadStringToDict(Charset prefix, byte start, string s)
        {
            char[] chars = s.ToCharArray();
            short i = start;
            foreach(char c in chars)
            {
                short currentIndex = (short)(((short)i << 8) | (short)prefix);
                i++;
                asciiChars[currentIndex] = c;
            }
        }

        private void loadDictionary()
        {
            string asciiCharsFirst = "0123456789"; 
            string asciiCharsSecond = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string asciiCharsThird = "abcdefghijklmnopqrstuvwxyz";

            loadStringToDict(Charset.ASCII, 0x4F, asciiCharsFirst);
            loadStringToDict(Charset.ASCII, 0x60, asciiCharsSecond);
            loadStringToDict(Charset.ASCII, 0x81, asciiCharsThird);
            asciiChars[0x4081] = ' ';
        }

        public int Length
        {
            get
            {
                int i = 0;
                while(true)
                {
                    short current = memoryInterface.readShort(address + (i * 2));
                    if (current == 0)
                    {
                        return i;
                    }
                    i++;
                }
            }
        }

        public int Size
        {
            get
            {
                return (Length + 1) * 2;
            }
        }

        public short[] Data
        {
            get
            {
                short[] shorts = new short[Length];
                for(int i = 0; i < shorts.Length; i++)
                {
                    shorts[i] = memoryInterface.readShort(address + (2 * i));
                }
                return shorts;
            }
            set
            {
                short[] shorts = value;
                for(int i = 0; i < shorts.Length; i++)
                {
                    memoryInterface.writeShort(address + (2 * i), shorts[i]);
                }
            }
        }

        public string ASCII
        {
            get
            {
                string text = "";
                short[] shorts = Data;
                foreach(short s in shorts)
                {
                    if (s == 0)
                    {
                        break;
                    }
                    if (asciiChars.ContainsKey(s))
                    {
                        text += asciiChars[s];
                    }
                }
                return text;
            }
            set
            {
                Dictionary<char, short> inverse = new Dictionary<char, short>();
                foreach(short s in asciiChars.Keys)
                {
                    inverse[asciiChars[s]] = s;
                }
                char[] chars = value.ToCharArray();
                short[] shorts = new short[chars.Length + 1];
                for(int i = 0; i<chars.Length; i++)
                {
                    if (inverse.ContainsKey(chars[i]))
                    {
                        shorts[i] = inverse[chars[i]];
                    }
                    else
                    {
                        shorts[i] = inverse['0'];
                    }
                }
                shorts[chars.Length] = 0;

                Data = shorts;
            }
        }

        public override string ToString()
        {
            return ASCII;
        }
    }
}
