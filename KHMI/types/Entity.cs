using System.Numerics;

namespace KHMI.types
{
    public class Entity
    {
        private IntPtr address;
        private DataInterface dataInterface;
        

        public Entity(DataInterface di, IntPtr entityAddress)
        {
            dataInterface = di;
            address = entityAddress;
        }

        public Vector3 Position
        {
            get
            {
                return new Vector3(dataInterface.modInterface.memoryInterface.readFloat(address + 0x10), dataInterface.modInterface.memoryInterface.readFloat(address + 0x14), dataInterface.modInterface.memoryInterface.readFloat(address + 0x18));
            }

            set
            {
                dataInterface.modInterface.memoryInterface.writeFloat(address + 0x10, value.X);
                dataInterface.modInterface.memoryInterface.writeFloat(address + 0x14, value.Y);
                dataInterface.modInterface.memoryInterface.writeFloat(address + 0x18, value.Z);
            }
        }

        public Vector3 Rotation
        {
            get
            {
                return new Vector3(dataInterface.modInterface.memoryInterface.readFloat(address + 0x20), dataInterface.modInterface.memoryInterface.readFloat(address + 0x24), dataInterface.modInterface.memoryInterface.readFloat(address + 0x28));
            }

            set
            {
                dataInterface.modInterface.memoryInterface.writeFloat(address + 0x20, value.X);
                dataInterface.modInterface.memoryInterface.writeFloat(address + 0x24, value.Y);
                dataInterface.modInterface.memoryInterface.writeFloat(address + 0x28, value.Z);
            }
        }

        public StatPage StatPage
        {
            get
            {
                IntPtr statPageAddress = (IntPtr)dataInterface.convert4to8(dataInterface.modInterface.memoryInterface.readInt(address + 0x6C));
                return new StatPage(dataInterface, statPageAddress);
            }
        }

        public PortraitInfo PortraitInfo
        {
            get
            {
                int offset = dataInterface.modInterface.memoryInterface.readInt(address + 0x140);
                if (offset == 0)
                {
                    return null;
                }
                return new PortraitInfo(dataInterface, dataInterface.convert4to8(offset));
            }
        }

        private string vec3toString(Vector3 v)
        {
            return string.Format("X: {0:F2}\nY: {1:F2}\nZ: {2:F2}\n", v.X, v.Y, v.Z);
        }

        public string toString()
        {
            string pos = string.Format("Position:\n{0}\n", vec3toString(Position));
            string rot = string.Format("Rotation\n{0}\n", vec3toString(Rotation));
            string sp = StatPage.toString();
            string pi = "";
            if (PortraitInfo != null)
            {
                pi = "PortraitInfo Info:\n" + PortraitInfo.toString();
            }

            return string.Format("{0}\n{1}\nStatPage Info:\n{2}\n{3}\n", pos, rot, sp, pi);
        }
    }
}
