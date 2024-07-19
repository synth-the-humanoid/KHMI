using KHMI;

Console.WriteLine("KHMI\n");
string[] providers = VersionInfo.SupportedProviders;
string selection = "";
string prompt = "Type the desired Provider selection and press enter to begin.";
for (int i = 0; i < providers.Length; i++)
{
    prompt = string.Format("{0}\n{1:D}: {2}", prompt, i + 1, providers[i]);
}

do
{
    Console.WriteLine(prompt);
    string response = Console.ReadLine();
    int choice = 0;
    int.TryParse(response, out choice);
    if (choice > 0 && choice <= providers.Length)
    {
        selection = providers[choice-1];
    }
    else
    {
        Console.WriteLine("Invalid choice. Try again.");
    }
}
while (selection == "");

Provider provider = VersionInfo.StringToProvider(selection);
string[] versions = VersionInfo.GetSupportedVersions(provider);
prompt = "Type the desired Version selection and press enter to begin.";
for (int i = 0; i < versions.Length; i++)
{
    prompt = string.Format("{0}\n{1:D}: {2}", prompt, i + 1, versions[i]);
}

selection = "";
do
{
    Console.WriteLine(prompt);
    string response = Console.ReadLine();
    int choice = 0;
    int.TryParse(response, out choice);
    if(choice > 0 && choice <= versions.Length)
    {
        selection = versions[choice-1];
    }
    else
    {
        Console.WriteLine("Invalid choice. Try again.");
    }
}
while (selection == "");


ModLoader modLoader = new ModLoader(provider, selection);

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