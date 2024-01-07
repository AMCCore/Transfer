using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Transfer.Common.Security;

namespace Transfer.Web.API.Models;

public class TokenValidator : ITokenValidator
{
    public const string SecKey = "@UjS6niZbkkcCmzRFzMRWEnFP6oazuCspv9&f2^fWRqfQwG$xb";

    public bool IsTokenValid(string token)
    {
        var mySecret = Encoding.UTF8.GetBytes(SecKey);
        var mySecurityKey = new SymmetricSecurityKey(mySecret);

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = "MyAuthClient",
                ValidAudience = "MyAuthClient",
                IssuerSigningKey = mySecurityKey,
            }, out SecurityToken validatedToken);
        }
        catch
        {
            return false;
        }
        return true;
    }
}
