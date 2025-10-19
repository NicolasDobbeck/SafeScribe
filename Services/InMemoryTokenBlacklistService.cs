using System.Collections.Concurrent;

namespace SafeScribe.Services
{
    public class InMemoryTokenBlacklistService : ITokenBlacklistService
    {
        private readonly ConcurrentDictionary<string, byte> _blacklist = new();

        public Task AddToBlacklistAsync(string jti)
        {
            _blacklist.TryAdd(jti, 0);

            return Task.CompletedTask;
        }

        public Task<bool> IsBlacklistedAsync(string jti)
        {
            bool isBlacklisted = _blacklist.ContainsKey(jti);

            return Task.FromResult(isBlacklisted);
        }
    }
}