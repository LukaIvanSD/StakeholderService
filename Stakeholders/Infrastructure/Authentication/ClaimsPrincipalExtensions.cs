using System.Security.Claims;

namespace Stakeholders.Infrastructure.Authentication
{
    public static class ClaimsPrincipalExtensions
    {
        public static long UserId(this ClaimsPrincipal user)
            => long.Parse(user.Claims.First(i => i.Type == "id").Value);
        public static long PersonId(this ClaimsPrincipal user)
            => long.Parse(user.Claims.First(i => i.Type == "personId").Value);
    }
}
