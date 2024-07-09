namespace KHMI
{
    public class ModInterface
    {
        private MemoryInterface memInterface;
        private CodeInterface codInterface;
        private DataInterface datInterface;
        private KHMIEventHandler evHandler;
        private KHMod[] modList = new KHMod[0];

        public ModInterface(MemoryInterface mi)
        {
            memInterface = mi;
            codInterface = new CodeInterface(memInterface);
            evHandler = new KHMIEventHandler(codInterface);
            createDefaultEvents();
            codInterface.ReloadDebug();
            datInterface = new DataInterface(this);
        }

        public void loadMod(KHMod mod)
        {
            KHMod[] newModList = new KHMod[modList.Length + 1];
            int i = 0;
            foreach(KHMod eachMod in modList)
            {
                newModList[i++] = eachMod;
            }
            newModList[i] = mod;
            modList = newModList;
        }


        private void createDefaultEvents()
        {
            evHandler.registerDataEvent("warpEvent", memInterface.nameToAddress("WarpID"), 4);
            evHandler.registerDataEvent("playerLoadedEvent", memInterface.nameToAddress("PlayerEntityPtr"), 8);
            evHandler.registerDataEvent("lockOnEvent", memInterface.nameToAddress("LockOnEntityPtr"), 8);
        }


        public bool close()
        {
            bool result = codInterface.close();
            return result && memInterface.close();
        }

        public void runEvents(int waitTime=50)
        {
            Dictionary<string, KHMIEvent> runnableEvents = evHandler.checkUpdate(waitTime);
            foreach(string eventName in  runnableEvents.Keys)
            {
                bool shouldPause = runnableEvents[eventName].callbackShouldPause;
                foreach (KHMod eachMod in modList)
                {
                    eachMod.handleEvent(eventName, runnableEvents[eventName].value, shouldPause);
                }
            }
        }

        public KHMIEvent eventByName(string eventName)
        {
            return evHandler.eventByName(eventName);
        }

        public MemoryInterface memoryInterface
        {
            get
            {
                return memInterface;
            }
        }

        public CodeInterface codeInterface
        {
            get
            {
                return codInterface;
            }
        }

        public DataInterface dataInterface
        {
            get
            {
                return datInterface;
            }
        }
    }
}
