using KHMI;

// simple tester program to showcase the dll - inserts a jump table in the main loop with code that sets world id to 1 (destiny islands) and room id to 0 (seashore)


MemoryInterface mi = new MemoryInterface(Provider.EPIC, "1.0.0.9"); // creates memory interface for EPIC version 1.0.0.9. affects memory offsets
bool isLinked = false;

Console.WriteLine("Attempting to link to Kingdom Hearts 1 Final Mix.");

do
{
    Console.WriteLine("Press enter to attempt to link.");
    Console.ReadLine();
    isLinked = mi.locateProcess();
    if (!isLinked)
    {
        Console.WriteLine("Failed to locate process.");
    }
}
while (!isLinked);

Console.WriteLine("Linked to process. Writing code");
DataInterface di = new DataInterface(mi);
di.WorldID = 0;
di.RoomID = 1;
di.WarpID = 0;
di.SceneID = 0;
Console.WriteLine("Success! Press Enter to End!"); // ends the app
mi.close();