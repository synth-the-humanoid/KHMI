namespace KHMI
{
    public class KHMod
    {
        protected ModInterface modInterface;
        public KHMod(ModInterface mi)
        {
            modInterface = mi;
        }

        public void handleEvent(string eventName, byte[] data)
        {
            switch (eventName)
            {
                case "warpEvent":
                    warpUpdate(BitConverter.ToInt32(data));
                    break;
                default:
                    break;
            }
        }

        public virtual void warpUpdate(int newWarpID) { }
    }
}
