namespace SuperMarket.Infrastructure.Framework.Security;

public class CustomAuthException : Exception
{
    public string FriendlyErrorMessage { get; set; }

    public CustomAuthException(string friendlyMessage) : base(friendlyMessage)
    {
        FriendlyErrorMessage = friendlyMessage;
    }
}