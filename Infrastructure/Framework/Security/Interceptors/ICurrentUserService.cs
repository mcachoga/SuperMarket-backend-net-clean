namespace SuperMarket.Infrastructure.Framework.Security;

public interface ICurrentUserService
{
    public string UserId { get; }
}