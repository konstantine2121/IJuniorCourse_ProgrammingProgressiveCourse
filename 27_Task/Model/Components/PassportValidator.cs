using _27_Task.Model.Enums;

namespace _27_Task.Model.Components
{
    public static class PassportValidator
    {
        private const int DigitsInPassport = 10;

        public static PassportValidation Validate(string passportInput, out string passport)
        {
            passport = passportInput;

            if (string.IsNullOrWhiteSpace(passport))
            {
                return PassportValidation.NumberIsEmpty;
            }

            passport = passport.Trim().Replace(" ", string.Empty);

            if (passport.Length < DigitsInPassport)
            {
                return PassportValidation.NumberHasInvalidFormat;
            }

            return PassportValidation.Correct;
        }
    }
}
