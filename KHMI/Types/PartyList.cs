namespace KHMI.Types
{
    public enum PartyMemberID
    {
        SORA = 0,
        DONALD = 1,
        GOOFY = 2,
        TARZAN = 3,
        POOH = 4,
        ALADDIN = 5,
        ARIEL = 6,
        JACK = 7,
        PETERPAN = 8,
        BEAST = 9
    }

    public class PartyList : KHMIType
    {
        public PartyList(DataInterface di, IntPtr address) : base(di, address) { }

        public static PartyList Current(DataInterface di)
        {
            return new PartyList(di, di.modInterface.memoryInterface.nameToAddress("PartyMemberIDArray"));
        }

        public PartyMemberID[] Members
        {
            get
            {
                PartyMemberID[] members = new PartyMemberID[3];
                for(int i = 0; i < members.Length; i++)
                {
                    members[i] = (PartyMemberID)memoryInterface.readByte(address + i);
                }
                return members;
            }
            set
            {
                for(int i = 0; i < value.Length; i++)
                {
                    memoryInterface.writeByte(address + i, (byte)value[i]);
                }
            }
        }

        public Entity[] Entities
        {
            get
            {
                return [Entity.getPlayer(dataInterface), Entity.getParty(dataInterface, 1), Entity.getParty(dataInterface, 2)];
            }
        }
    }
}
