namespace Foliora.Application.Identity.DTOs;

public record RegisterRequest(string Email, string Password);
public record LoginRequest(string Email, string Password);
public record GoogleSsoRequest(string Email, string GoogleSubjectId);
public record AuthResponse(Guid UserId, string Email, string Token);
