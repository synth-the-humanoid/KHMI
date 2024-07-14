namespace KHMI
{
    public enum Provider { EPIC, STEAM };

    internal class OffsetHandler
    {
        private Dictionary<string, int> offsets = new Dictionary<string, int>();

        public OffsetHandler(string relPath, Provider p, string version)
        {
            if (File.Exists(relPath))
            {
                string release;
                switch (p)
                {
                    case Provider.EPIC:
                        release = "epic";
                        break;
                    case Provider.STEAM:
                        release = "steam";
                        break;
                    default:
                        release = "jacksparrow";
                        break;
                }
                release += version;

                using (StreamReader sr = new StreamReader(relPath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line.StartsWith("--") || line.StartsWith("//") || line == "")
                        {
                            continue;
                        }
                        string[] fields = line.Split(",");
                        for (int i = 1; i < fields.Length; i += 2)
                        {
                            if (fields[i] == release)
                            {
                                offsets[fields[0]] = Convert.ToInt32(fields[i + 1], 16);
                            }
                        }
                    }
                    sr.Close();
                }
            }
        }

        public int getOffset(string name)
        {
            if (offsets.ContainsKey(name))
            {
                return offsets[name];
            }
            return 0;
        }
    }
}
