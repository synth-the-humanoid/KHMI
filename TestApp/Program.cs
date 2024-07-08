using KHMI;
using KHMI.types;


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

    public override void warpUpdate(int newWarpID)
    {
        Console.WriteLine("WarpID Update!\nNew WarpID: {0:D}\n", newWarpID);
    }

    public override void playerLoadedEvent(Entity newPlayer)
    {
        Console.WriteLine("Sora Loaded!\n{0}", newPlayer.toString());
    }
}
