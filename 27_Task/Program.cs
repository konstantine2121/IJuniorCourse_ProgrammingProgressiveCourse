using System;
using System.Windows.Forms;
using _27_Task.Common;
using _27_Task.DataAccess;

namespace _27_Task
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var dbManager = new DbManager(new DummyLogger());

            Application.Run(
                new EnterForm(
                    dbManager, 
                    CreateCheckForm(dbManager)));
        }

        private static CheckPassportForm CreateCheckForm(DbManager dbManager)
        {
            return new CheckPassportForm(
                new VotersInfoProvider(dbManager));
        }
    }
}
