using System;
using System.Collections.Generic;
using _27_Task.Model;
using _27_Task.Model.Enums;

namespace _27_Task.Presenter
{
    public class PassportCheckerPresenter
    {
        private const string NotFound = "Паспорт «{0}» в списке участников дистанционного голосования НЕ НАЙДЕН";
        private const string CheckMessageFormat = "По паспорту «{0}» доступ к бюллетеню на дистанционном электронном голосовании {1}";
        private const string Alowed = "ПРЕДОСТАВЛЕН";
        private const string NotAlowed = "НЕ ПРЕДОСТАВЛЕН";
        
        private const string PassportEmpty = "Введите серию и номер паспорта";
        private const string PassportHasInvalidFormat = "Неверный формат серии или номера паспорта";

        Dictionary<PassportValidation, string> _validationMap = new Dictionary<PassportValidation, string>()
        {
            [PassportValidation.NumberIsEmpty] = PassportEmpty,
            [PassportValidation.NumberHasInvalidFormat] = PassportHasInvalidFormat,
        };

        public PassportCheckerPresenter(VoterCheckingService model, ICheckPassportForm passportForm)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            View = passportForm ?? throw new ArgumentNullException(nameof(passportForm));

            View.RegisterPresenter(this);

            Model.PassportValidated += OnPassportValidated;
            Model.VoterChecked += OnVoterChecked; ;
        }

        private VoterCheckingService Model { get; }

        private ICheckPassportForm View { get; }


    public void Check()
        {
            Model.Check(View.PassportNumber);
        }

        private void OnVoterChecked(object sender, Model.EventArgs.VoterCheckEventArgs e)
        {
            var result = e.CheckResult;
            var passport = e.PassportNumber;
            var message = string.Empty;

            switch (result) 
            {
                case VoterCheckResult.VoterNotFound:
                    message = string.Format(NotFound, passport);
                    break;

                case VoterCheckResult.VoteAllowed:
                case VoterCheckResult.VoteNotAllowed:
                    var canVote = result == VoterCheckResult.VoteAllowed;
                    message = GetCheckMessage(passport, canVote);
                    break;
            }

            View.VoterCheckResult = message;
        }

        private void OnPassportValidated(object sender, Model.EventArgs.PassportValidationEventArgs e)
        {
            var result = e.CheckResult;

            if (result == PassportValidation.Correct)
            {
                return;
            }

            if (_validationMap.TryGetValue(result, out var message))
            {
                View.ShowInputTip(message);
            }
            else
            {
                throw new InvalidOperationException(result +" not supported.");
            }
        }

        private string GetCheckMessage(string passport, bool canVote)
        {
            return string.Format(
                CheckMessageFormat,
                passport, 
                canVote ? Alowed : NotAlowed);
        }
    }
}
