namespace KHMI
{
    public class KHMIEvent
    {
        private CodeInterface cInterface;
        private byte[] cachedVal;
        private IntPtr eventAddress;
        private bool codeEvent;
        private bool pauseCallback;
        public KHMIEvent(CodeInterface ci, IntPtr address, int size, bool isCodeEvent=false, bool pauseOnCallback=false)
        {
            if (ci != null)
            {
                cInterface = ci;
                codeEvent = isCodeEvent;
                pauseCallback = pauseOnCallback;
                int eventSize = size;
                if (codeEvent)
                {
                    eventSize = 1;
                    eventAddress = cInterface.allocDataRegion(eventSize);
                    cInterface.memoryInterface.writeByte(eventAddress, 0);
                    cInterface.insertHook(address, assembleCodeEvent(eventAddress), size);
                }
                else
                {
                    
                    eventAddress = address;
                }
                cachedVal = cInterface.memoryInterface.readBytes(eventAddress, eventSize);
            }
        }

        private byte[] assembleCodeEvent(IntPtr dataAddress)
        {
            byte[] prefix = { 0x50, 0x48, 0xB8 };
            byte[] addressBytes = BitConverter.GetBytes(dataAddress);
            byte[] postfix = { 0x83, 0x30, 0x01, 0x58 };
            byte[] payload = new byte[prefix.Length + addressBytes.Length + postfix.Length];

            int i = 0;
            foreach(byte b in prefix)
            {
                payload[i++] = b;
            }
            foreach (byte b in addressBytes)
            {
                payload[i++] = b;
            }
            foreach (byte b in postfix)
            {
                payload[i++] = b;
            }

            return payload;
        }

        public bool checkUpdate()
        {
            byte[] newVal = cInterface.memoryInterface.readBytes(eventAddress, cachedVal.Length);
            bool hasChanged = false;
            for(int i = 0; i < newVal.Length; i++)
            {
                hasChanged = hasChanged || (newVal[i] != cachedVal[i]);
            }
            if (hasChanged)
            {
                cachedVal = newVal;
            }
            return hasChanged;
        }

        public byte[] value
        {
            get
            {
                return cachedVal;
            }
        }

        public CodeInterface codeInterface
        {
            get
            {
                return cInterface;
            }
        }

        public bool isCodebased
        {
            get
            {
                return codeEvent;
            }
        }

        public bool callbackShouldPause
        {
            get
            {
                return pauseCallback;
            }
        }
    }
}
