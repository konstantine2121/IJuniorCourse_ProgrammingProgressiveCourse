using System;
using System.Windows.Forms;
using _27_Task.DataAccess;

namespace _27_Task
{
    public partial class EnterForm : Form
    {
        private readonly DbManager _dbManager;
        private readonly CheckPassportForm _checkPassportForm;

        public EnterForm(DbManager dbManager, CheckPassportForm checkPassportForm)
        {
            InitializeComponent();
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
            _checkPassportForm = checkPassportForm;
        }

        private void enterButton_Click(object sender, EventArgs e)
        {
            if (!_dbManager.CheckDbFileExists())
            {
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
                    _dbManager.CreateDatabase();
                    _dbManager.FillVotersInfo();
                }
                else
                {
                    MessageBox.Show("Выход из системы.");
                    Close();
                    return;
                }
            }

            ShowNextForm();
        }

        private void ShowNextForm()
        {
            Hide();
            _checkPassportForm.ShowDialog();
            Close();
        }

       
    }
}
