using _27_Task.Model.Enums;

namespace _27_Task.Model.EventArgs
{
    public class VoterCheckEventArgs : System.EventArgs
    {
        public VoterCheckEventArgs(VoterCheckResult checkResult, string passportNumber)
        {
            CheckResult = checkResult;
            PassportNumber = passportNumber;
        }

        public VoterCheckResult CheckResult { get; }
        public string PassportNumber { get; }
    }
}
