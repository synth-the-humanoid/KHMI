namespace KHMI.Types
{
    public class WarpTable : KHMIType
    {
        private IntPtr endAddress;
        public WarpTable(DataInterface di, IntPtr addressStart, IntPtr addressEnd=0) : base(di, addressStart)
        {
            if (addressEnd != 0)
            {
                endAddress = addressEnd;
            }
            else
            {
                IntPtr endAddressPtr = memoryInterface.nameToAddress("WarpTableEndPtr");
                endAddress = (IntPtr)memoryInterface.readLong(endAddressPtr);
            }
        }

        public int Size
        {
            get
            {
                return (int)(endAddress - address) / 0x40;
            }
        }

        public Warp[] Warps
        {
            get
            {
                Warp[] warps = new Warp[Size];
                for(int i = 0; i < warps.Length; i++)
                {
                    warps[i] = new Warp(dataInterface, address + (i * 0x40));
                }
                return warps;
            }
        }

        public override string ToString()
        {
            string info = string.Format("Warp Count: {0:D}\n", Size);
            Warp[] warps = Warps;
            for(int i = 0; i < warps.Length; i++)
            {
                info += string.Format("Warp ID: {0:D}\n{1}\n", i, warps[i].ToString());
            }
            return info;
        }
    }
}
