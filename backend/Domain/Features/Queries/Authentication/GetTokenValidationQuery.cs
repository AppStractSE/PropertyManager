using Domain.Domain.Authentication;
using MediatR;

namespace Domain.Features.Authentication.Queries
{
    public class GetTokenValidationQuery : IRequest<AuthUser>
    {
        public string Token { get; set; } = "";
    }
}
