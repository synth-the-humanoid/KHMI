using KHMI.Types;

namespace KHMI
{
    public class KHMod
    {
        protected ModInterface modInterface;
        private int loadedWarp;
        private int loadedWorld;

        public KHMod(ModInterface mi)
        {
            modInterface = mi;
            modInterface.loadMod(this);
            updateLoadedRoomInfo();
        }

        private void updateLoadedRoomInfo()
        {
            loadedWarp = modInterface.memoryInterface.readInt(modInterface.memoryInterface.nameToAddress("WarpID"));
            loadedWorld = modInterface.memoryInterface.readInt(modInterface.memoryInterface.nameToAddress("WorldID"));
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
                case "warpTableEvent":
                    IntPtr warpTableEndPtr = modInterface.memoryInterface.nameToAddress("WarpTableEndPtr");
                    IntPtr warpTableEnd = (IntPtr)modInterface.memoryInterface.readLong(warpTableEndPtr);
                    IntPtr warpTableStart = (IntPtr)BitConverter.ToInt64(data);
                    warpTableUpdate(new WarpTable(modInterface.dataInterface, warpTableStart, warpTableEnd));
                    break;
                case "onHPChange":
                    if(modInterface.memoryInterface.readInt(modInterface.memoryInterface.nameToAddress("WorldID")) == loadedWorld)
                    {
                        if(modInterface.memoryInterface.readInt(modInterface.memoryInterface.nameToAddress("WarpID")) == loadedWarp)
                        {
                            byte[] entityPtrBytes = new byte[8];
                            for (int i = 0; i < entityPtrBytes.Length; i++)
                            {
                                entityPtrBytes[i] = data[i + 1];
                            }
                            Entity target = new Entity(modInterface.dataInterface, (IntPtr)BitConverter.ToInt64(entityPtrBytes));
                            if (target.StatPage.CurrentHP == 0)
                            {
                                onEntityDeath(target);
                            }
                            onHPChange(target);
                        }
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

            updateLoadedRoomInfo();
        }

        public virtual void warpUpdate(int newWarpID) { }
        public virtual void playerLoaded(Entity newPlayer) { }
        public virtual void playerUnloaded() { }
        public virtual void playerLockOff() { }
        public virtual void playerLockOn(Entity target) { }
        public virtual void warpTableUpdate(WarpTable wt) { }
        public virtual void onHPChange(Entity target) { }
        public virtual void onEntityDeath(Entity deceased) { }
    }
}
