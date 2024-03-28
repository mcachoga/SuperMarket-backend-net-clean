using SuperMarket.Common.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace SuperMarket.WebApi.Security
{
    public class MustHavePermissionAttribute : AuthorizeAttribute
    {
        public MustHavePermissionAttribute(string feature, string action) => Policy = AppPermission.NameFor(feature, action);        
    }
}