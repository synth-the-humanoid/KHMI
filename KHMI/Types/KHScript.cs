namespace KHMI.Types
{
    public class KHScript : KHMIType
    {
        public KHScript(DataInterface di, IntPtr address) : base(di, address) { }

        public int EventID
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

        public Entity Entity
        {
            get
            {
                IntPtr entityAddress = (IntPtr)memoryInterface.readLong(address + 0x8);
                if(entityAddress == IntPtr.Zero)
                {
                    return null;
                }
                return new Entity(dataInterface, entityAddress);
            }
            set
            {
                memoryInterface.writeLong(address + 0x8, value.EntityPtr);
            }
        }

        public IntPtr CodePtr
        {
            get
            {
                return (IntPtr)memoryInterface.readLong(address + 0x80);
            }
            set
            {
                memoryInterface.writeLong(address + 0x80, value);
            }
        }

        public int ReactionEventOffset
        {
            get
            {
                return memoryInterface.readInt(address + 0xA4);
            }
            set
            {
                memoryInterface.writeInt(address + 0xA4, value);
            }
        }

        public int CollisionEventOffset
        {
            get
            {
                return memoryInterface.readInt(address + 0xA8);
            }
            set
            {
                memoryInterface.writeInt(address + 0xA8, value);
            }
        }

        public int TargetedEventOffset
        {
            get
            {
                return memoryInterface.readInt(address + 0xAC);
            }
            set
            {
                memoryInterface.writeInt(address + 0xAC, value);
            }
        }

        public int CurrentOffset
        {
            get
            {
                return memoryInterface.readInt(address + 0x190);
            }
            set
            {
                memoryInterface.writeInt(address + 0x190, value);
            }
        }
    }
}
