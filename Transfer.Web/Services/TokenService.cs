using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Transfer.Common.Security;
using Transfer.Web.Moduls;

namespace Transfer.Web.Services;

public class TokenService : TokenValidator, ITokenService, ITokenValidator
{
    public string BuildToken(Guid userId)
    {
        var claims = new[] {
                //new Claim(ClaimTypes.Name, userId.ToString()),
                //new Claim(ClaimTypes.GivenName, user.UserName),
                //new Claim(ClaimTypes.Role, JsonConvert.SerializeObject(user.Rights)),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenValidator.SecKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken("MyAuthClient", "MyAuthClient", claims,
            expires: DateTime.Now.AddDays(90), signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
