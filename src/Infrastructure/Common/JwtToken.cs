using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Infrastructure.Common
{
    public class JwtToken
    {
        public string Token { get; set; }
        public int ExpireSeconds { get; set; }
        public JwtToken(JwtSecurityToken jwtSecurityToken)
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            ExpireSeconds = (int)(jwtSecurityToken.ValidTo - DateTime.UtcNow).TotalSeconds;
        }
    }
}
