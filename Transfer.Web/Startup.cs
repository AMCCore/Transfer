using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using Telegram.Bot;
using Transfer.Bl;
using Transfer.Common;
using Transfer.Common.Cache;
using Transfer.Common.Security;
using Transfer.Common.Settings;
using Transfer.Dal.Context;
using Transfer.Web.Models;
using Transfer.Web.Moduls;
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
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        //параметры приложения
        //var settings = Configuration.GetSection("appSettings").Get<TransferSettings>();
        services.Configure<TransferSettings>(Configuration.GetSection("appSettings"));

        //параметры почты
        services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));

        //подключение к БД
        services.AddDbContext<TransferContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("TransferDb")));

        services.AddTransient<IUnitOfWork, Dal.UnitOfWork>();

        services.AddTransient<ICacheService, InMemoryCache>();

        services.AddHttpContextAccessor();
        services.AddTransient<IAdvancedSecurityService, SecurityService>();
        services.AddTransient<ITripRequestSecurityService, TripRequestSecurityService>();
        services.AddTransient<ITokenValidator, TokenValidator>();
        services.AddTransient<ISecurityService, SecurityService>();
        services.AddTransient<ITokenService, TokenService>();

        services.AddTransient<IMailModule, MailModule>();

        services.TransferBlConfigue();

        //автлоризация через Cookie (Claims)
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, x =>
            {
                x.Cookie.HttpOnly = false;
                x.LoginPath = "/";
                x.SlidingExpiration = true;
                x.ExpireTimeSpan = TimeSpan.FromHours(12);
            });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "MyAuthClient",
                    ValidAudience = "MyAuthClient",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenValidator.SecKey))
                };
            });

#if !DEBUG

        services.AddHostedService<ConfigureWebhook>();

        services.AddHttpClient("tgwebhook")
        .AddTypedClient<ITelegramBotClient>(httpClient
            => new TelegramBotClient(_transferSettings.TGBotToken, httpClient));

        services.AddScoped<HandleUpdateService>();

#endif

        services.AddControllers().AddNewtonsoftJson();
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

        Security.Configure(app.ApplicationServices.GetRequiredService<IAdvancedSecurityService>(),
            app.ApplicationServices.GetRequiredService<ICacheService>(),
            app.ApplicationServices.GetRequiredService<IConfiguration>());
    }
}