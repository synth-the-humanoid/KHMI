namespace KHMI
{
    public class KHMIEventHandler
    {
        private Dictionary<string,KHMIEvent> activeEvents = new Dictionary<string,KHMIEvent>();
        private CodeInterface cInterface;

        public KHMIEventHandler(CodeInterface ci)
        {
            if (ci != null)
            {
                cInterface = ci;
            }
        }

        public void registerDataEvent(string eventName, IntPtr address, int size, bool pauseOnCallback=false)
        {
            KHMIEvent newEvent = new KHMIEvent(cInterface, address, size, false, pauseOnCallback);
            activeEvents[eventName] = newEvent;
        }

        public void registerCodeEvent(string eventName, IntPtr address, int codeSize, bool pauseOnCallback=false)
        {
            KHMIEvent newEvent = new KHMIEvent(cInterface, address, codeSize, true, pauseOnCallback);
            activeEvents[eventName] = newEvent;
        }

        public Dictionary<string, KHMIEvent> checkUpdate()
        {
            Dictionary<string, KHMIEvent> runnableEvents = new Dictionary<string, KHMIEvent>();
            foreach(string eachKey in activeEvents.Keys)
            {
                if (activeEvents[eachKey].checkUpdate())
                {
                    runnableEvents[eachKey] = activeEvents[eachKey];
                }
            }
            return runnableEvents;
        }

        public CodeInterface codeInterface
        {
            get
            {
                return cInterface;
            }
        }

        public KHMIEvent eventByName(string eventName)
        {
            if (activeEvents.ContainsKey(eventName))
            {
                return activeEvents[eventName];
            }
            return null;
        }
    }
}
