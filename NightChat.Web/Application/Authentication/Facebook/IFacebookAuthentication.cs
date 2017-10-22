using System.Security.Principal;
using NightChat.Web.Application.Authentication;
using NightChat.Web.Application.Authentication.Facebook.Models;

namespace NightChat.Web.Application.Authentication.Facebook
{
    public interface IFacebookAuthentication
    {
        void Authorize(CodeModel codeModel);

        bool IsAuthorized(IPrincipal principal);

        bool TryGetUser(IIdentity identity, out UserIdentity user);
    }
}