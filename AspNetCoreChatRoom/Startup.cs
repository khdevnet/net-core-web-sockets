using AspNetCoreChatRoom.Facebook;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetCoreChatRoom
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

            services.AddMvc();
            services.AddRouting();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/chat/error");
            }

            app.UseStaticFiles();
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = "MyCookieMiddlewareInstance",
                LoginPath = new PathString("/chat/NotAuthorized/"),
                AccessDeniedPath = new PathString("/chat/NotAuthorized/"),
                AutomaticAuthenticate = true,
                AutomaticChallenge = true
            });
            app.UseWebSockets();
            app.UseMiddleware<ChatWebSocketMiddleware>();
        }
    }
}