using System;
using System.Windows.Forms;
using _27_Task.DataAccess;
using _27_Task.Utils;

namespace _27_Task
{
    public partial class CheckPassportForm : Form
    {
        private const string CheckMessageFormat = "\"По паспорту «\"{0}\"» доступ к бюллетеню на дистанционном электронном голосовании {1}";
        private const string Alowed = "ПРЕДОСТАВЛЕН";
        private const string NotAlowed = "НЕ ПРЕДОСТАВЛЕН";

        private const int DigitsInPassport = 10;
        private readonly VotersInfoProvider _votersInfoProvider;
        private readonly HashCalculator _hashCalculator = HashCalculator.CreateSha256Calculator();

        public CheckPassportForm(VotersInfoProvider votersInfoProvider)
        {
            InitializeComponent();
            _votersInfoProvider = votersInfoProvider ?? throw new ArgumentNullException(nameof(votersInfoProvider));
        }

        private string PassportTextValue => passportTextbox.Text.Trim();

        private void checkButton_Click(object sender, EventArgs e)
        {
            if (CheckPassportInput(out var passport))
            {
                var hash = _hashCalculator.CalculateHash(passport);
                var records = _votersInfoProvider.FindInfo(hash);

                if (records.Count < 1)
                {
                    PrintNotFound();
                }
                else
                {
                    PrintAlowed(records[0].CanVote);
                }
            }
        }

        private void PrintNotFound()
        {
            textResult.Text = "Паспорт «" + passportTextbox.Text + "» в списке участников дистанционного голосования НЕ НАЙДЕН";
        }

        private void PrintAlowed(bool canVote)
        {
            textResult.Text = string.Format(
                CheckMessageFormat, 
                passportTextbox.Text, 
                canVote ? Alowed : NotAlowed);
        }

        #region Input Validation

        private bool CheckPassportInput(out string passport)
        {
            passport = PassportTextValue;

            if (passport == string.Empty)
            {
                MessageBox.Show("Введите серию и номер паспорта");
                return false;
            }

            passport = passport.Replace(" ", string.Empty);

            if (!Validate(passport))
            {
                textResult.Text = "Неверный формат серии или номера паспорта";
                return false;
            }

            return true;
        }

        private bool Validate(string value)
        {
            return !string.IsNullOrWhiteSpace(value) && 
                value.Length >= DigitsInPassport ;
        }

        #endregion Input Validation
    }
}
