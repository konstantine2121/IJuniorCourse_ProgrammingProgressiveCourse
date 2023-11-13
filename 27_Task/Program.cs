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
            DbManager dbManager = new DbManager(new DummyLogger());
            dbManager.CreateDatabase();
            dbManager.FillData();

            return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
