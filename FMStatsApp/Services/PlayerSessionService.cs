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
        private readonly ILogger<PlayerSessionService> _logger;
        private const string PLAYERS_KEY = "Players";

        public PlayerSessionService(IHttpContextAccessor httpContextAccessor, ILogger<PlayerSessionService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<List<Player>> GetPlayersAsync()
        {
            try
            {
                var session = _httpContextAccessor.HttpContext?.Session;
                if (session == null)
                {
                    _logger.LogWarning("Session is null when trying to get players");
                    return new List<Player>();
                }
                
                var players = session.GetObjectFromJson<List<Player>>(PLAYERS_KEY);
                
                // Log outcome
                if (players == null)
                {
                    _logger.LogInformation("No players found in session");
                    return new List<Player>();
                }
                
                _logger.LogDebug("Retrieved {count} players from session", players.Count);
                return players;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving players from session");
                return new List<Player>();
            }
        }

        public async Task SavePlayersAsync(List<Player> players)
        {
            try 
            {
                var session = _httpContextAccessor.HttpContext?.Session;
                if (session == null)
                {
                    _logger.LogWarning("Session is null when trying to save players");
                    return;
                }
                
                session.SetObjectAsJson(PLAYERS_KEY, players);
                _logger.LogInformation("Saved {count} players to session", players.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving players to session");
            }
            await Task.CompletedTask;
        }

        public async Task ClearPlayersAsync()
        {
            try
            {
                var session = _httpContextAccessor.HttpContext?.Session;
                if (session == null)
                {
                    _logger.LogWarning("Session is null when trying to clear players");
                    return;
                }
                
                session.Remove(PLAYERS_KEY);
                _logger.LogInformation("Cleared players from session");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing players from session");
            }
            await Task.CompletedTask;
        }

        public bool HasPlayers()
        {
            try
            {
                var session = _httpContextAccessor.HttpContext?.Session;
                if (session == null)
                {
                    _logger.LogWarning("Session is null when checking for players");
                    return false;
                }
                
                var raw = session.GetString(PLAYERS_KEY);
                if (string.IsNullOrWhiteSpace(raw) || raw == "[]")
                {
                    _logger.LogDebug("No players found in session: {raw}", raw ?? "null");
                    return false;
                }
                
                _logger.LogDebug("Players found in session");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking for players in session");
                return false;
            }
        }

        public async Task<int> GetPlayerCountAsync()
        {
            var players = await GetPlayersAsync();
            return players?.Count ?? 0;
        }
    }
}