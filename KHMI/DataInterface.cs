namespace KHMI
{

    // class to wrap data management of commonly used variables
    public class DataInterface
    {
        private ModInterface mInterface;
        private IntPtr dataPtr4to8;
        private KHMIEvent conv4to8ev;

        public DataInterface(ModInterface mi)
        {
            if (mi != null)
            {
                mInterface = mi;
                conv4to8ev = hook4to8PConvert();
                
            }
        }

        private KHMIEvent hook4to8PConvert()
        {
            dataPtr4to8 = mInterface.codeInterface.allocDataRegion(12);
            byte[] payload = assemble4to8PConvert(dataPtr4to8);
            mInterface.codeInterface.insertDataHook(mInterface.memoryInterface.nameToAddress("MainLoopEntryOffset"), payload, dataPtr4to8, 14);
            return new KHMIEvent(mInterface.codeInterface, dataPtr4to8+4, 8);
        }

        private byte[] assemble4to8PConvert(IntPtr data)
        {
            byte[] prefix = { 0x51, 0x52, 0x48, 0x31, 0xC9, 0x8B, 0x08, 0x50, 0x48, 0xB8 };
            byte[] functionPtr = BitConverter.GetBytes(mInterface.memoryInterface.nameToAddress("4To8PConvert"));
            byte[] postfix = { 0xFF, 0xD0, 0x59, 0x48, 0x89, 0x41, 0x04, 0x5A, 0x59 };

            byte[] payload = new byte[prefix.Length + functionPtr.Length + postfix.Length];
            
            int i = 0;
            foreach(byte b in prefix)
            {
                payload[i++] = b;
            }
            foreach (byte b in functionPtr)
            {
                payload[i++] = b;
            }
            foreach (byte b in postfix)
            {
                payload[i++] = b;
            }

            return payload;
        }

        public IntPtr convert4to8(int offset, int maxHold=8)
        {
            mInterface.memoryInterface.writeInt(dataPtr4to8, offset);
            int waitTime = 0;
            while (waitTime < maxHold && !conv4to8ev.checkUpdate())
            {
                Thread.Sleep(1);
                waitTime++;
            }
            return (IntPtr)BitConverter.ToInt64(conv4to8ev.value);
        }

        public int WarpID
        {
            get
            {
                return mInterface.memoryInterface.readInt(mInterface.memoryInterface.nameToAddress("WarpID"));
            }
            set
            {
                mInterface.memoryInterface.writeInt(mInterface.memoryInterface.nameToAddress("WarpID"), value);
            }
        }

        public int RoomID
        {
            get
            {
                return mInterface.memoryInterface.readInt(mInterface.memoryInterface.nameToAddress("RoomID"));
            }
            set
            {
                mInterface.memoryInterface.writeInt(mInterface.memoryInterface.nameToAddress("RoomID"), value);
            }
        }

        public int SceneID
        {
            get
            {
                return mInterface.memoryInterface.readInt(mInterface.memoryInterface.nameToAddress("SceneID"));
            }
            set
            {
                mInterface.memoryInterface.writeInt(mInterface.memoryInterface.nameToAddress("SceneID"), value);
            }
        }

        public int WorldID
        {
            get
            {
                return mInterface.memoryInterface.readInt(mInterface.memoryInterface.nameToAddress("WorldID"));
            }
            set
            {
                mInterface.memoryInterface.writeInt(mInterface.memoryInterface.nameToAddress("WorldID"), value);
            }
        }

        public ModInterface modInterface
        {
            get
            {
                return mInterface;
            }
        }
    }
}
