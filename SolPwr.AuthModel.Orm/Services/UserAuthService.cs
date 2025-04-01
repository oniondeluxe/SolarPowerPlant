using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OnionDlx.SolPwr.Configuration;
using OnionDlx.SolPwr.Dto;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OnionDlx.SolPwr.Services
{
    internal class UserAuthService : IUserAuthService
    {
        readonly IConfiguration _config;
        readonly UserManager<IdentityUser> _userMgr;
        readonly ResponseFactory<UserAuthService> _responses;

        public async Task<UserAuthResponse> RegisterUserAsync(UserAccountRegistration registration)
        {
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(UserAccountRegistration));
            }

            if (registration.Password != registration.ConfirmPassword)
            {
                return _responses.CreateFaulted(Messages.ERR_PWD_MISMATCH);
            }

            var userRecord = new IdentityUser
            {
                UserName = registration.Email,
                Email = registration.Email,
            };

            var result = await _userMgr.CreateAsync(userRecord, registration.Password);
            if (!result.Succeeded)
            {
                return _responses.CreateFaulted(result.Errors.FirstOrDefault()?.Description)
                                 .WithIdentityErrors(result.Errors);
            }

            return _responses.CreateSuccess(Messages.MSG_ACCT_CREATED);
        }


        public async Task<UserAuthResponse> SignonUserAsync(UserSignonRecord signon)
        {
            var user = await _userMgr.FindByEmailAsync(signon.Email);
            if (user == null)
            {
                return _responses.CreateFaulted(Messages.ERR_SIGNON_FAIL);
            }

            var pwResult = await _userMgr.CheckPasswordAsync(user, signon.Password);
            if (!pwResult)
            {
                return _responses.CreateFaulted(Messages.ERR_SIGNON_FAIL);
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthenticationSettings:Key"]));
            var token = new JwtSecurityToken(
                issuer: _config["AuthenticationSettings:Issuer"],
                audience: _config["AuthenticationSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return _responses.CreateSuccess(tokenString);
        }


        public UserAuthService(UserManager<IdentityUser> userMgr, IConfiguration configuration, ILogger<UserAuthService> logger)
        {
            _userMgr = userMgr;
            _config = configuration;
            // All responses will have logging guaranteed
            _responses = new ResponseFactory<UserAuthService>(logger);
        }
    }
}
