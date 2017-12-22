using System.Security.Principal;
using NightChat.Web.Authentication;
using NightChat.Web.Extensibility.Authentication.Facebook.Models;

namespace NightChat.Web.Extensibility.Authentication.Facebook
{
    public interface IFacebookAuthentication
    {
        void Authorize(CodeModel codeModel);

        bool IsAuthorized(IPrincipal principal);

        bool TryGetUser(IIdentity identity, out UserIdentity user);
    }
}