using KHMI.types;

namespace KHMI
{
    public class KHMod
    {
        protected ModInterface modInterface;
        private IntPtr result4to8P = IntPtr.Zero;

        public KHMod(ModInterface mi)
        {
            modInterface = mi;
            modInterface.loadMod(this);
        }

        public void handleEvent(string eventName, byte[] data)
        {
            switch (eventName)
            {
                case "warpEvent":
                    warpUpdate(BitConverter.ToInt32(data));
                    break;
                case "playerLoadedEvent":
                    IntPtr playerAddress = (IntPtr)BitConverter.ToInt64(data);
                    if (playerAddress != IntPtr.Zero)
                    {
                        playerLoadedEvent(new Entity(modInterface.dataInterface, playerAddress));
                    }
                    else
                    {
                        playerUnloadedEvent();
                    }
                    break;
                default:
                    break;
            }
        }

        public virtual void warpUpdate(int newWarpID) { }
        public virtual void playerLoadedEvent(Entity newPlayer) { }
        public virtual void playerUnloadedEvent() { }
    }
}
