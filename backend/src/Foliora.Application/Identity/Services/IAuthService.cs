using Foliora.Application.Identity.DTOs;

namespace Foliora.Application.Identity.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    Task<AuthResponse> GoogleSsoAsync(GoogleSsoRequest request, CancellationToken cancellationToken);
}
