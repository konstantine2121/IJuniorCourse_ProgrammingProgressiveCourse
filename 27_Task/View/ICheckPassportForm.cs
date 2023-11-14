using _27_Task.Presenter;

namespace _27_Task
{
    public interface ICheckPassportForm
    {
        string PassportNumber { get; }

        string VoterCheckResult { get; set; }

        void RegisterPresenter(PassportCheckerPresenter presenter);

        void ShowInputTip(string message);
    }
}