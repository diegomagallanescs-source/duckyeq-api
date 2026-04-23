using DuckyEQ.Domain.Enums;

namespace DuckyEQ.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string Username { get; private set; } = null!;      // immutable, PascalCase auto-generated
    public string KnownAs { get; private set; } = null!;        // max 10 chars, mutable
    public DuckCharacter DuckCharacter { get; private set; }
    public int OverallXP { get; private set; }
    public int OverallLevel { get; private set; }
    public int StreakDays { get; private set; }
    public DateTime? LastActiveDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool EmailVerified { get; private set; }

    private User() { }

    public static User Create(string email, string passwordHash, string username, string knownAs, DuckCharacter duckCharacter)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            Username = username,
            KnownAs = knownAs,
            DuckCharacter = duckCharacter,
            OverallXP = 0,
            OverallLevel = 1,
            StreakDays = 0,
            CreatedAt = DateTime.UtcNow,
            EmailVerified = false
        };
    }

    public void UpdateKnownAs(string knownAs) => KnownAs = knownAs;
    public void VerifyEmail() => EmailVerified = true;
}