using System;
using System.Windows.Forms;
using _27_Task.Presenter;

namespace _27_Task
{
    public partial class CheckPassportForm : Form, ICheckPassportForm
    {
        private const string TipCaption = "Ошибка ввода";
        private const int TipDuration = 5;

        #region Fields

        private PassportCheckerPresenter _presenter;
        private ToolTip _toolTip;

        #endregion Fields

        #region Ctor

        public CheckPassportForm()
        {
            InitializeComponent();

            _toolTip = new ToolTip();
            _toolTip.SetToolTip(passportTextBox, TipCaption);
            passportTextBox.TextChanged += OnTextChanged;
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
            _toolTip.Show(message, this, TipDuration);
        }

        #endregion ICheckPassportForm Implementation

        private void HideTip()
        {
            _toolTip.Hide(this);
        }

        #region Event Handlers

        private void OnCheckClick(object sender, EventArgs e)
        {
            _presenter?.Check();
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            HideTip();
        }

        #endregion Event Handlers
    }
}
