namespace KHMI
{
    public class ModInterface
    {
        private MemoryInterface memInterface;
        private CodeInterface codInterface;
        private DataInterface datInterface;
        private KHMIEventHandler evHandler;
        private KHMod[] modList;

        public ModInterface(MemoryInterface mi)
        {
            memInterface = mi;
            codInterface = new CodeInterface(memInterface);
            datInterface = new DataInterface(memInterface);
            evHandler = new KHMIEventHandler(codInterface);
            createDefaultEvents();
            codInterface.ReloadDebug();
        }

        public void loadMods(KHMod[] mods)
        {
            modList = mods;
        }

        private void createDefaultEvents()
        {
            evHandler.registerDataEvent("warpEvent", memInterface.nameToAddress("WarpID"), 4);
        }

        public bool close()
        {
            bool result = codInterface.close();
            return result && memInterface.close();
        }

        public void runEvents()
        {
            Dictionary<string, KHMIEvent> runnableEvents = evHandler.checkUpdate();
            foreach(string eventName in  runnableEvents.Keys)
            {
                bool shouldPause = runnableEvents[eventName].callbackShouldPause;
                if (shouldPause)
                {
                    codInterface.DebugPause();
                }
                foreach (KHMod eachMod in modList)
                {
                    eachMod.handleEvent(eventName, runnableEvents[eventName].value);
                }
                if (shouldPause)
                {
                    codInterface.DebugUnpause();
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
