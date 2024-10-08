﻿namespace KHMI.Types
{
    public class Actor : KHMIType
    {
        public Actor(DataInterface di, IntPtr address) : base(di, address) { }

        public float Movability
        {
            get
            {
                return memoryInterface.readFloat(address + 0x18);
            }
            set
            {
                memoryInterface.writeFloat(address + 0x18, value);
            }
        }

        public int ChestRewardID
        {
            get
            {
                return memoryInterface.readInt(address + 0x48);
            }
            set
            {
                memoryInterface.writeInt(address + 0x48, value);
            }
        }

        public Item ChestReward
        {
            get
            {
                return Item.FromChestRewardID(dataInterface, ChestRewardID);
            }
            set
            {
                IntPtr rewardBase = memoryInterface.nameToAddress("ChestRewardTableBase");
                IntPtr rewardAddress = rewardBase + (ChestRewardID * 2);
                short rewardData = (short)(value.ItemID << 4);
                memoryInterface.writeShort(rewardAddress, rewardData);
            }
        }

        public bool IsChest
        {
            get
            {
                return ChestRewardID != 0;
            }
        }

        public byte InteractionID
        {
            get
            {
                return memoryInterface.readByte(address + 0x5F);
            }
            set
            {
                memoryInterface.writeByte(address + 0x5F, value);
            }
        }

        public string MDLS
        {
            get
            {
                int offset = memoryInterface.readInt(address + 0x60);
                IntPtr mdlsAddress = dataInterface.convert4to8(offset);
                byte[] data = memoryInterface.readBytes(mdlsAddress, 32);
                char[] cData = new char[data.Length];
                for (int i = 0; i < cData.Length; i++)
                {
                    if (data[i] == 0)
                    {
                        cData[i] = '\0';
                        break;
                    }
                    cData[i] = (char)data[i];
                }
                return new string(cData);
            }
            set
            {
                int offset = memoryInterface.readInt(address + 0x60);
                IntPtr mdlsAddress = dataInterface.convert4to8(offset);
                char[] cData = value.ToCharArray();
                byte[] data = new byte[32];
                for(int i = 0; i < cData.Length && i < data.Length; i++)
                {
                    data[i] = (byte)cData[i];
                }
                data[data.Length - 1] = 0;
                memoryInterface.writeBytes(mdlsAddress, data);
            }
        }

        public string MSET
        {
            get
            {
                int offset = memoryInterface.readInt(address + 0x64);
                IntPtr msetAddress = dataInterface.convert4to8(offset);
                byte[] data = memoryInterface.readBytes(msetAddress, 32);
                char[] cData = new char[data.Length];
                for (int i = 0; i < cData.Length; i++)
                {
                    if (data[i] == 0)
                    {
                        cData[i] = '\0';
                        break;
                    }
                    cData[i] = (char)data[i];
                }
                return new string(cData);
            }
            set
            {
                int offset = memoryInterface.readInt(address + 0x64);
                IntPtr msetAddress = dataInterface.convert4to8(offset);
                char[] cData = value.ToCharArray();
                byte[] data = new byte[32];
                for (int i = 0; i < cData.Length && i < data.Length; i++)
                {
                    data[i] = (byte)cData[i];
                }
                data[data.Length - 1] = 0;
                memoryInterface.writeBytes(msetAddress, data);
            }
        }

        public string Name
        {
            get
            {
                byte[] data = memoryInterface.readBytes(address + 0x68, 16);
                char[] cData = new char[data.Length];
                for (int i = 0; i < cData.Length; i++)
                {
                    if (data[i] == 0)
                    {
                        cData[i] = '\0';
                        break;
                    }
                    cData[i] = (char)data[i];
                }
                return new string(cData);
            }
        }


        public override string ToString()
        {
            return string.Format("Actor:\nName: {0}\nMovability: {1:F2}\nMDLS: {2}\nMSET: {3}\n", Name, Movability, MDLS, MSET);
        }
    }
}
