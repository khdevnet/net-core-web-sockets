using System.Security.Principal;
using NightChat.Web.Application.Authorization;
using NightChat.Web.Application.Authorization.Facebook.Models;

namespace NightChat.Web.Application.Authorization.Facebook
{
    public interface IFacebookAuthorization
    {
        void Authorize(CodeModel codeModel);

        bool IsAuthorized(IPrincipal principal);

        bool TryGetUser(IIdentity identity, out UserIdentity user);
    }
}