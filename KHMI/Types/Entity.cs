using System.Numerics;

namespace KHMI.Types
{
    public class Entity : KHMIType
    {
        public Entity(DataInterface di, IntPtr address) : base(di, address) { }

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
