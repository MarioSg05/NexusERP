using NexusERP.Application.Common.Models;

namespace NexusERP.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    JwtTokenResult GenerateToken(
        Guid userId,
        string email);
}