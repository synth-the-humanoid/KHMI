namespace KHMI.Types
{
    public class WarpTable : KHMITypeTable
    {
        public WarpTable(DataInterface di, IntPtr addressStart, IntPtr addressEnd) : base(di, addressStart, addressEnd, 0x40) { }

        public Warp[] Warps
        {
            get
            {
                IntPtr[] addresses = Addresses;
                Warp[] warps = new Warp[addresses.Length];
                for(int i = 0; i < warps.Length; i++)
                {
                    warps[i] = new Warp(dataInterface, addresses[i]);
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
