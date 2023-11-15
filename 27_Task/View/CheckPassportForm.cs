using System;
using System.Windows.Forms;
using _27_Task.Presenter;

namespace _27_Task
{
    public partial class CheckPassportForm : Form, ICheckPassportForm
    {
        private const string TipCaption = "Ошибка ввода";
        
        #region Fields

        private PassportCheckerPresenter _presenter;

        #endregion Fields

        #region Ctor

        public CheckPassportForm()
        {
            InitializeComponent();
        }

        #endregion Ctor

        #region ICheckPassportForm Implementation

        public string PassportNumber => passportTextBox.Text;

        public string VoterCheckResult
        {
            get => resultTextBox.Text;
            set => resultTextBox.Text = value;
        }

        public void RegisterPresenter(PassportCheckerPresenter presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
        }

        public void ShowInputTip(string message)
        {
            MessageBox.Show(message, TipCaption);
        }

        #endregion ICheckPassportForm Implementation

        #region Event Handlers

        private void OnCheckClick(object sender, EventArgs e)
        {
            _presenter?.Check();
        }

        #endregion Event Handlers
    }
}
