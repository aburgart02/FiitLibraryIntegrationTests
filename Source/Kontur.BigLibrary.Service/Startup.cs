using System;
using Autofac;
using FluentValidation;
using FluentValidation.AspNetCore;
using Kontur.BigLibrary.DataAccess;
using Kontur.BigLibrary.DataAccess.Context;
using Kontur.BigLibrary.Service.Configuration;
using Kontur.BigLibrary.Service.Contracts;
using Kontur.BigLibrary.Service.Extensions;
using Kontur.BigLibrary.Service.Helpers;
using Kontur.BigLibrary.Service.Integration;
using Kontur.BigLibrary.Service.Middleware;
using Kontur.BigLibrary.Service.Services.BookService;
using Kontur.BigLibrary.Service.Services.BookService.Repository;
using Kontur.BigLibrary.Service.Services.EventService;
using Kontur.BigLibrary.Service.Services.EventService.Repository;
using Kontur.BigLibrary.Service.Services.ImageService;
using Kontur.BigLibrary.Service.Services.ImageService.Repository;
using Kontur.BigLibrary.Service.Services.LibrarianService;
using Kontur.BigLibrary.Service.Services.RubricsService;
using Kontur.BigLibrary.Service.Services.SynonimMaker;
using Kontur.BigLibrary.Service.Validations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Vostok.Logging.Abstractions;
using ConsoleLog = Vostok.Logging.Console.ConsoleLog;
using ILog = Vostok.Logging.Abstractions.ILog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Kontur.BigLibrary.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Kontur.BigLibrary.Service.Services.AuthService;

namespace Kontur.BigLibrary.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDataProtection()
                .SetDefaultKeyLifetime(TimeSpan.FromDays(36500))
                .SetApplicationName("biglibrary");

            services.AddScoped<IAuthService, AuthService>();

            services.AddDbContext<BigLibraryContext>(options =>
                options.UseSqlite(Configuration["ConnectionString"]));

            services
                .AddDefaultIdentity<IdentityUser>(configureOptions: options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<BigLibraryContext>()
                .AddDefaultTokenProviders();

            services.AddControllersWithViews().AddFluentValidation();
            services.AddFluentValidationBehavior();
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });
            services.AddHostedService<BookSyncService>();
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            // Настройка аутентификации JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = new JwtSettings();
                Configuration.GetSection("JwtSettings").Bind(jwtSettings);

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))

                };
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UsePathBase("/");
            app.UseForwardedHeaders(GetForwardedHeadersOptions());
            app.UseCustomExceptionMiddleware();
            app.UseMiddleware<CheckAuthenticationMiddleware>();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseRouting();
            // порядок UseAuthentication и UseAuthorization важен, добавлены из-за использования JWT
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSpa(ConfigureSpa);

            void ConfigureSpa(ISpaBuilder spa)
            {
                spa.Options.SourcePath = "ClientApp";
                spa.Options.DefaultPageStaticFileOptions = new StaticFileOptions { OnPrepareResponse = DisableCacheIndexPage };

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:8080");
                }

                void DisableCacheIndexPage(StaticFileResponseContext responseContext)
                {
                    if (responseContext.File.Name != Constants.IndexPage)
                        return;

                    var headers = responseContext.Context.Response.GetTypedHeaders();
                    var cacheControlHeaderValue = new CacheControlHeaderValue { NoStore = true, NoCache = true };

                    headers.CacheControl = cacheControlHeaderValue;
                }
            }

            ConfigureDatabase();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            var logger = GetLogger();

            builder.RegisterInstance(logger).As<ILog>().SingleInstance();

            builder.RegisterType<BookRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<SynonymMaker>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<BookService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<LibrarianService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<RubricsService>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ImageRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ImageService>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<EventRepository>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<EventService>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<ImageTransformer>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<BookValidator>().As<IValidator<Book>>().SingleInstance();
            builder.RegisterType<RubricValidator>().As<IValidator<Rubric>>().SingleInstance();
            builder.RegisterType<ImageValidator>().As<IValidator<FormImageFile>>().SingleInstance();
            
            builder.Register(x => new DbConnectionFactory(Configuration["ConnectionString"]))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<HttpContextAccessor>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<CheckAuthenticationMiddleware>().SingleInstance();
            builder.RegisterType<AuthenticationHelpers>().AsImplementedInterfaces().SingleInstance();
        }

        private static ILog GetLogger()
        {
            return new ConsoleLog()
                .WithMinimumLevel(
#if DEBUG
                    LogLevel.Debug
#else
                    LogLevel.Info
#endif
                );

        }

        private void ConfigureDatabase()
        {
            DataAccessConfiguration.Configure();
        }

        private static ForwardedHeadersOptions GetForwardedHeadersOptions()
        {
            var options = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                ForwardedProtoHeaderName = Constants.XSchemeHeader
            };

            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();

            return options;
        }
    }
}
