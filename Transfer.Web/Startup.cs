using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Transfer.Bl;
using Transfer.Common;
using Transfer.Common.Cache;
using Transfer.Common.Security;
using Transfer.Dal.Context;
using Transfer.Web.Models;
using Newtonsoft.Json;
using Telegram.Bot;
using Transfer.Web.Services;

namespace Transfer.Web;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
        _transferSettings = Configuration.GetSection("appSettings").Get<TransferSettings>();
    }

    public IConfiguration Configuration { get; }

    private TransferSettings _transferSettings { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews().AddNewtonsoftJson();
        services.AddMvc();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Transfer API", Version = "v1" });
            c.OperationFilter<SwaggerFileOperationFilter>();
        });

        //параметры приложения
        //var settings = Configuration.GetSection("appSettings").Get<TransferSettings>();
        services.Configure<TransferSettings>(Configuration.GetSection("appSettings"));


        //подключение к БД
        services.AddDbContext<TransferContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("TransferDb")));

        services.AddTransient<IUnitOfWork, Dal.UnitOfWork>();

        services.AddTransient<ICacheService, InMemoryCache>();

        services.AddHttpContextAccessor();
        services.AddTransient<ISecurityService, SecurityService>();

        services.TransferBlConfigue();

        //автлоризация через Cookie (Claims)
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, x =>
            {
                x.Cookie.HttpOnly = false;
                x.LoginPath = "/";
                x.SlidingExpiration = true;
                x.ExpireTimeSpan = TimeSpan.FromMinutes(15);
            });

        #if DEBUG

        services.AddHostedService<ConfigureWebhook>();

        services.AddHttpClient("tgwebhook")
        .AddTypedClient<ITelegramBotClient>(httpClient
            => new TelegramBotClient(_transferSettings.TGBotToken, httpClient));

        services.AddScoped<HandleUpdateService>();

        #endif
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSwagger();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");


            endpoints.MapSwagger();

                //todo возможно придётся включить при тестировании авторизации
                //endpoints.MapControllers();
                //endpoints.MapRazorPages();
            });

        if (env.IsDevelopment())
        {
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Transfer API V1");
            });
        }


        var cookiePolicyOptions = new CookiePolicyOptions
        {
            CheckConsentNeeded = context => false,
            //MinimumSameSitePolicy = SameSiteMode.None,
            //MinimumSameSitePolicy = SameSiteMode.Strict,
        };

        app.UseCookiePolicy(cookiePolicyOptions);

        Security.Configure(app.ApplicationServices.GetRequiredService<ISecurityService>(),
            app.ApplicationServices.GetRequiredService<ICacheService>(),
            app.ApplicationServices.GetRequiredService<IConfiguration>());
    }
}