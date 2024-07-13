using KHMI;
using KHMI.Types;
using System.Numerics;


Console.WriteLine("Press enter to attempt to load Kingdom Hearts Final Mix");
Console.ReadLine();

ModLoader ml = new ModLoader(Provider.EPIC, "1.0.0.9");
ml.attach();

Console.WriteLine("Process located.");



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

    ml.runEvents();
}

Console.WriteLine("Closing KHMI.");
ml.close();