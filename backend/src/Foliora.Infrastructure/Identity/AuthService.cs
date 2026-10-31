using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Foliora.Domain.Entities;
using Foliora.Infrastructure.Persistence;
using Foliora.Application.Identity.DTOs;
using Foliora.Application.Identity.Services;

namespace Foliora.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly FolioraDbContext _dbContext;
    private readonly PasswordHasher<User> _passwordHasher = new();
    private readonly IConfiguration _configuration;

    public AuthService(FolioraDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        var user = new User
        {
            Email = request.Email
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var token = GenerateJwtToken(user);
        return new AuthResponse(user.Id, user.Email, token);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
        if (user == null || string.IsNullOrEmpty(user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var token = GenerateJwtToken(user);
        return new AuthResponse(user.Id, user.Email, token);
    }

    public async Task<AuthResponse> GoogleSsoAsync(GoogleSsoRequest request, CancellationToken cancellationToken)
    {
        var externalIdentity = await _dbContext.ExternalIdentities
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Provider == "google" && e.SubjectId == request.GoogleSubjectId, cancellationToken);

        User user;
        if (externalIdentity != null)
        {
            user = externalIdentity.User;
        }
        else
        {
            user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (user == null)
            {
                user = new User
                {
                    Email = request.Email
                };
                _dbContext.Users.Add(user);
                await _dbContext.SaveChangesAsync(cancellationToken);

            }

            var extId = new ExternalIdentity
            {
                UserId = user.Id,
                Provider = "google",
                SubjectId = request.GoogleSubjectId
            };
            _dbContext.ExternalIdentities.Add(extId);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var token = GenerateJwtToken(user);
        return new AuthResponse(user.Id, user.Email, token);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["Secret"] ?? "FolioraSuperSecretKeyDefault1234567890!@#";
        var key = Encoding.ASCII.GetBytes(secretKey);

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
