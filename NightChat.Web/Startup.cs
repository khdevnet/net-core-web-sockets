using System;
using AspNetCoreChatRoom.Facebook;
using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NightChat.Core;
using NightChat.Domain;
using NightChat.Domain.Dto;
using NightChat.Web.Common;
using NightChat.Web.Common.Authorization;
using NightChat.Web.Facebook;
using NightChat.Web.Facebook.Authorization;
using NightChat.Web.Facebook.Models;
using NightChat.DataAccess;
using NightChat.Infrastructure;

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

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<FacebookOauthOptions>(oauthOptions =>
            {
                Configuration.GetSection("FacebookOauthOptions").Bind(oauthOptions);
            });

            services.AddMvc();
            services.AddSession();
        }

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
                    OnValidatePrincipal = CookieValidatator.ValidateAsync
                }
            });

            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120)
            };

            app.UseWebSockets(webSocketOptions);
            app.UseMiddleware<WebSocketProcessingMiddleware>();
            AutoMapperConfigure();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<DataAccessAutofacModule>();
            builder.RegisterModule<DomainAutofacModule>();
            builder.RegisterModule<CoreAutofacModule>();
            builder.RegisterModule<SocketsInfrastructureAutofacModule>();

            builder.RegisterType<FacebookHttpSender>().As<IFacebookHttpSender>();
            builder.RegisterType<FacebookAuthorization>().As<IFacebookAuthorization>();
            builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
            builder.RegisterType<UrlProvider>().As<IUrlProvider>().SingleInstance();
            builder.RegisterType<FacebookLoginUrlProvider>().As<IFacebookLoginUrlProvider>().SingleInstance();
            builder.RegisterType<FacebookRedirectUrlProvider>().As<IFacebookRedirectUrlProvider>().SingleInstance();
        }

        private static void AutoMapperConfigure()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<UserInfoModel, UserData>()
                .ForCtorParam("avatar", opt => opt.MapFrom(src => src.Picture.Data.Url));
                config.CreateMap<TokenModel, TokenData>();

                config.AddProfile<DomainAutoMapperProfile>();
            });
        }
    }
}