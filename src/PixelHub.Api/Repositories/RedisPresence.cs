using StackExchange.Redis;

namespace PixelHub.Api.Repositories;

public class RedisPresence
{
    private const string Key = "presence:joueurs";
    private const int OnlineWindowSeconds = 60;
    private readonly IDatabase _db;

    public RedisPresence(IConnectionMultiplexer redis) => _db = redis.GetDatabase();

    public async Task TouchAsync(string pseudo)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        await _db.SortedSetAddAsync(Key, pseudo, now);
    }

    public async Task<long> CountOnlineAsync()
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return await _db.SortedSetLengthAsync(
            Key, now - OnlineWindowSeconds, now);
    }
}