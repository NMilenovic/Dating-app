using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
namespace Dating_app{
    public static class JwtHelper
    {
        private static readonly SymmetricSecurityKey SimetricniKljuc;

        static JwtHelper()
        {
            var secret = Config.GetJwtConfig();

            var secretBytes = Encoding.UTF8.GetBytes(secret);
            SimetricniKljuc = new SymmetricSecurityKey(secretBytes);
        }
        public static void ConfigureJwt(JwtBearerOptions opts)
        {
            opts.RequireHttpsMetadata = false;
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = SimetricniKljuc,
                ValidateIssuerSigningKey = true,
                RequireSignedTokens = true,
                ValidateLifetime = false
            };
        }
         public static string CreateToken(Dictionary<string, object> dict)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateJwtSecurityToken(new SecurityTokenDescriptor
            {
                Claims = dict,
                SigningCredentials = new SigningCredentials(SimetricniKljuc, "HS256")
            });
            return tokenHandler.WriteToken(token);
        }
    }
    
}