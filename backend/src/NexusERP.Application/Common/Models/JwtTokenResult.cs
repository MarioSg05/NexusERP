namespace NexusERP.Application.Common.Models;

public sealed record JwtTokenResult(
    string AccessToken,
    DateTime ExpiresAt);