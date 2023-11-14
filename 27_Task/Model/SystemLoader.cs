using System;
using _27_Task.DataAccess;

namespace _27_Task.Model
{
    internal class EnterService
    {
        private readonly IDbManager _dbManager;

        public EnterService(IDbManager dbManager) 
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
        }

        public event EventHandler DatabaseRestored;

        public bool CheckDatabaseExists()
        {
            return _dbManager.CheckDbFileExists();
        }

        public void RestoreDatabase()
        {
            _dbManager.CreateDatabase();
            _dbManager.FillVotersInfo();
            
            DatabaseRestored?.Invoke(this, System.EventArgs.Empty);
        }
    }
}
