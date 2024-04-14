﻿namespace SuperMarket.Infrastructure.Framework.Validations;

public class CustomValidationException : Exception
{
    public List<string> ErrorMessages { get; set; }

    public string FriendlyErrorMessage { get; set; }

    public CustomValidationException(List<string> errorMessages, string friendlyMessage) : base(friendlyMessage)
    {
        ErrorMessages = errorMessages;
        FriendlyErrorMessage = friendlyMessage;
    }
}