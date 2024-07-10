using KHMI;
using KHMI.Types;
using System.Numerics;


Console.WriteLine("Press enter to attempt to load Kingdom Hearts Final Mix");
Console.ReadLine();

MemoryInterface memInt = new MemoryInterface(Provider.EPIC, "1.0.0.9", "./offsets.csv");

do
{
    memInt.locateProcess();
    if (!memInt.isLinked)
    {
        Console.WriteLine("Did not locate process. Make sure the game is running and press enter to retry.");
        Console.ReadLine();
    }
}
while(!memInt.isLinked);

Console.WriteLine("Process located. Press enter to load test mod.");
Console.ReadLine();

ModInterface mi = new ModInterface(memInt);
new TestMod(mi);

Console.WriteLine("Loaded test mod. Press enter to begin running test mod. Afterwards, press enter again to stop KHMI.");
Console.ReadLine();

while (true)
{
    if (Console.KeyAvailable)
    {
        ConsoleKeyInfo cki = Console.ReadKey();
        if (cki.Key == System.ConsoleKey.Enter)
        {
            break;
        }
    }

    mi.runEvents();
}

Console.WriteLine("Closing KHMI.");
mi.close();

class TestMod : KHMod
{
    public TestMod(ModInterface mi) : base(mi)
    {

    }

    public override void warpTableUpdate(WarpTable wt)
    {
        Warp[] warps = wt.Warps;
        Random r = new Random();
        r.Shuffle(warps);
        for (int i = 0; i < warps.Length; i++)
        {
            Warp current = wt.Warps[i];
            current.PlayerPosition = warps[i].PlayerPosition;
            current.PlayerRotation = warps[i].PlayerRotation;
            current.Party1Position = warps[i].Party1Position;
            current.Party1Rotation = warps[i].Party1Rotation;
            current.Party2Position = warps[i].Party2Position;
            current.Party2Rotation = warps[i].Party2Rotation;
        }
    }

    public override void playerLoaded(Entity newPlayer)
    {
        Entity[] entities = Entity.getLoadedEntities(modInterface.dataInterface);
        Vector3[] positions = new Vector3[entities.Length];
        for(int i = 0; i < positions.Length; i++)
        {
            positions[i] = entities[i].Position;
        }
        Random r = new Random();
        r.Shuffle(positions);
        for(int i = 0; i < positions.Length; i++)
        {
            entities[i].Position = positions[i];
        }
    }
}
