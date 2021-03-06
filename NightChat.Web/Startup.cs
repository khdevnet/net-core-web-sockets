﻿using Autofac;
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
using NightChat.Domain.Extensibility.Dto;
using NightChat.Infrastructure.DataAccess;
using NightChat.Web.Authentication;
using NightChat.Web.Authentication.Facebook;
using NightChat.Web.Extensibility.Authentication;
using NightChat.Web.Extensibility.Authentication.Facebook;
using NightChat.Web.Extensibility.Authentication.Facebook.Models;
using NightChat.Web.Extensibility.Providers;
using NightChat.Web.Extensibility.Sockets;
using NightChat.Web.Providers;
using NightChat.Web.Sockets;
using NightChat.Web.Sockets.SocketMessageProcessor;

namespace NightChat.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
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
                AuthenticationScheme = Constants.AuthCookieName,
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

            app.UseWebSockets();
            app.UseMiddleware<WebSocketProcessingMiddleware>();
            AutoMapperConfigure();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Chat}/{action=Index}/{id?}");
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<DataAccessAutofacModule>();
            builder.RegisterModule<DomainAutofacModule>();
            builder.RegisterModule<CoreAutofacModule>();
            builder.RegisterType<SocketMessageProcessor>().As<ISocketMessageProcessor>();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType<FacebookHttpSender>().As<IFacebookHttpSender>();
            builder.RegisterType<FacebookAuthentication>().As<IFacebookAuthentication>();
            builder.RegisterType<CookieAuthenticationService>().As<ICookieAuthenticationService>();
            builder.RegisterType<UrlProvider>().As<IUrlProvider>().SingleInstance();
            builder.RegisterType<FacebookLoginUrlProvider>().As<IFacebookLoginUrlProvider>().SingleInstance();
            builder.RegisterType<FacebookRedirectUrlProvider>().As<IFacebookRedirectUrlProvider>().SingleInstance();
            builder.RegisterType<WebSocketConnectionManager>().As<IWebSocketConnectionMannager>().SingleInstance();
            builder.RegisterType<WebSocketHandler>().As<IWebSocketHandler>();
        }

        private static void AutoMapperConfigure()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<UserInfoModel, UserData>().ForMember(x => x.Avatar, opt => opt.MapFrom(s => s.Picture.Data.Url));
                config.CreateMap<TokenModel, TokenData>();

                config.AddProfile<DomainAutoMapperProfile>();
            });
        }
    }
}