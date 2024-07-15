using System.Reflection;

namespace KHMI
{
    public static class VersionInfo
    {
        public static string[] GetSupportedVersions(Provider provider)
        {
            switch(provider)
            {
                case Provider.EPIC:
                    return ["1.0.0.9"];
                case Provider.STEAM:
                    return ["none"];
                default:
                    return ["none"];
            }
        }

        public static string[] SupportedProviders
        {
            get
            {
                return ["epic"];
            }
        }

        public static Provider StringToProvider(string s)
        {
            switch(s)
            {
                case "epic":
                    return Provider.EPIC;
                case "steam":
                    return Provider.STEAM;
                default:
                    return Provider.JACKSPARROW;
            }
        }
    }
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

        private bool hasExtension(string baseString, string extension)
        {
            string[] splits = baseString.Split(".");
            return splits[splits.Length-1] == extension;
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
                if (hasExtension(file, ".dll"))
                {
                    Assembly DLL = Assembly.LoadFile(Path.GetFullPath(file));
                    Type[] exportedTypes = DLL.GetExportedTypes();
                    foreach (Type type in exportedTypes)
                    {
                        if (type.IsSubclassOf(typeof(KHMod)))
                        {
                            Activator.CreateInstance(type, args: [modInterface]);
                        }
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
