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
                case "frameUpdate":
                    frameUpdate();
                    break;
                default:
                    break;
            }
        }

        public virtual void frameUpdate() { }
    }
}
