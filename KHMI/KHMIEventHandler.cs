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

        public void registerDataEvent(string eventName, IntPtr address, int size, bool pauseOnCallback=false, bool originalCodeFirst=true)
        {
            KHMIEvent newEvent = new KHMIEvent(cInterface, address, size, false, pauseOnCallback, originalCodeFirst);
            activeEvents[eventName] = newEvent;
        }

        public void registerCodeEvent(string eventName, IntPtr address, int codeSize, bool pauseOnCallback=false, bool originalCodeFirst = true)
        {
            KHMIEvent newEvent = new KHMIEvent(cInterface, address, codeSize, true, pauseOnCallback, originalCodeFirst);
            activeEvents[eventName] = newEvent;
        }

        public Dictionary<string, KHMIEvent> checkUpdate(int waitTime=5)
        {
            Dictionary<string, KHMIEvent> runnableEvents = new Dictionary<string, KHMIEvent>();
            foreach(string eachKey in activeEvents.Keys)
            {
                if (activeEvents[eachKey].checkUpdate())
                {
                    runnableEvents[eachKey] = activeEvents[eachKey];
                }
                Thread.Sleep(waitTime);
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
