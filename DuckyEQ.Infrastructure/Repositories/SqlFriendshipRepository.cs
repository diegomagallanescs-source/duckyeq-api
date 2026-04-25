using DuckyEQ.Contracts.DTOs;
using DuckyEQ.Contracts.Interfaces.Repositories;
using DuckyEQ.Domain.Entities;
using DuckyEQ.Domain.Enums;
using DuckyEQ.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DuckyEQ.Infrastructure.Repositories;

public class SqlFriendshipRepository : IFriendshipRepository
{
    private readonly DuckyEQDbContext _db;
    public SqlFriendshipRepository(DuckyEQDbContext db) => _db = db;

    public async Task<Friendship> CreateAsync(Friendship friendship)
    {
        _db.Friendships.Add(friendship);
        await _db.SaveChangesAsync();
        return friendship;
    }

    // LEFT JOIN approach: friendships + users + today's check-in (single batch per table)
    public async Task<IReadOnlyList<FriendWithCheckInDto>> GetFriendsWithCheckInAsync(Guid userId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var sentFriends = await _db.Friendships
            .Where(f => f.RequesterId == userId && f.Status == FriendshipStatus.Accepted)
            .Join(_db.Users, f => f.AddresseeId, u => u.Id,
                  (f, u) => new { FriendshipId = f.Id, Friend = u })
            .ToListAsync();

        var receivedFriends = await _db.Friendships
            .Where(f => f.AddresseeId == userId && f.Status == FriendshipStatus.Accepted)
            .Join(_db.Users, f => f.RequesterId, u => u.Id,
                  (f, u) => new { FriendshipId = f.Id, Friend = u })
            .ToListAsync();

        var allPairs = sentFriends.Concat(receivedFriends).ToList();
        if (allPairs.Count == 0) return [];

        var friendIds = allPairs.Select(x => x.Friend.Id).ToList();

        var checkIns = await _db.DailyCheckIns
            .Where(c => friendIds.Contains(c.UserId) && c.CheckInDate == today)
            .ToDictionaryAsync(c => c.UserId);

        // Load equipped items keyed by userId → Dictionary<ShopCategory, ShopItem>
        var rawEquipped = await _db.UserInventories
            .Where(i => friendIds.Contains(i.UserId) && i.IsEquipped)
            .Join(_db.ShopItems, i => i.ShopItemId, s => s.Id,
                  (i, s) => new EquippedEntry(i.UserId, i.Category, s))
            .ToListAsync();

        var equippedByUser = rawEquipped
            .GroupBy(e => e.UserId)
            .ToDictionary(g => g.Key,
                          g => g.ToDictionary(e => e.Category, e => e.ShopItem));

        return allPairs.Select(pair =>
        {
            checkIns.TryGetValue(pair.Friend.Id, out var checkIn);
            equippedByUser.TryGetValue(pair.Friend.Id, out var equipped);

            return new FriendWithCheckInDto(
                FriendshipId: pair.FriendshipId,
                UserId: pair.Friend.Id,
                KnownAs: pair.Friend.KnownAs,
                Username: pair.Friend.Username,
                OverallLevel: pair.Friend.OverallLevel,
                DuckCharacter: pair.Friend.DuckCharacter,
                EquippedItems: ToEquippedItems(equipped),
                HasCheckedInToday: checkIn is not null,
                EmotionIds: checkIn is not null ? [checkIn.EmotionId.ToString()] : null,
                Phrase: checkIn?.Phrase
            );
        }).ToList();
    }

    public async Task<IReadOnlyList<Friendship>> GetPendingIncomingAsync(Guid userId) =>
        await _db.Friendships
            .Where(f => f.AddresseeId == userId && f.Status == FriendshipStatus.Pending)
            .ToListAsync();

    public Task<Friendship?> GetByIdAsync(Guid id) =>
        _db.Friendships.FirstOrDefaultAsync(f => f.Id == id);

    public async Task UpdateStatusAsync(Guid friendshipId, FriendshipStatus status)
    {
        var friendship = await _db.Friendships.FirstOrDefaultAsync(f => f.Id == friendshipId);
        if (friendship is null) return;

        if (status == FriendshipStatus.Accepted) friendship.Accept();
        else if (status == FriendshipStatus.Declined) friendship.Decline();

        await _db.SaveChangesAsync();
    }

    // Bidirectional: checks (a→b) and (b→a) in one query
    public Task<Friendship?> GetBetweenUsersAsync(Guid userId1, Guid userId2) =>
        _db.Friendships.FirstOrDefaultAsync(f =>
            (f.RequesterId == userId1 && f.AddresseeId == userId2) ||
            (f.RequesterId == userId2 && f.AddresseeId == userId1));

    public async Task<FriendDetailDto?> GetFriendDetailAsync(Guid currentUserId, Guid friendUserId)
    {
        var friendship = await _db.Friendships.FirstOrDefaultAsync(f =>
            f.Status == FriendshipStatus.Accepted &&
            ((f.RequesterId == currentUserId && f.AddresseeId == friendUserId) ||
             (f.RequesterId == friendUserId && f.AddresseeId == currentUserId)));

        if (friendship is null) return null;

        var friend = await _db.Users.FirstOrDefaultAsync(u => u.Id == friendUserId);
        if (friend is null) return null;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var checkIn = await _db.DailyCheckIns
            .FirstOrDefaultAsync(c => c.UserId == friendUserId && c.CheckInDate == today);

        var rawEquipped = await _db.UserInventories
            .Where(i => i.UserId == friendUserId && i.IsEquipped)
            .Join(_db.ShopItems, i => i.ShopItemId, s => s.Id,
                  (i, s) => new EquippedEntry(friendUserId, i.Category, s))
            .ToListAsync();

        var equippedMap = rawEquipped.ToDictionary(e => e.Category, e => e.ShopItem);

        return new FriendDetailDto(
            UserId: friend.Id,
            KnownAs: friend.KnownAs,
            Username: friend.Username,
            OverallLevel: friend.OverallLevel,
            OverallXP: friend.OverallXP,
            DuckCharacter: friend.DuckCharacter,
            EquippedItems: ToEquippedItems(equippedMap),
            HasCheckedInToday: checkIn is not null,
            TodayEmotionIds: checkIn is not null ? [checkIn.EmotionId.ToString()] : null,
            TodayPhrase: checkIn?.Phrase,
            FriendshipId: friendship.Id
        );
    }

    private static EquippedItems ToEquippedItems(Dictionary<ShopCategory, ShopItem>? map)
    {
        if (map is null || map.Count == 0)
            return new EquippedItems(null, null, null, null);

        ShopItemDto? Slot(ShopCategory cat)
        {
            if (!map.TryGetValue(cat, out var s)) return null;
            return new ShopItemDto(s.Id, s.Name, s.Category, s.CoinPrice,
                s.DuckyImageUrl, s.DaisyImageUrl, true, true, s.Rarity, s.Description);
        }

        return new EquippedItems(
            Hat: Slot(ShopCategory.Hat),
            Accessory: Slot(ShopCategory.Accessory),
            Glow: Slot(ShopCategory.Glow),
            Color: Slot(ShopCategory.Color)
        );
    }

    private record EquippedEntry(Guid UserId, ShopCategory Category, ShopItem ShopItem);
}
