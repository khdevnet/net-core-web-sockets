using System.Security.Principal;
using NightChat.WebApi.Common.Authorization;
using NightChat.WebApi.Facebook.Models;

namespace NightChat.WebApi.Facebook.Authorization
{
    public interface IFacebookAuthorization
    {
        void Authorize(CodeModel codeModel);

        bool IsAuthorized(IPrincipal principal);

        bool TryGetUser(IIdentity identity, out UserIdentity user);
    }
}