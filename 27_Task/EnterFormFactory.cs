using _27_Task.Common;
using _27_Task.DataAccess;
using _27_Task.Model;
using _27_Task.Presenter;

namespace _27_Task
{
    internal static partial class Program
    {
        private class EnterFormFactory
        {
            public static EnterForm CreateEnterForm()
            {
                var dbManager = new DbManager(new DummyLogger());

                var checkerService = new VoterCheckingServiceFactory().Create(dbManager);
                var checkForm = new CheckPassportForm();
                var presenter = new PassportCheckerPresenter(checkerService, checkForm);

                return new EnterForm(dbManager, checkForm);
            }
        }
    }

}
