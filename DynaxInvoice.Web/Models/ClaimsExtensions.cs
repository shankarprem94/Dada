using System;
using System.Security.Claims;
using System.Security.Principal;

namespace DynaxInvoice.Web.Models
{
    public static class ClaimsExtensions
    {
        public static Claim FindClaim(this IPrincipal user, string claimType)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrEmpty(claimType)) throw new ArgumentNullException(nameof(claimType));

            var claimsPrincipal = user as ClaimsPrincipal;
            return claimsPrincipal?.FindFirst(claimType);
        }

        public static string GetEmail(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, ClaimTypes.Email)?.Value;
        }

        public static string GetName(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            return FindClaim(user, ClaimTypes.Name)?.Value;
        }
        //public static int? GetRoleId(this IPrincipal user)
        //{
        //    if (user == null) throw new ArgumentNullException(nameof(user));

        //    string value = FindClaim(user, ClaimTypes.Role)?.Value;
        //    if (string.IsNullOrEmpty(value)) return default(int?);

        //    int result;
        //    return int.TryParse(value, out result) ? result : default(int?);
        //}
        public static string GetRole(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

           return FindClaim(user, ClaimTypes.Role)?.Value;          
        }

        public static int? GetUserId(this IPrincipal user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            string value = FindClaim(user, ClaimTypes.Sid)?.Value;
            if (string.IsNullOrEmpty(value)) return default;

            int result;
            return int.TryParse(value, out result) ? result : default(int?);
        }
    }
}