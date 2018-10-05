using System;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using BaseLine2018.Common.Models.Domain.Identity;
using BaseLine2018.Data.Context;
using BaseLine2018.Common.LookupData;

namespace BaseLine2018.Api.StartupExtensions
{
    public static class IdentityStartupExtension
    {
        public static void SetIdentity(IServiceCollection services)
        {

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 12;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30d);
                options.Lockout.MaxFailedAccessAttempts = 5;

                //// Cookie settings
                    //options.Cookies.ApplicationCookie.CookieSecure = IsDevelopment ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
                    //options.Cookies.ApplicationCookie.CookieHttpOnly = true;
                    //options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromMinutes(60d);
                    //options.Cookies.ApplicationCookie.SlidingExpiration = true;
                    //options.Cookies.ApplicationCookie.LoginPath = "/Account/LogIn";
                    //options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOff";

                    //options.SecurityStampValidationInterval = TimeSpan.FromMinutes(10d);

                // User settings
                    //options.SignIn.RequireConfirmedEmail = !IsDevelopment;
                options.User.RequireUniqueEmail = true;
            });


            // Define roles that are associated with a policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy(IdentityProviderPolicy.UserManagement, policy => policy.RequireRole(IdentityProviderRole.SuperUser, IdentityProviderRole.Auditor));
                options.AddPolicy(IdentityProviderPolicy.Auditing, policy => policy.RequireRole(IdentityProviderRole.Auditor));

            });


        }

    }
}
