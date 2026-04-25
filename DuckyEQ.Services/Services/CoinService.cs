using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Contracts.Interfaces.Services;

namespace DuckyEQ.Services.Services;

public class CoinService : ICoinService
{
    private readonly ICoinRepository _coinRepo;

    public CoinService(ICoinRepository coinRepo)
    {
        _coinRepo = coinRepo;
    }

    public async Task<int> GetBalanceAsync(Guid userId)
    {
        var coins = await _coinRepo.GetByUserAsync(userId);
        return coins?.Balance ?? 0;
    }

    public async Task AwardAsync(Guid userId, int amount)
    {
        await _coinRepo.EnsureExistsAsync(userId);
        await _coinRepo.AwardAsync(userId, amount);
    }

    public Task<bool> TryDeductAsync(Guid userId, int amount) =>
        _coinRepo.DeductAsync(userId, amount);
}
