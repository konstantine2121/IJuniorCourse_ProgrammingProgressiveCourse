using System;
using System.Windows.Forms;
using _27_Task.DataAccess;
using _27_Task.Model;

namespace _27_Task.Presenter
{
    internal class EnterPresenter
    {
        private readonly EnterService _enterService;
        private readonly IEnterForm _enterForm;

        public EnterPresenter(EnterService enterService, IEnterForm enterForm)
        {
            _enterService = enterService ?? throw new ArgumentNullException(nameof(enterService));
            _enterForm = enterForm ?? throw new ArgumentNullException(nameof(enterForm));
            _enterService.DatabaseRestored += OnDatabaseRestored;
        }

        public void Enter()
        {
            if (_enterService.CheckDatabaseExists())
            {
                _enterForm.ShowNextForm();
                return;
            }

            var message =
                $"Файл базы данных '{DbManager.DatabaseFile}' не найден." + Environment.NewLine +
                "Желаете загрузить резервную копию?";

            var dialogResult = MessageBox.Show(
                message,
                "БД недоступна.",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                _enterService.RestoreDatabase();
            }
            else
            {
                MessageBox.Show("Выход из системы.");
                _enterForm.Close();
            }
        }

        private void OnDatabaseRestored(object sender, System.EventArgs e)
        {
            _enterForm.ShowNextForm();
        }
    }
}
