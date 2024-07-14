namespace KHMI.Types
{
    public class KHMITypeTable : KHMIType
    {
        private IntPtr endAddress;
        private int objSize;
        private bool isValid = false;
        public KHMITypeTable(DataInterface di, IntPtr addressStart, IntPtr addressEnd, int oSize) : base (di, addressStart)
        {
            if(addressStart != 0)
            {
                if (addressEnd != 0)
                {
                    if (oSize != 0) {
                        endAddress = addressEnd;
                        objSize = oSize;
                        isValid = true;
                    }
                }
            }
        }

        public int Size
        {
            get
            {
                if (isValid)
                {
                    return (int)((endAddress - address) / objSize);
                }
                return 0;
            }
        }

        public IntPtr[] Addresses
        {
            get
            {
                if (isValid)
                {
                    IntPtr[] values = new IntPtr[Size];
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = address + (i * objSize);
                    }
                    return values;
                }
                return new IntPtr[0];
            }
        }

        protected IntPtr addressEnd
        {
            get
            {
                if (isValid)
                {
                    return endAddress;
                }
                return IntPtr.Zero;
            }
        }

        protected int objectSize
        {
            get
            {
                if (isValid)
                {
                    return objSize;
                }
                return 0;
            }
        }
    }
}
