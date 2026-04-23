namespace DuckyEQ.Domain.Entities;

public class QuackCoins
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public int Balance { get; private set; }

    private QuackCoins() { }
}