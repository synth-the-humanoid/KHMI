using KHMI;

Console.WriteLine("KHMI\nPress enter to attempt to link to game.");
Console.ReadLine();

ModLoader modLoader = new ModLoader(Provider.EPIC, "1.0.0.9");

while (!modLoader.attach())
{
    Console.WriteLine("Failed to link to game. Press enter to retry, or type \"quit\" to end.");
    string result = Console.ReadLine();
    if (result == "quit")
    {
        modLoader = null;
        break;
    }
}

if (modLoader != null)
{
    Console.WriteLine("Process linked successfully. Running. Press enter to quit.");

    Task t = Task.Run(() =>
    {
        while(modLoader.Runnable)
        {
            modLoader.runEvents();
            Thread.Sleep(1);
        }
    });

    while(modLoader.Runnable)
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo cki = Console.ReadKey();
            if (cki.Key == ConsoleKey.Enter)
            {
                modLoader.close();
            }
        }
    }
}

Console.WriteLine("Exiting process.");