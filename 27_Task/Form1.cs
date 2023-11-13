using System.Windows.Forms;
using _27_Task.DataAccess;

namespace _27_Task
{
    public partial class Form1 : Form
    {
        private const int DigitsInPassport = 10;
        private readonly VotersInfoProvider _votersInfoProvider = new VotersInfoProvider();

        public Form1()
        {
            InitializeComponent();
        }

        private string PassportTextValue => passportTextbox.Text.Trim();

        private void checkButton_Click(object sender, System.EventArgs e)
        {
            if (CheckPassportInput(out var passport))
            {

            }
        }

        private bool CheckPassportInput(out string passport)
        {
            passport = PassportTextValue;

            if (passport == string.Empty)
            {
                MessageBox.Show("Введите серию и номер паспорта");
                return false;
            }

            passport = passport.Replace(" ", string.Empty);

            if (Validate(passport) == false)
            {
                textResult.Text = "Неверный формат серии или номера паспорта";
                return false;
            }

            return true;
        }


        private bool Validate(string value)
        {
            return string.IsNullOrEmpty(value) == false || value.Length >= DigitsInPassport;
        }
    }
}
