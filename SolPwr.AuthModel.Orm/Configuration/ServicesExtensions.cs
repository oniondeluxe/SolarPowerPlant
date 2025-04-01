using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnionDlx.SolPwr.Persistence;
using OnionDlx.SolPwr.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Configuration
{
    public static class ServicesAuthExtensions
    {
        /// <summary>
        /// Will add the needed boilerplate to IoC, without the need to introduce a dependency to identity in the main app
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthServices(this IServiceCollection coll, ConfigurationManager config, string connString)
        {
            coll.AddDbContext<AuthIdentityContext>(options => options.UseSqlServer(connString));
            coll.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Cheaping out on the policies here ;)
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
            })
                .AddEntityFrameworkStores<AuthIdentityContext>()
                .AddDefaultTokenProviders();

            coll.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    var key = config["AuthenticationSettings:Key"];
                    var issuer = config["AuthenticationSettings:Issuer"];
                    var audience = config["AuthenticationSettings:Audience"];
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        RequireExpirationTime = false,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
                    };
                });

            coll.AddScoped<IUserAuthService, UserAuthService>();

            return coll;
        }
    }
}
