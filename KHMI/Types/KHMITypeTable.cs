namespace KHMI.Types
{
    public class KHMITypeTable : KHMIType
    {
        private IntPtr endAddress;
        private int objSize;
        public KHMITypeTable(DataInterface di, IntPtr addressStart, IntPtr addressEnd, int oSize) : base (di, addressStart)
        {
            endAddress = addressEnd;
            objSize = oSize;
        }

        public int Size
        {
            get
            {
                return (int)((endAddress - address) / objSize);
            }
        }

        public IntPtr[] Addresses
        {
            get
            {
                IntPtr[] values = new IntPtr[Size];
                for(int i = 0; i < values.Length; i++)
                {
                    values[i] = address + (i * objSize);
                }
                return values;
            }
        }

        protected IntPtr addressEnd
        {
            get
            {
                return endAddress;
            }
        }

        protected int objectSize
        {
            get
            {
                return objSize;
            }
        }
    }
}
