using Microsoft.AspNetCore.Authorization;

namespace SuperMarket.Infrastructure.Framework.Security;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string feature, string action) => Policy = AppPermission.NameFor(feature, action);        
}