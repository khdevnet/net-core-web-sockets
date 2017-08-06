using System.Security.Principal;
using NightChat.Web.Common.Authorization;
using NightChat.Web.Facebook.Models;

namespace NightChat.Web.Facebook.Authorization
{
    public interface IFacebookAuthorization
    {
        void Authorize(CodeModel codeModel);

        bool IsAuthorized(IPrincipal principal);

        bool TryGetUser(IIdentity identity, out UserIdentity user);
    }
}