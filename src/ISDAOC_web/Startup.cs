using Domain.Entities;
using Domain.Messaging;
using Infrastructure.Data;
using Infrastructure.Messaging;
using ISDAOC_Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Presentation.Services;
using System;
using System.IO;

namespace ISDAOC_web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new SqlConnectionStringBuilder(
          Configuration.GetConnectionString("DefaultConnection"));

            if (Configuration["DbPassword"] != null)
            {
                builder.Password = Configuration["DbPassword"];
            }

            services.AddDbContext<StructureDBContext>(options =>
                options.UseSqlServer(builder.ConnectionString, sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                }));
            services.AddTransient<IStructureService, StructureService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IDataModelService, DataModelService>();
            services.AddTransient<IFeatureService, FeatureService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IWorkFlowService, WorkFlowService>();

            services.AddTransient<IDataDefinitionService, DataDefinitionService>();
            services.AddTransient<IGeneralUtilityService, GeneralUtilityService>();
            services.AddScoped<ViewRenderService, ViewRenderService>();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
            //    services.AddDefaultIdentity<ApplicationUser>()
            //.AddEntityFrameworkStores<ApplicationDbContext>()
            //.AddDefaultUI();

            services.AddDefaultIdentity<DCAppUser>(options =>
       options.SignIn.RequireConfirmedAccount = true)
           .AddEntityFrameworkStores<StructureDBContext>();
            services.AddControllersWithViews();
            services.AddRazorPages(options => {
                options.Conventions.AuthorizeFolder("/");
            }).AddRazorRuntimeCompilation();

            if (Configuration["Authentication:Google:client_id"] != null)
            {
                services.AddAuthentication()
                    .AddGoogle(options =>
                    {
                        options.ClientId = Configuration["Authentication:Google:client_id"];
                        options.ClientSecret = Configuration["Authentication:Google:client_secret"];
                    });
            }
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
            
            services.AddMvc()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver =
                       new DefaultContractResolver();
                    
                })
                .AddRazorRuntimeCompilation()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization();
            // the following option restores the UPPER case behaviour while serializing th json
            //      .AddNewtonsoftJson(options =>
            //options.SerializerSettings.ContractResolver =
            //   new CamelCasePropertyNamesContractResolver());
            //.AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()); ;

            var emailSenderUserName = Configuration["AppSettings:EmailSenderUserName"];
            var emailSenderPassword = Configuration["AppSettings:EmailSenderPassword"];
            var emailSenderHostAddress = Configuration["AppSettings:EmailSenderHostAddress"];
            //var emailSenderPortNumber = int.Parse(Configuration["AppSettings:EmailSenderPortNumber"]);

            services.AddScoped(_ => new MessageServiceOptions
            {
                PortNumber = 23,
                HostAddress = emailSenderHostAddress,
                Password = emailSenderPassword,
                UserName = emailSenderUserName
            });

            services.AddTransient<IEmailSender, MessageService>();
            services.AddTransient<ISmsSender, MessageService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjU1NDM4QDMxMzgyZTMxMmUzMGk3TzRTR3ZhVHkxcTFzem9Oc0k0Tkl0ampVMXBvQmovRitIRWdnd3VaNW89");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(routes =>
            {
                routes.MapDefaultControllerRoute();
                routes.MapAreaControllerRoute(
                     name: "Admin",
                    areaName: "Admin",
                      pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
                routes.MapAreaControllerRoute(
                    name: "Designer",
                     areaName: "Designer",
                    "Designer/{controller=Home}/{action=Index}/{id?}");
                routes.MapAreaControllerRoute(
                    name: "User",
                    areaName: "User",
                    pattern: "User/{controller=Home}/{action=Index}/{id?}");
                routes.MapRazorPages();
            });

            if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules")))
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2", @"dist", @"ej2.min.js"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"js", @"ej2", @"ej2.min.js"));
                }

                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2", @"bootstrap.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"bootstrap.css"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2", @"material.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"material.css"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2", @"highcontrast.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"highcontrast.css"));
                    File.Copy(Path.Combine(Directory.GetCurrentDirectory(), @"node_modules", @"@syncfusion", @"ej2", @"fabric.css"), Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", @"css", @"ej2", @"fabric.css"));
                }
            }
        }
    }
}