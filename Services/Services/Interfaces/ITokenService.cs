namespace Services.Services.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}