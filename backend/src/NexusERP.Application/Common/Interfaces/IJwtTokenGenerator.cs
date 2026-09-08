using NexusERP.Application.Common.Models;
using NexusERP.Domain.Identity.Enums;

namespace NexusERP.Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    JwtTokenResult GenerateToken(
        Guid userId,
        string email,
        UserRole role);
}