﻿namespace KHMI.Types
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
