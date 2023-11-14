using System;
using System.Windows.Forms;
using _27_Task.DataAccess;
using _27_Task.Model;
using _27_Task.Presenter;

namespace _27_Task
{
    public partial class EnterForm : Form, IEnterForm
    {
        private readonly CheckPassportForm _checkPassportForm;
        private readonly EnterPresenter _enterPresenter;

        public EnterForm(IDbManager dbManager, CheckPassportForm checkPassportForm)
        {
            InitializeComponent();
            
            _checkPassportForm = checkPassportForm ?? throw new ArgumentNullException(nameof(_checkPassportForm));

            if (dbManager is null)
            {
                throw new ArgumentNullException(nameof(dbManager));
            }
            
            _enterPresenter = new EnterPresenter(new EnterService(dbManager), this);
        }

        private void OnEnterClick(object sender, EventArgs e)
        {
            _enterPresenter.Enter();
        }

        public void ShowNextForm()
        {
            Hide();
            _checkPassportForm.ShowDialog();
            Close();
        }
    }
}
