using System.Security.Claims;
using TimMovie.SharedKernel.Validators;

namespace TimMovie.Web.Extensions;

public static class ClaimsPrincipalExtension
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        ArgumentValidator.ThrowExceptionIfNull(claimsPrincipal, nameof(claimsPrincipal));
        
        return Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}