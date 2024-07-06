using KHMI;

// simple tester program to showcase the dll


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
CodeInterface ci = new CodeInterface(mi);       // writes code to the main game loop that sets the world id to destiny islands every frame
byte[] prefix = { 0x50, 0x48, 0xB8 }; // push rax ; mov rax,
byte[] address = BitConverter.GetBytes(mi.nameToAddress("WorldID")); // WorldID
byte[] postfix = { 0xC7, 0x00, 0x01, 0x00, 0x00, 0x00, 0x58 }; // mov [rax],1 ; pop rax
byte[] payload = new byte[prefix.Length + address.Length + postfix.Length];

int i = 0;
foreach(byte b in prefix)
{
    payload[i++] = b;
}
foreach (byte b in address)
{
    payload[i++] = b;
}
foreach (byte b in postfix)
{
    payload[i++] = b;
}
ci.insertCode(mi.nameToAddress("MainLoopEntryOffset"), payload, 14); // inserts code at a defined symbol in offsets.csv

Console.WriteLine("Success! Press Enter to Kill!"); // kills the process as it deallocates in-use memory. in a real application, don't close the dll classes until the game closes
ci.close();
mi.close();