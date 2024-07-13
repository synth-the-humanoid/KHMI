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

    private void setMaxHP(Entity e, int newMaxHP)
    {
        e.StatPage.MaxHP = newMaxHP;
        PartyStatPage psp = e.StatPage.PartyStatPage;
        if (psp != null)
        {
            psp.MaxHP = (byte)newMaxHP;
        }
        Console.WriteLine("{0}'s Max HP cut to {1:D}", e.Actor.Name, newMaxHP);
    }

    public override void onHPChange(Entity target)
    {
        if (target.IsPartyMember)
        {
            int newMaxHP = (target.StatPage.MaxHP + target.StatPage.CurrentHP) / 2;
            setMaxHP(target, newMaxHP);
        }
        else
        {
            Entity player = Entity.getPlayer(modInterface.dataInterface);
            setMaxHP(player, player.StatPage.MaxHP + 1);
        }
    }

    public override void onEntityDeath(Entity deceased)
    {
        if (deceased.IsPartyMember)
        {
            if(!deceased.IsPlayer)
            {
                Entity player = Entity.getPlayer(modInterface.dataInterface);
                setMaxHP(player, player.StatPage.MaxHP * 2 / 3);
                player.StatPage.CurrentHP = Math.Min(player.StatPage.CurrentHP, player.StatPage.MaxHP);
            }
        }
    }
}
