using System;
using _27_Task.Model.Components;
using _27_Task.Model.EventArgs;
using _27_Task.Model.Enums;

namespace _27_Task.Model
{
    public class VoterCheckingService
    {
        private VoterInfoChecker _voterChecker;

        public VoterCheckingService(VoterInfoChecker voterChecker)
        {
            _voterChecker = voterChecker ?? throw new ArgumentNullException(nameof(voterChecker));
        }

        public event EventHandler<PassportValidationEventArgs> PassportValidated;
        public event EventHandler<VoterCheckEventArgs> VoterChecked;

        public void Check(string passportInput) 
        {
            var validation = PassportValidator.Validate(passportInput, out var passport);

            if (validation != PassportValidation.Correct)
            {
                PassportValidated?.Invoke(this, new PassportValidationEventArgs(validation));
                return;
            }

            var checking = _voterChecker.Check(passport);

            VoterChecked?.Invoke(this, new VoterCheckEventArgs(checking, passportInput));
        }
    }
}
