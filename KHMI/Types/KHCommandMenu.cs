namespace KHMI.Types
{
    public class KHCommandMenu : KHMIType
    {
        public KHCommandMenu(DataInterface di, IntPtr address) : base(di, address) { }

        public static KHCommandMenu FromID(DataInterface di,  int id)
        {
            int offset = id * 0x38;
            IntPtr baseAddressPtr = di.modInterface.memoryInterface.nameToAddress("CommandMenuArrayBase");
            IntPtr baseAddress = (IntPtr)di.modInterface.memoryInterface.readLong(baseAddressPtr);
            return new KHCommandMenu(di, baseAddress + offset);
        }

        public static KHCommandMenu GetTopMenu(DataInterface di)
        {
            IntPtr commandMenuArrayBase = di.modInterface.memoryInterface.nameToAddress("CommandMenuArrayBase");
            int size = 0;
            for (int i = 0; size == 0; i++)
            {
                IntPtr current = commandMenuArrayBase + i * 8;
                if (di.modInterface.memoryInterface.readLong(current) == 0)
                {
                    size = i;
                    if(size == 0)
                    {
                        break;
                    }
                }
            }
            IntPtr topMenuPtr = commandMenuArrayBase + (size - 1) * 8;
            IntPtr topMenuAddress = (IntPtr)di.modInterface.memoryInterface.readLong(topMenuPtr);
            return new KHCommandMenu(di, topMenuAddress);
        }

        public int MenuStyle
        {
            get
            {
                return memoryInterface.readInt(address);
            }
            set
            {
                memoryInterface.writeInt(address, value);
            }
        }

        public int CurrentSelection
        {
            get
            {
                return memoryInterface.readInt(address + 0xC);
            }
            set
            {
                memoryInterface.writeInt(address + 0xC, value);
            }
        }
        public int Size
        {
            get
            {
                return memoryInterface.readInt(address + 0x10);
            }
            set
            {
                memoryInterface.writeInt(address + 0x10, value);
            }
        }

        public byte[] CommandArray
        {
            get
            {
                return memoryInterface.readBytes(address + 0x14, Size);
            }
            // no setter because the game has multiple lines of code setting this. change the KHCommand itself to achieve the effects you want
        }

        public KHCommand[] Commands
        {
            get
            {
                byte[] commandIDs = CommandArray;
                KHCommand[] commands = new KHCommand[Size];
                for(int i = 0; i < commands.Length; i++)
                {
                    commands[i] = KHCommand.FromID(dataInterface, commandIDs[i]);
                }
                return commands;
            }
        }

        public int MenuCount
        {
            get
            {
                int i = 0;
                KHCommand[] commands = Commands;
                foreach(KHCommand c in commands)
                {
                    if(c.NextMenuID != 0)
                    {
                        i++;
                    }
                }
                return i;
            }
        }

        public KHCommandMenu[] Menus
        {
            get
            {
                KHCommandMenu[] menus = new KHCommandMenu[MenuCount];
                int commandIterator = 0;
                for(int i = 0; i < Menus.Length; i++)
                {
                    KHCommand current = null;
                    while(current.NextMenu == null)
                    {
                        current = Commands[commandIterator++];
                    }
                    menus[i] = current.NextMenu;
                }
                return menus;
            }
        }
    }
}
