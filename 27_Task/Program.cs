using System;
using System.Windows.Forms;

namespace _27_Task
{
    internal static partial class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(EnterFormFactory.CreateEnterForm());
        }


    }
}
