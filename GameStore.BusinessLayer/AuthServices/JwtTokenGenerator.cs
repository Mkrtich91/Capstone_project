namespace GameStore.BusinessLayer.AuthServices
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using GameStore.BusinessLayer.Interfaces.Configuration;
    using GameStore.BusinessLayer.Interfaces.IAuthServices;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IPermissionService _permissionService;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions, IPermissionService permissionService)
        {
            _jwtOptions = jwtOptions.Value;
            _permissionService = permissionService;
        }

        public async Task<string> GenerateTokenAsync(
            IdentityUser applicationUser,
            IEnumerable<string> roles,
            IEnumerable<Claim> additionalClaims = null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var claimList = new List<Claim>
        {
            new Claim("userId", applicationUser.Id),
            new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
            new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, applicationUser.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var permissions = await _permissionService.GetPermissionsByRolesAsync(roles);
            claimList.AddRange(permissions.Select(permission => new Claim("Permission", permission)));

            if (additionalClaims != null)
            {
                claimList.AddRange(additionalClaims);
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer,
                Subject = new ClaimsIdentity(claimList),
                Expires = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
