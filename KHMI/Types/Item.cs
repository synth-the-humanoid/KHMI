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

        public static Item FromChestRewardID(DataInterface di, int rewardID)
        {
            if(rewardID != 0)
            {
                IntPtr rewardBase = di.modInterface.memoryInterface.nameToAddress("ChestRewardTableBase");
                IntPtr rewardAddress = rewardBase + (rewardID * 2);
                short rewardData = di.modInterface.memoryInterface.readShort(rewardAddress);
                byte itemID = (byte)(rewardData >> 4);
                return Item.FromID(di, itemID);
            }
            return null;
        }

        public static Item FromName(DataInterface di, string name)
        {
            int i = 0;
            while(i < 256)
            {
                Item currentItem = Item.FromID(di, (byte)i++);
                if (currentItem.Name.ASCII == name)
                {
                    return currentItem;
                }
            }
            return null;
        }


        public short ActionID
        {
            get
            {
                return memoryInterface.readShort(address);
            }
            set
            {
                memoryInterface.writeShort(address, value);
            }
        }

        public KHAction Action
        {
            get
            {
                return KHAction.FromID(dataInterface, ActionID);
            }
        }

        public KHString Name
        {
            get
            {
                return Action.Name;
            }
        }

        public byte InventoryAmount
        {
            get
            {
                IntPtr inventoryArray = memoryInterface.nameToAddress("InventoryArray");
                return memoryInterface.readByte(inventoryArray + ItemID - 1);
            }
            set
            {
                IntPtr inventoryArray = memoryInterface.nameToAddress("InventoryArray");
                memoryInterface.writeByte(inventoryArray + ItemID - 1, value);
            }
        }

        public byte ItemID
        {
            get
            {
                IntPtr baseAddressPtr = memoryInterface.nameToAddress("ItemInfoPtr");
                IntPtr baseAddress = (IntPtr)memoryInterface.readLong(baseAddressPtr);
                int offset = (int)(address - baseAddress);
                return (byte) (((offset / 4) / 5) + 1); 
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

        public short BuyPrice
        {
            get
            {
                return memoryInterface.readShort(address + 0x8);
            }
            set
            {
                memoryInterface.writeShort(address + 0x8, value);
            }
        }

        public short SellPrice
        {
            get
            {
                return memoryInterface.readShort(address + 0xA);
            }
            set
            {
                memoryInterface.writeShort(address + 0xA, value);
            }
        }

        public override string ToString()
        {
            return string.Format("Item:\nName: {0}\nIcon ID: {1:D}\nDescription: {2}\nBuy Price: {3:D}\nSell Price: {4:D}\n", Name.ToString(), IconID, Description.ToString(), BuyPrice, SellPrice);
        }
    }
}
