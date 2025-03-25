using TeachingBlazorApp.Infrastructure.Models.Requests.Identity;
using System.Security.Claims;
using System.Threading.Tasks;
using TeachingBlazorApp.Infrastructure.Models.Requests.Identity;
using IResult = TeachingBlazorApp.Infrastructure.Utils.Wrapper.IResult;

namespace TeachingBlazorApp.Infrastructure.Managers.Identity.Authentication
{
    public interface IAuthenticationManager
    {
        Task<IResult> Login(TokenRequest _tokenRequest);

        // Task<IResult> Logout();

        // Task<string> RefreshToken();

        // Task<string> TryRefreshToken();

        // Task<string> TryForceRefreshToken();

        // Task<ClaimsPrincipal> CurrentUser();
    }
}