using System.Reflection;

namespace KHMI
{
    public class ModLoader
    {
        private string offsets;
        private string mods;
        private ModInterface modInterface;
        private Provider prov;
        private string vString;
        private bool isClosed = false;

        public ModLoader(Provider p, string version, string offsetFile="./offsets.csv", string modsFile="./mods/")
        {
            offsets = offsetFile;
            mods = modsFile;
            prov = p;
            vString = version;
        }

        public bool attach()
        {
            MemoryInterface mi = new MemoryInterface(prov, vString, offsets);
            if (mi.locateProcess())
            {
                modInterface = new ModInterface(mi);
                loadMods();
                return true;
            }
            return false;
        }

        private void loadMods()
        {
            if(!Directory.Exists(mods))
            {
                Directory.CreateDirectory(mods);
            }
            string[] files = Directory.GetFiles(mods);
            foreach(string file in files)
            {
                Assembly DLL = Assembly.LoadFile(Path.GetFullPath(file));
                Type[] exportedTypes = DLL.GetExportedTypes();
                foreach(Type type in exportedTypes)
                {
                    if(type.IsSubclassOf(typeof(KHMod)))
                    {
                        Activator.CreateInstance(type, args:[modInterface]);
                    }
                }
            }
        }

        public void runEvents(int waitTime=50)
        {
            if (!isClosed)
            {
                modInterface.runEvents(waitTime);
            }
        }

        public bool close()
        {
            isClosed = true;
            return modInterface.close();
        }

        public bool Runnable
        {
            get
            {
                return !isClosed;
            }
        }
    }
}
