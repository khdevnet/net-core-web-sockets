using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NightChat.WebApi.Common.Authorization;
using NightChat.WebApi.Common.Services;
using NightChat.WebApi.Facebook.Models;

namespace NightChat.WebApi.Facebook.Authorization
{
    public class FacebookAuthorization : IFacebookAuthorization
    {
        private readonly IFacebookHttpSender facebookHttpSender;
        private readonly IUsersService usersService;
        private readonly ITokensService tokensService;
        private readonly IFormsAuthenticationService formsAuthenticationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FacebookAuthorization(
            IFacebookHttpSender facebookHttpSender,
            IUsersService usersService,
            ITokensService tokensService,
            IFormsAuthenticationService formsAuthenticationService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.facebookHttpSender = facebookHttpSender;
            this.usersService = usersService;
            this.tokensService = tokensService;
            this.formsAuthenticationService = formsAuthenticationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public void Authorize(CodeModel codeModel)
        {
            TokenModel token = facebookHttpSender.GetToken(codeModel.Code);
            if (token != null)
            {
                UserInfoModel userModel = facebookHttpSender.GetUserDetails(token.AccessToken);
                if (userModel != null)
                {
                    usersService.AddOrUpdateUser(userModel);
                    tokensService.AddOrUpdateToken(userModel.Id, token);
                    formsAuthenticationService.SetAuthCookie(userModel, token);
                    return;
                }
            }

            throw new AuthenticationException("Facebook Authentication failed");
        }

        public bool IsAuthorized(IPrincipal principal)
        {
            UserIdentity user;
            return principal.Identity.IsAuthenticated && TryGetUser(principal.Identity, out user) && tokensService.Validate(user.Id);
        }

        public bool TryGetUser(IIdentity identity, out UserIdentity user)
        {
         var s =   httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            user = httpContextAccessor.HttpContext.User.Identity as UserIdentity;
            return user != null;
        }

        //public bool TryGetUser(IIdentity identity, out UserIdentity user)
        //{
        //    user = null;
        //    var formsIdentity = identity as ClaimsIdentity;
        //    if (formsIdentity != null)
        //    {
        //        user = JsonConvert.DeserializeObject<UserIdentity>(formsIdentity.S Ticket.UserData);
        //    }
        //    return user != null;
        //}
    }
}