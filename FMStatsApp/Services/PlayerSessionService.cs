using FMStatsApp.Extensions;
using FMStatsApp.Models;

namespace FMStatsApp.Services
{
    public interface IPlayerSessionService
    {
        Task<List<Player>> GetPlayersAsync();
        Task SavePlayersAsync(List<Player> players);
        Task ClearPlayersAsync();
        bool HasPlayers();
        Task<int> GetPlayerCountAsync();
    }

    public class PlayerSessionService : IPlayerSessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string PLAYERS_KEY = "Players";

        public PlayerSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Player>> GetPlayersAsync()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetObjectFromJson<List<Player>>(PLAYERS_KEY) ?? new List<Player>();
        }

        public async Task SavePlayersAsync(List<Player> players)
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.SetObjectAsJson(PLAYERS_KEY, players);
            await Task.CompletedTask;
        }

        public async Task ClearPlayersAsync()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            session?.Remove(PLAYERS_KEY);
            await Task.CompletedTask;
        }

        public bool HasPlayers()
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            return session?.GetString(PLAYERS_KEY) != null;
        }

        public async Task<int> GetPlayerCountAsync()
        {
            var players = await GetPlayersAsync();
            return players.Count;
        }
    }
}