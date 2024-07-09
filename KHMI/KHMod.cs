using KHMI.Types;

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

        internal void handleEvent(string eventName, byte[] data, bool shouldPause)
        {
            if (shouldPause)
            {
                modInterface.codeInterface.StartDebug();
                modInterface.codeInterface.DebugPause();
            }
            switch (eventName)
            {
                case "warpEvent":
                    warpUpdate(BitConverter.ToInt32(data));
                    break;
                case "playerLoadedEvent":
                    IntPtr playerAddress = (IntPtr)BitConverter.ToInt64(data);
                    if (playerAddress != IntPtr.Zero)
                    {
                        playerLoaded(new Entity(modInterface.dataInterface, playerAddress));
                    }
                    else
                    {
                        playerUnloaded();
                    }
                    break;
                case "lockOnEvent":
                    IntPtr entityAddress = (IntPtr)BitConverter.ToInt64(data);
                    if (entityAddress == IntPtr.Zero)
                    {
                        playerLockOff();
                    }
                    else
                    {
                        playerLockOn(new Entity(modInterface.dataInterface, entityAddress));
                    }
                    break;
                default:
                    break;
            }

            if (shouldPause)
            {
                modInterface.codeInterface.DebugUnpause();
                modInterface.codeInterface.StopDebug();
            }
        }

        public virtual void warpUpdate(int newWarpID) { }
        public virtual void playerLoaded(Entity newPlayer) { }
        public virtual void playerUnloaded() { }
        public virtual void playerLockOff() { }
        public virtual void playerLockOn(Entity target) { }
    }
}
