using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_IdentitySecurity_JWT.Utilities
{
    public class Tokens
    {
        public string GenerateTokens(List<Claim> claims, DateTime expiresAt, string secretKey)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(secretKey);
                var jwt = new JwtSecurityToken(
                        claims: claims,
                        notBefore: DateTime.Now,
                        expires: expiresAt,
                        signingCredentials: new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature
                            )
                    );
                return new JwtSecurityTokenHandler().WriteToken(jwt);

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
