namespace KHMI.Types
{
    public class ActorTable : KHMITypeTable
    {
        public ActorTable(DataInterface di, IntPtr addressStart, IntPtr addressEnd) : base(di, addressStart, addressEnd, 0x78) { }

        public Actor[] Actors
        {
            get
            {
                IntPtr[] addresses = Addresses;
                Actor[] actors = new Actor[Size];
                for(int i = 0; i<actors.Length; i++)
                {
                    actors[i] = new Actor(dataInterface, addresses[i]);
                }
                return actors;
            }
        }

        public static ActorTable Current(DataInterface di)
        {
            IntPtr actorTableBasePointer = di.modInterface.memoryInterface.nameToAddress("ActorArrayBasePointer");
            IntPtr actorTableEndPointer = di.modInterface.memoryInterface.nameToAddress("ActorArrayEndPointer");
            IntPtr actorTableStart = (IntPtr)di.modInterface.memoryInterface.readLong(actorTableBasePointer);
            IntPtr actorTableEnd = (IntPtr)di.modInterface.memoryInterface.readLong(actorTableEndPointer) - 78;
            return new ActorTable(di, actorTableStart, actorTableEnd);
        }
    }
}
