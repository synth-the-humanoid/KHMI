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

    public override void onHPChange(Entity target)
    {
        Console.WriteLine("Entity {0}'s HP changed to {1:D}\n", target.Actor.Name, target.StatPage.CurrentHP);
    }
    public override void onEntityDeath(Entity deceased)
    {
        Console.WriteLine("Entity {0} deceased. Entities warped to their deathspot to mourn.", deceased.Actor.Name);
        Vector3 pos = deceased.Position;
        IntPtr firstEnt = modInterface.memoryInterface.nameToAddress("FirstEntity");
        IntPtr lastEntPtr = modInterface.memoryInterface.nameToAddress("FinalEntityPtr");
        IntPtr lastEnt = (IntPtr)modInterface.memoryInterface.readLong(lastEntPtr);
        Entity[] allEntities = new EntityTable(modInterface.dataInterface, firstEnt, lastEnt).Entities;
        foreach(Entity e in allEntities)
        {
            if (e.Actor.Movability != 0)
            {
                e.Position = pos;
            }
        }
    }
}
