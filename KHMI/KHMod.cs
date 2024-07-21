using KHMI.Types;

namespace KHMI
{
    public class KHMod
    {
        protected ModInterface modInterface;
        private int loadedWarp;
        private int loadedWorld;
        private Dictionary<string, int> hpCache = new Dictionary<string, int>();

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
            loadHPCache();
        }

        private void loadHPCache()
        {
            hpCache = new Dictionary<string, int>();
            EntityTable et = EntityTable.Current(modInterface.dataInterface);
            Entity[] entities = et.Entities;

            foreach(Entity e in entities)
            {
                hpCache[e.Actor.Name] = e.StatPage.CurrentHP;
            }
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
                case "roomEvent":
                    Room newRoom = new Room(modInterface.dataInterface, World.Current(modInterface.dataInterface), BitConverter.ToInt32(data));
                    roomUpdate(newRoom);
                    break;
                case "worldEvent":
                    World newWorld = new World(modInterface.dataInterface, BitConverter.ToInt32(data));
                    worldUpdate(newWorld);
                    IntPtr roomIDAddress = modInterface.memoryInterface.nameToAddress("RoomID");
                    Room newWRoom = new Room(modInterface.dataInterface, newWorld, modInterface.memoryInterface.readInt(roomIDAddress));
                    roomUpdate(newWRoom);
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
                case "party1LoadedEvent":
                    IntPtr party1Address = (IntPtr)BitConverter.ToInt64(data);
                    if(party1Address != IntPtr.Zero)
                    {
                        partyLoaded(new Entity(modInterface.dataInterface, party1Address), 1);
                    }
                    else
                    {
                        partyUnloaded(1);
                    }
                    break;
                case "party2LoadedEvent":
                    IntPtr party2Address = (IntPtr)BitConverter.ToInt64(data);
                    if (party2Address != IntPtr.Zero)
                    {
                        partyLoaded(new Entity(modInterface.dataInterface, party2Address), 2);
                    }
                    else
                    {
                        partyUnloaded(2);
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
                            if(hpCache.ContainsKey(target.Actor.Name))
                            {
                                if(target.StatPage.CurrentHP > hpCache[target.Actor.Name])
                                {
                                    onHeal(target);
                                }
                                else
                                {
                                    onDamage(target);
                                }
                            }
                            onHPChange(target);
                        }
                    }
                    break;
                case "cameraStyleEvent":
                    cameraStyleChange(BitConverter.ToInt32(data));
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
        public virtual void roomUpdate(Room newRoom) { }
        public virtual void worldUpdate(World newWorld) { }
        public virtual void playerLoaded(Entity newPlayer) { }
        public virtual void playerUnloaded() { }
        public virtual void partyLoaded(Entity newParty, int partySlot) { }
        public virtual void partyUnloaded(int partySlot) { }
        public virtual void playerLockOff() { }
        public virtual void playerLockOn(Entity target) { }
        public virtual void warpTableUpdate(WarpTable wt) { }
        public virtual void onHPChange(Entity target) { }
        public virtual void onEntityDeath(Entity deceased) { }
        public virtual void onDamage(Entity target) { }
        public virtual void onHeal(Entity target) { }
        public virtual void cameraStyleChange(int newStyle) { }
    }
}
