using System.Numerics;

namespace KHMI.Types
{
    public class Warp : KHMIType
    {
        public Warp(DataInterface di, IntPtr address) : base(di, address) { }

        public static Warp FromID(DataInterface di, int id, WarpTable wt=null)
        {
            if(wt == null)
            {
                wt = WarpTable.Current(di);
            }
            return wt.Warps[id];
        }

        public static Warp Current(DataInterface di)
        {
            IntPtr warpIDAddress = di.modInterface.memoryInterface.nameToAddress("WarpID");
            int warpID = di.modInterface.memoryInterface.readInt(warpIDAddress);
            return Warp.FromID(di, warpID, WarpTable.Current(di));
        }

        public int RoomID
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

        public Room Room
        {
            get
            {
                return new Room(dataInterface, World.Current(dataInterface), RoomID);
            }
        }

        private Vector3 getPos(int id)
        {
            id++;
            return memoryInterface.readVector3(address + (0x10 * id));
        }

        private void setPos(int id, Vector3 pos)
        {
            id++;
            memoryInterface.writeVector3(address + (0x10 * id), pos);
        }

        private float getRot(int id)
        {
            id++;
            return memoryInterface.readFloat((address + (0x10 * id)) + 0xC);
        }

        private void setRot(int id,  float rot)
        {
            id++;
            memoryInterface.writeFloat((address + (0x10 * id)) + 0xC, rot);
        }

        public Vector3 PlayerPosition
        {
            get
            {
                return getPos(0);
            }
            set
            {
                setPos(0, value);
            }
        }

        public float PlayerRotation
        {
            get
            {
                return getRot(0);
            }
            set
            {
                setRot(0, value);
            }
        }

        public Vector3 Party1Position
        {
            get
            {
                return getPos(1);
            }
            set
            {
                setPos(1, value);
            }
        }

        public float Party1Rotation
        {
            get
            {
                return getRot(1);
            }
            set
            {
                setRot(1, value);
            }
        }

        public Vector3 Party2Position
        {
            get
            {
                return getPos(2);
            }
            set
            {
                setPos(2, value);
            }
        }

        public float Party2Rotation
        {
            get
            {
                return getRot(2);
            }
            set
            {
                setRot(2, value);
            }
        }

        private string getString(int id)
        {
            Vector3 pos = getPos(id);
            return string.Format("X: {0:F2}\nY: {1:F2}\nZ: {2:F2}\nRotation: {3:F2}\n", pos.X, pos.Y, pos.Z, getRot(id));
        }

        public override string ToString()
        {
            string player = string.Format("Player:\n{0}\n", getString(0));
            string party1 = string.Format("Party 1:\n{0}\n", getString(1));
            string party2 = string.Format("Party 2:\n{0}\n", getString(2));
            string warpInfo = string.Format("{0}{1}{2}", player, party1, party2);

            return string.Format("Warp:\nRoom ID: {0:D}\n{1}\n", RoomID, warpInfo);
        }
    }
}
