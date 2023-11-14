﻿using _27_Task.Model.Enums;

namespace _27_Task.Model.EventArgs
{
    internal class PassportValidationEventArgs : System.EventArgs
    {
        public PassportValidationEventArgs(PassportValidation checkResult)
        {
            CheckResult = checkResult;
        }

        public PassportValidation CheckResult { get; }
    }
}
