namespace DuckyEQ.Domain.Entities;

public class QuackCoins
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public int Balance { get; private set; }
    public int TotalEarned { get; private set; }
    public DateTime? LastEarnedAt { get; private set; }

    private QuackCoins() { }

    public static QuackCoins Create(Guid userId)
    {
        return new QuackCoins
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Balance = 0,
            TotalEarned = 0
        };
    }

    public void Award(int amount)
    {
        Balance += amount;
        TotalEarned += amount;
        LastEarnedAt = DateTime.UtcNow;
    }

    public bool TryDeduct(int amount)
    {
        if (Balance < amount) return false;
        Balance -= amount;
        return true;
    }
}
