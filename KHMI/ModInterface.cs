namespace KHMI
{
    public class ModInterface
    {
        private MemoryInterface memInterface;
        private CodeInterface codInterface;
        private DataInterface datInterface;
        private KHMIEventHandler evHandler;
        private KHMod[] modList = new KHMod[0];
        private bool isClosed = false;


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
            createDefaultHooks();
            evHandler.registerDataEvent("warpEvent", memInterface.nameToAddress("WarpID"), 4);
            evHandler.registerDataEvent("playerLoadedEvent", memInterface.nameToAddress("PlayerEntityPtr"), 8);
            evHandler.registerDataEvent("party1LoadedEvent", memInterface.nameToAddress("Party1EntityPtr"), 8);
            evHandler.registerDataEvent("party2LoadedEvent", memInterface.nameToAddress("Party2EntityPtr"), 8);
            evHandler.registerDataEvent("lockOnEvent", memInterface.nameToAddress("LockOnEntityPtr"), 8);
            evHandler.registerDataEvent("warpTableEvent", memInterface.nameToAddress("WarpTableStartPtr"), 8);
        }

        private void createDefaultHooks()
        {
            IntPtr onHPChangeData = injectOnHPChange();
            evHandler.registerDataEvent("onHPChange", onHPChangeData, 9);
        }

        private IntPtr injectOnHPChange()
        {
            IntPtr dataRegion = codInterface.allocDataRegion(9);
            byte[] code = { 0x83, 0x30, 0x01, 0x48, 0x89, 0x70, 0x01 };
            IntPtr target = memInterface.nameToAddress("onHPChangeHook");
            codInterface.insertDataHook(target, code, dataRegion, 13, false);
            return dataRegion;
        }


        public bool close()
        {
            bool result = codInterface.close();
            return result && memInterface.close();
        }



        public void runEvents(int waitTime=5)
        {
            Dictionary<string, KHMIEvent> runnableEvents = evHandler.checkUpdate(waitTime);
            foreach (string eventName in runnableEvents.Keys)
            {
                bool shouldPause = runnableEvents[eventName].callbackShouldPause;
                foreach (KHMod eachMod in modList)
                {
                    Task t = Task.Run(() =>
                    {
                        eachMod.handleEvent(eventName, runnableEvents[eventName].value, shouldPause);
                    });
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
