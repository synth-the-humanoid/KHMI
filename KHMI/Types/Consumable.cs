namespace KHMI.Types
{
    public class Consumable : Item
    {
        public Consumable(DataInterface di, IntPtr address) : base(di, address) { }

        public short Strength
        {
            get
            {
                return memoryInterface.readShort(address + 0x6);
            }
            set
            {
                memoryInterface.writeShort(address + 0x6, value);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}\nStrength: {1:D}", base.ToString(), Strength);
        }
    }
}
