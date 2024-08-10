namespace KHMI.Types
{
    public class KHScriptTable : KHMITypeTable
    {
        public KHScriptTable(DataInterface di, IntPtr startAddress, IntPtr endAddress) : base(di, startAddress, endAddress, 0x360) { }

        public static KHScriptTable Current(DataInterface di)
        {
            IntPtr sizePtr = di.modInterface.memoryInterface.nameToAddress("ScriptCounter");
            int size = di.modInterface.memoryInterface.readInt(sizePtr);
            IntPtr arrayBase = di.modInterface.memoryInterface.nameToAddress("ScriptArrayBase");
            IntPtr arrayEnd = arrayBase + (size * 0x360);
            return new KHScriptTable(di, arrayBase, arrayEnd);
        }

        public KHScript[] Scripts
        {
            get
            {
                IntPtr[] addresses = Addresses;
                KHScript[] events = new KHScript[addresses.Length];
                for(int i = 0; i < events.Length; i++)
                {
                    events[i] = new KHScript(dataInterface, addresses[i]);
                }

                return events;
            }
        }
    }
}
