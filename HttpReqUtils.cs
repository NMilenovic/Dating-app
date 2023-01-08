using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace Dating_app
{
    public class HttpReqUtils
    {
        public static string GetUserId(HttpRequest requst)
        {
            if(requst.Headers.TryGetValue("Authorization",out var authHeader))
            {
                var encoded = authHeader.ToString().Replace("Bearer ",string.Empty);

                var encodedParts = encoded.Split('.');
                var payload = JwtPayload.Base64UrlDeserialize(encodedParts[1]);

                return payload.Sub;
            }
            return null;
        }
    }
}