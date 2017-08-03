using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetCoreChatRoom.Facebook;
using Autofac;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NightChat.Web.Common.Authorization;
using NightChat.WebApi.Common;
using NightChat.WebApi.Common.Authorization;
using NightChat.WebApi.Common.Data;
using NightChat.WebApi.Common.Data.Repositories;
using NightChat.WebApi.Common.Services;
using NightChat.WebApi.Facebook;
using NightChat.WebApi.Facebook.Authorization;
using Plugin.Http.Extensibility.Senders;
using Plugin.Http.Senders;

namespace NightChat.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<FacebookOauthOptions>(oauthOptions =>
            {
                Configuration.GetSection("FacebookOauthOptions").Bind(oauthOptions);
            });
            // Add framework services.
            services.AddMvc();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSession();

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationScheme = AuthorizationConstants.AuthCookieName,
                LoginPath = new PathString("/chat/login"),
                AccessDeniedPath = new PathString("/chat/NotAuthorized"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                Events = new CookieAuthenticationEvents
                {
                    // Set other options
                    OnValidatePrincipal = LastChangedValidator.ValidateAsync
                }
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<FacebookHttpSender>().As<IFacebookHttpSender>();
            builder.RegisterType<HttpMessageSender>().As<IHttpMessageSender>();
            builder.RegisterType<FacebookAuthorization>().As<IFacebookAuthorization>();
            builder.RegisterType<TokensService>().As<ITokensService>();
            builder.RegisterType<UsersService>().As<IUsersService>();
            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
            builder.RegisterType<SessionDataContext>().As<ISessionDataContext>().SingleInstance();
            builder.RegisterType<UsersRepository>().As<IUsersRepository>();
            builder.RegisterType<TokensRepository>().As<ITokensRepository>();
            builder.RegisterType<UrlProvider>().As<IUrlProvider>().SingleInstance();
            builder.RegisterType<FacebookLoginUrlProvider>().As<IFacebookLoginUrlProvider>().SingleInstance();
            builder.RegisterType<FacebookRedirectUrlProvider>().As<IFacebookRedirectUrlProvider>().SingleInstance();
        }
    }
    public static class LastChangedValidator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            ClaimsPrincipal userPrincipal = context.Principal;
            if (!userPrincipal.Identity.IsAuthenticated)
            {
                await context.HttpContext.Authentication.SignOutAsync(AuthorizationConstants.AuthCookieName);
            }

            var sender = context.HttpContext.RequestServices.GetRequiredService<IFacebookHttpSender>();
            var claim = context.Principal.Claims.FirstOrDefault(c => c.Type == "token");

            if (claim != null)
            {
                var r = sender.InspectToken(claim.Value);
                if (r != System.Net.HttpStatusCode.OK)
                {
                    await context.HttpContext.Authentication.SignOutAsync(AuthorizationConstants.AuthCookieName);
                }
            }
        }
    }
}