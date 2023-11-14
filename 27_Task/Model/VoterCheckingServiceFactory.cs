using _27_Task.Common;
using _27_Task.DataAccess;
using _27_Task.Model.Components;

namespace _27_Task.Model
{
    internal class VoterCheckingServiceFactory
    {
        public VoterCheckingService Create(IDbManager dbManager = null)
        {
            if (dbManager == null)
            {
                dbManager = new DbManager(new DummyLogger());
            }

            return new VoterCheckingService(
                new VoterInfoChecker(
                    new VotersInfoProvider(dbManager)));
        }
    }
}
