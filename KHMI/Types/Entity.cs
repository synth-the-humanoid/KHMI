using System.Numerics;

namespace KHMI.Types
{
    public class Entity : KHMIType
    {
        public Entity(DataInterface di, IntPtr address) : base(di, address) { }

        public static Entity getPlayer(DataInterface di)
        {
            IntPtr playerPtrAddress = di.modInterface.memoryInterface.nameToAddress("PlayerEntityPtr");
            if (playerPtrAddress != IntPtr.Zero)
            {
                IntPtr playerAddress = (IntPtr)di.modInterface.memoryInterface.readLong(playerPtrAddress);
                return new Entity(di, playerAddress);
            }
            return null;
        }

        public static Entity getParty(DataInterface di, int partyID)
        {
            IntPtr entityAddressPtr = IntPtr.Zero;
            switch(partyID)
            {
                case 1:
                    entityAddressPtr = di.modInterface.memoryInterface.nameToAddress("Party1EntityPtr");
                    break;
                case 2:
                    entityAddressPtr = di.modInterface.memoryInterface.nameToAddress("Party2EntityPtr");
                    break;
                default:
                    entityAddressPtr = IntPtr.Zero;
                    break;
            }
            if (entityAddressPtr != IntPtr.Zero)
            {
                IntPtr entityAddress = (IntPtr)di.modInterface.memoryInterface.readLong(entityAddressPtr);
                return new Entity(di, entityAddress);
            }
            return null;
        }

        public IntPtr EntityPtr
        {
            get
            {
                return address;
            }
        }


        public Vector3 Position
        {
            get
            {
                return memoryInterface.readVector3(address + 0x10);
            }
            set
            {
                memoryInterface.writeVector3(address + 0x10, value);
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return memoryInterface.readVector3(address + 0x20);
            }
            set
            {
                memoryInterface.writeVector3(address + 0x20, value);
            }
        }

        public StatPage StatPage
        {
            get
            {
                int offset = memoryInterface.readInt(address + 0x6C);
                if (offset == 0)
                {
                    return null;
                }
                return new StatPage(dataInterface, dataInterface.convert4to8(offset));
            }
        }

        public Actor Actor
        {
            get
            {
                int offset = memoryInterface.readInt(address + 0x130);
                if (offset == 0)
                {
                    return null;
                }
                return new Actor(dataInterface, dataInterface.convert4to8(offset));
            }
        }

        public PortraitFrame PortraitFrame
        {
            get
            {
                int offset = memoryInterface.readInt(address + 0x140);
                if (offset == 0)
                {
                    return null;
                }
                return new PortraitFrame(dataInterface, dataInterface.convert4to8(offset));
            }
        }

        public bool IsPartyMember
        {
            get
            {
                if (StatPage != null)
                {
                    if (StatPage.PartyStatPage != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool IsPlayer
        {
            get
            {
                Entity player = Entity.getPlayer(dataInterface);
                return address == player.address;
            }
        }

        public bool IsChest
        {
            get
            {
                return Actor.IsChest;
            }
        }

        public bool IsEnvironment
        {
            get
            {
                if(StatPage != null)
                {
                    return StatPage.IsEnvironment;
                }
                return false;
            }
        }

        public bool IsEnemy
        {
            get
            {
                return (!IsPartyMember && !IsEnvironment);
            }
        }

        public override string ToString()
        {
            string pos = string.Format("Position:\nX: {0:F2}\nY: {1:F2}\nZ: {2:F2}\n", Position.X, Position.Y, Position.Z);
            string rot = string.Format("Rotation:\nX: {0:F2}\nY: {1:F2}\nZ: {2:F2}\n", Rotation.X, Rotation.Y, Rotation.Z);
            string sp = "";
            string ac = "";
            string pf = "";

            if (StatPage != null)
            {
                sp = StatPage.ToString();
            }
            if (Actor != null)
            {
                ac = Actor.ToString();
            }
            if (PortraitFrame != null)
            {
                pf = PortraitFrame.ToString();
            }

            return string.Format("Entity:\n{0}{1}{2}{3}{4}", pos, rot, sp, ac, pf);
        }
    }
}
