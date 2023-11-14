using System;
using _27_Task.DataAccess;
using _27_Task.Model.Enums;
using _27_Task.Utils;

namespace _27_Task.Model.Components
{
    public class VoterInfoChecker
    {
        private readonly HashCalculator _hashCalculator = HashCalculator.CreateSha256Calculator();
        private VotersInfoProvider _votersInfoProvider;

        public VoterInfoChecker(VotersInfoProvider votersInfoProvider)
        {
            _votersInfoProvider = votersInfoProvider ?? throw new ArgumentNullException(nameof(votersInfoProvider));
        }

        public VoterCheckResult Check(string passport)
        {
            var hash = _hashCalculator.CalculateHash(passport);
            var records = _votersInfoProvider.FindInfo(hash);

            if (records.Count < 1)
            {
                return VoterCheckResult.VoterNotFound;
            }
            else
            {
                var canVote = records[0].CanVote;

                return canVote ?
                    VoterCheckResult.VoteAllowed :
                    VoterCheckResult.VoteNotAllowed;
            }
        }
    }
}
