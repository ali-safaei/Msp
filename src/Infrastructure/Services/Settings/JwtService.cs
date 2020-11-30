using Common;
using Common.Utilities;
using Infrastructure.Common;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Settings
{
    public class JwtService : IJwtService
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> _userManager;

        public JwtService(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this._userManager = userManager;
        }

        public async Task<JwtToken> CreateAsync(User user)
        {
            var jwtSetting = new JwtSettings(); 
            var secretKey = Encoding.UTF8.GetBytes(jwtSetting.SecretKey); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(jwtSetting.Encryptkey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var claims = await AddClaimsAsync(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtSetting.Issuer,
                Audience = jwtSetting.Audience,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(jwtSetting.NotBeforeMinutes),
                Expires = DateTime.Now.AddMinutes(jwtSetting.ExpirationMinutes),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
            //string encryptedJwt = tokenHandler.WriteToken(securityToken);
            return new JwtToken(securityToken);
        }

        private async Task<IEnumerable<Claim>> AddClaimsAsync(User user)
        {
            var result = await signInManager.ClaimsFactory.CreateAsync(user);
            //add custom claims
            var list = new List<Claim>(result.Claims);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.HasValue())
                foreach (var role in roles)
                    list.Add(new Claim(ClaimTypes.Role, role));

            return list;
        }
    }
}
