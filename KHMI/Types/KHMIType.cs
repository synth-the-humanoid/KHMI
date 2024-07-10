namespace KHMI.Types
{
    public class KHMIType
    {
        private DataInterface datInterface;
        private IntPtr typeAddress;

        public KHMIType(DataInterface di, IntPtr khmiTypeAddress)
        {
            datInterface = di;
            typeAddress = khmiTypeAddress;
        }

        protected DataInterface dataInterface
        {
            get
            {
                return datInterface;
            }
        }

        protected IntPtr address
        {
            get
            {
                return typeAddress;
            }
        }

        protected MemoryInterface memoryInterface
        {
            get
            {
                return datInterface.modInterface.memoryInterface;
            }
        }
    }
}
