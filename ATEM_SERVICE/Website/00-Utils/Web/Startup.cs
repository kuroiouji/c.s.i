using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.AspNetCore.Localization;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Web.Config
{
    public class Startup
    {
        private const string DB_CONFIG = "DBConnection";
        private class AssemblyModule
        {
            public string FileName { get; set; }
            public string MethodName { get; set; }
            public string AssemblyType { get; set; }
        }

        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            string connectionString = configuration.GetConnectionString(DB_CONFIG);

            #region Initial Constants from config.

            Type[] constants = new Type[]
            {
                typeof(Utils.Constants),
                typeof(Web.Constants)
            };
            foreach(Type constant in constants)
            {
                foreach (PropertyInfo prop in constant.GetProperties())
                {
                    string key = string.Format("Constants:{0}", prop.Name);
                    if (configuration[key] != null)
                    {
                        if (prop.PropertyType == typeof(int))
                        {
                            int i;
                            if (int.TryParse(configuration[key], out i))
                                prop.SetValue(null, i, null);
                        }
                        else
                            prop.SetValue(null, configuration[key], null);
                    }
                }
            }

            #endregion

            List<AssemblyModule> asmModules = new List<AssemblyModule>();
            configuration.GetSection("Modules").Bind(asmModules);

            #region Add DbContext

            Web.Services.ApplicationDbContext.InitialService(services, connectionString);

            foreach (AssemblyModule module in asmModules)
            {
                if (module.AssemblyType != "Service")
                    continue;

                System.Reflection.Assembly csvc = Utils.IOUtil.GetAssembly(module.FileName);
                if (csvc != null)
                {
                    Type csvct = csvc.GetType(module.MethodName);
                    System.Reflection.MethodInfo csvcmi = csvct.GetMethod("InitialService");
                    if (csvcmi != null)
                    {
                        csvcmi.Invoke(null, new object[]
                        {
                            services,
                            connectionString
                        });
                    }
                }
            }

            #endregion

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();

                    });
            });
            
            #region Add Identity

            IdentityBuilder identity = services.AddIdentity<Web.Models.User.ApplicationUser, Web.Models.User.ApplicationRole>(options =>
            {
                // Password settings
                options.Password.RequiredLength = 0;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Web.Constants.LOGIN_WAITING_TIME);
                options.Lockout.MaxFailedAccessAttempts = Web.Constants.MAXIMUM_LOGIN_FAIL;

                // User settings
                options.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<Web.Services.ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddUserManager<Web.Models.User.ApplicationUserManager>()
                .AddRoleManager<Web.Models.User.ApplicationRoleManager>()
                .AddSignInManager<Web.Models.User.ApplicationSignInManager>()
                .AddPasswordValidator<Web.Validator.UserPasswordValidator<Web.Models.User.ApplicationUser>>();

            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = configuration["JwtIssuer"],
                        ValidAudience = configuration["JwtIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"])),

                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            #endregion

            services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            #region Add MVC

            IMvcBuilder mvcb = services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                    options.SerializerSettings.Converters.Add(new Web.Converter.JsonDateTimeConverter());
                });

            foreach (AssemblyModule module in asmModules)
            {
                if (module.AssemblyType != "Api")
                    continue;

                System.Reflection.Assembly api = Utils.IOUtil.GetAssembly(module.FileName);
                if (api != null)
                    mvcb.AddApplicationPart(api);
            }
            
            #endregion

            services.Configure<MvcOptions>(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory("AllowAllOrigins"));
            });

            //Initial Process Service.
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, Services.ProcessTaskService>();
        }

        public static void Configure(IConfiguration configuration, IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, 
            Web.Services.ApplicationDbContext dbContext)
        {
            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // ===== Use Authentication ======
            app.UseAuthentication();

            app.UseCors("AllowAllOrigins");

            app.UseMvc();

            dbContext.Database.EnsureCreated();

            Web.Constants.SCREEN_LIST = dbContext.GetScreenList();

            #region Initial Constants from config.

            List<Models.SystemConfigDo> configList = dbContext.GetSystemConfig();
            foreach (PropertyInfo prop in typeof(Web.Constants).GetProperties())
            {
                Models.SystemConfigDo config = configList.Find((x) => x.SystemCode == prop.Name);
                if (config != null)
                {
                    if (prop.PropertyType == typeof(int))
                    {
                        int i;
                        if (int.TryParse(config.SystemValue1, out i))
                            prop.SetValue(null, i, null);
                    }
                    else
                        prop.SetValue(null, config.SystemValue1, null);
                }
            }

            #endregion
        }
    }
}
