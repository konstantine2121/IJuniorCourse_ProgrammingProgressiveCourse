namespace _27_Task.Model.Data
{
    public class VoterInfo
    {
        public VoterInfo(string passport, string name, bool canVote)
        {
            Passport = passport;
            Name = name;
            CanVote = canVote;
        }

        public string Passport { get; }
        public string Name { get; }
        public bool CanVote { get; }
    }
}
