using Microsoft.AspNetCore.Identity;

namespace SuperMarket.Application.Identity.Extensions
{
    internal static class IdentityResultExtensions
    {
        public static List<string> GetErrorDescriptions(this IdentityResult identityResult)
        {
            var errorDescriptions = new List<string>();
            
            foreach (var error in identityResult.Errors)
            {
                errorDescriptions.Add(error.Description);
            }

            return errorDescriptions;
        }
    }
}
