namespace KHMI.Types
{
    public class KHAction : KHMIType
    {
        public KHAction(DataInterface di, IntPtr address) : base (di, address) { }

        public static KHAction FromID(DataInterface di, short id)
        {
            int stringOffset = id * 4; 
            IntPtr actionNameBase = di.modInterface.memoryInterface.nameToAddress("ActionNameBase");
            IntPtr stringAddress = di.convert4to8(di.modInterface.memoryInterface.readInt(actionNameBase + stringOffset));
            return new KHAction(di, stringAddress);
        }

        public KHString Name
        {
            get
            {
                return new KHString(dataInterface, address);
            }
        }
    }
}
