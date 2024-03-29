using Core.Domain.Authentication;
using Core.Repository.Entities;
using Core.Repository.Interfaces;
using Core.Utilities;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Features.Authentication.Queries
{
    public class GetTokenValidationQueryHandler : IRequestHandler<GetTokenValidationQuery, AuthUser>
    {
        private readonly UserManager<AuthUserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ICache _cache;

        public GetTokenValidationQueryHandler(UserManager<AuthUserEntity> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IMapper mapper, ICache cache)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<AuthUser> Handle(GetTokenValidationQuery request, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]);
            tokenHandler.ValidateToken(request.Token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userName = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return null;
            }
            else
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                if (jwtToken.ValidTo < DateTime.Now.AddHours(AuthUtils.EXPIRATION_TIME - 2))
                {
                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var token = AuthUtils.GetToken(authClaims, _configuration);


                    var authUser = new AuthUser()
                    {
                        TokenInfo = new TokenInfo()
                        {
                            Token = new JwtSecurityTokenHandler().WriteToken(token),
                            Expiration = token.ValidTo,
                        },
                        User = new User()
                        {
                            UserId = user.Id,
                            UserName = user.UserName,
                            DisplayName = user.DisplayName,
                            Role = userRoles.FirstOrDefault()
                        }
                    };

                    await _cache.SetAsync("Validate:" + authUser.User.UserId, authUser, TimeSpan.FromHours(AuthUtils.EXPIRATION_TIME));
                    return authUser;
                }
                else
                {
                    if (_cache.Exists("Validate:" + user.Id))
                    {
                        return await _cache.GetAsync<AuthUser>("Validate:" + user.Id);
                    }

                    var authUser = new AuthUser()
                    {
                        TokenInfo = new TokenInfo()
                        {
                            Token = request.Token,
                            Expiration = jwtToken.ValidTo,
                        },
                        User = new User()
                        {
                            UserId = user.Id,
                            UserName = user.UserName,
                            DisplayName = user.DisplayName,
                            Role = userRoles.FirstOrDefault()
                        }
                    };

                    await _cache.SetAsync($"Validate:{authUser.User.UserId}", authUser,
                       TimeSpan.FromHours(AuthUtils.EXPIRATION_TIME - (authUser.TokenInfo.Expiration.Hour - DateTime.Now.Hour)));

                    return authUser;

                }
            }
        }
    }
}