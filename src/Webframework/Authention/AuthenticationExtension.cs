using Common;
using Common.Resources;
using Common.Utilities;
using Infrastructure.Identity.Models;
using Infrastructure.Repositories.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Webframework.Authention
{
    public static class AuthenticationExtension
    {
        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            var jwtSettings = new JwtSettings();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var secretkey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
                var encryptionkey = Encoding.UTF8.GetBytes(jwtSettings.Encryptkey);

                var validationParameters = new TokenValidationParameters
                {
                    ClockSkew = TimeSpan.Zero, // default: 5 min
                    RequireSignedTokens = true,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretkey),

                    RequireExpirationTime = true,
                    ValidateLifetime = true,

                    ValidateAudience = true, //default : false
                    ValidAudience = jwtSettings.Audience,

                    ValidateIssuer = true, //default : false
                    ValidIssuer = jwtSettings.Issuer,

                    TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
                };

                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = validationParameters;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("Authentication failed.", context.Exception);

                        if (context.Exception != null)
                            throw new AuthenticationException(AccountResources.authentication_failed);

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = async context =>
                    {
                        var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();
                        var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();

                        var claimsIdentity = context.Principal.Identity as ClaimsIdentity;
                        if (claimsIdentity.Claims?.Any() != true)
                            context.Fail(AccountResources.token_has_no_claims);
                        var securityStamp = claimsIdentity.FindFirst(new ClaimsIdentityOptions().SecurityStampClaimType)?.Value;
                        if (!securityStamp.HasValue())
                            context.Fail(AccountResources.token_has_no_secuirty_stamp);

                        //Find user and token from database and perform your custom validation
                        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                        var user = await userRepository.GetAsync(userId, context.HttpContext.RequestAborted);

                        //if (user.SecurityStamp != Guid.Parse(securityStamp))
                        //    context.Fail("Token secuirty stamp is not valid.");

                        var validatedUser = await signInManager.ValidateSecurityStampAsync(context.Principal);
                        if (validatedUser == null)
                            context.Fail("Token secuirty stamp is not valid.");

                        if (!user.IsActive)
                            context.Fail("User is not active.");

                        await userRepository.UpdateLastLoginAsync(user, context.HttpContext.RequestAborted);
                    },
                    OnChallenge = context =>
                    {
                        //var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(nameof(JwtBearerEvents));
                        //logger.LogError("OnChallenge error", context.Error, context.ErrorDescription);

                        if (context.AuthenticateFailure != null)
                            throw new AuthenticationException(AccountResources.authenticate_failure);
                        throw new UnauthorizedAccessException(AccountResources.unauthorized_access_resource);

                        //return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
