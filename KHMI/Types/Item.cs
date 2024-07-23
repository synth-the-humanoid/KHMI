namespace KHMI.Types
{
    public class Item : KHMIType
    {
        public Item(DataInterface di, IntPtr address) : base(di, address) { }

        public static Item FromID(DataInterface di, byte id)
        {
            int offset = ((id - 1) * 5) * 4;
            IntPtr baseAddressPtr = di.modInterface.memoryInterface.nameToAddress("ItemInfoPtr");
            IntPtr baseAddress = (IntPtr)di.modInterface.memoryInterface.readLong(baseAddressPtr);
            return new Item(di, baseAddress + offset);
        }

        public static Item FromRewardID(DataInterface di, int rewardID)
        {
            if(rewardID != 0)
            {
                IntPtr rewardBase = di.modInterface.memoryInterface.nameToAddress("RewardTableBase");
                IntPtr rewardAddress = rewardBase + (rewardID * 2);
                short rewardData = di.modInterface.memoryInterface.readShort(rewardAddress);
                byte itemID = (byte)(rewardData >> 4);
                return Item.FromID(di, itemID);
            }
            return null;
        }

        public KHString Name
        {
            get
            {
                int stringOffset = memoryInterface.readShort(address) * 4;
                IntPtr itemNameBase = memoryInterface.nameToAddress("ItemNameBase");
                IntPtr stringAddress = dataInterface.convert4to8(memoryInterface.readInt(itemNameBase + stringOffset));
                return new KHString(dataInterface, stringAddress);
            }
        }

        public byte ItemID
        {
            get
            {
                IntPtr baseAddressPtr = memoryInterface.nameToAddress("ItemInfoPtr");
                IntPtr baseAddress = (IntPtr)memoryInterface.readLong(baseAddressPtr);
                int offset = (int)(address - baseAddress);
                return (byte) (((offset / 4) / 5) - 1); 
            }
        }

        public byte IconID
        {
            get
            {
                return memoryInterface.readByte(address + 0x2);
            }
            set
            {
                memoryInterface.writeByte(address + 0x2, value);
            }
        }

        public KHString Description
        {
            get
            {
                int offset = memoryInterface.readInt(address + 0x0C);
                IntPtr stringAddress = dataInterface.convert4to8(offset);
                return new KHString(dataInterface, stringAddress);
            }
        }

        public override string ToString()
        {
            return string.Format("Item:\nName: {0}\nIcon ID: {1:D}\nDescription: {2}\n", Name.ToString(), IconID, Description.ToString());
        }
    }
}
