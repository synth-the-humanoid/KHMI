using KHMI;
using KHMI.Types;

namespace TestDLLMod
{
    public class TestMod : KHMod
    {
        public TestMod(ModInterface mi) : base(mi)
        {
            string data = "";
            for(int i = 0; i <= 16; i++)
            {
                World current = new World(modInterface.dataInterface, i);
                Room[] currentRooms = current.Rooms;
                string currentData = string.Format("World: {0:D}", i);
                foreach(Room r in currentRooms)
                {
                    currentData = string.Format("{0}\nRoom Name: {1}\nScene: {2:D}\n", currentData, r.Name, r.SceneID);
                }
                data += currentData;
            }
            Console.WriteLine(data);
        }
    }
}
