using System;
using System.Net.Http;
using System.Text;
using Dotes.BL.Templates;
using Dotes.DAL.Templates;
using Dotes.DAL.TemplateTypes;
using Dotes.Web.Auth;
using Ext.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Templates.Web.Auth;
using Templates.Web.Models;

namespace Templates.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private const string SecretKey = "qGbNivDRj1iqsfhPVkH23sMRmHLpUA2d";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.Converters.Add(new StringEnumConverter()));

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dotes", Version = "v1" });
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddSingleton<ITemplatesBL, TemplatesBL>();
            services.AddSingleton<ITemplateRepository, SQLiteTemplateRepository>();
            services.AddSingleton<ITemplateTypeRepository, SQLiteTemplateTypeRepository>();
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.AddSingleton<HttpClient>();

            services.AddMvc();

            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dotes v1"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = exceptionHandlerPathFeature.Error;
                Console.WriteLine(exception.Message);
            }));

            app.UseAuthentication();
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();

            app.Use(async (context, next) =>
            {
                var dataProtectionProvider = app.ApplicationServices.GetService<IDataProtectionProvider>();
                var dataProtector = dataProtectionProvider.CreateProtector(JwtIssuerOptions.DataProtectorPurpose);
                var authSessionKey = SessionHelper.AuthSessionKey;

                if (context.Session.Get<LoginModel>(authSessionKey) == null && context.Request.Cookies.ContainsKey("login") && context.Request.Cookies.ContainsKey("password"))
                {
                    try
                    {
                        var login = dataProtector.Unprotect(context.Request.Cookies["login"]);
                        var password = dataProtector.Unprotect(context.Request.Cookies["password"]);

                        if (login == Configuration["AuthInfo:Login"] && password == Configuration["AuthInfo:Password"])
                        {
                            var model = new LoginModel(login, password);
                            context.Session.Set(authSessionKey, model);
                        }
                    }
                    catch (Exception e)
                    {
                        context.Response.Cookies.Delete("login");
                        context.Response.Cookies.Delete("password");
                        context.Session.Remove(authSessionKey);
                    }
                }
                await next.Invoke();
            });

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                spa.Options.StartupTimeout = TimeSpan.FromSeconds(120);

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
