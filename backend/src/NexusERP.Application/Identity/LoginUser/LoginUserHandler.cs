using Microsoft.EntityFrameworkCore;

using NexusERP.Application.Common.Exceptions;
using NexusERP.Application.Common.Interfaces;
using NexusERP.Domain.Identity.ValueObjects;

namespace NexusERP.Application.Identity.LoginUser;

public sealed class LoginUserHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public LoginUserHandler(
        IApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginUserResponse> Handle(
        LoginUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var email =
            new Email(request.Email);

        var user =
            await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Email == email,
                    cancellationToken);

        if (user is null)
        {
            throw new UnauthorizedException(
                "Invalid credentials.");
        }

        var passwordIsValid =
            _passwordHasher.Verify(
                request.Password,
                user.PasswordHash.Value);

        if (!passwordIsValid)
        {
            throw new UnauthorizedException(
                "Invalid credentials.");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedException(
                "User account is inactive.");
        }

        var token =
    _jwtTokenGenerator.GenerateToken(
        user.Id,
        user.Email.Value,
        user.Role);

        return new LoginUserResponse(
            user.Id,
            user.Email.Value,
            token.AccessToken,
            token.ExpiresAt);
    }
}