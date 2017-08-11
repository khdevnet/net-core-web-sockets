using System.Security.Authentication;
using System.Security.Principal;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NightChat.Domain.Dto;
using NightChat.Domain.Services;
using NightChat.Web.Application.Authorization.Facebook.Models;

namespace NightChat.Web.Application.Authorization.Facebook
{
    public class FacebookAuthorization : IFacebookAuthorization
    {
        private readonly IFacebookHttpSender facebookHttpSender;
        private readonly IUsersService usersService;
        private readonly ITokensService tokensService;
        private readonly ICookieAuthenticationService formsAuthenticationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FacebookAuthorization(
            IFacebookHttpSender facebookHttpSender,
            IUsersService usersService,
            ITokensService tokensService,
            ICookieAuthenticationService formsAuthenticationService,
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
                    usersService.AddOrUpdateUser(Mapper.Map<UserData>(userModel));
                    tokensService.AddOrUpdateToken(userModel.Id, token.AccessToken, token.ExpiresInSeconds);
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
            user = httpContextAccessor.HttpContext.User.Identity as UserIdentity;
            return user != null;
        }
    }
}