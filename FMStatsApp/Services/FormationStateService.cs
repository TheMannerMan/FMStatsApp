using FMStatsApp.Models;
using System.Text.Json;

namespace FMStatsApp.Services
{
    public interface IFormationStateService
    {
        Task<List<FormationPosition>> GetFormationPositionsAsync(string formationName);
        Task SaveFormationPositionsAsync(string formationName, List<FormationPosition> positions);
        Task ClearFormationPositionsAsync(string formationName);
        List<FormationPosition> CreateFormationPositions(Formation formation);
    }

    public class FormationStateService : IFormationStateService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPlayerSessionService _playerSessionService;
        private readonly ILogger<FormationStateService> _logger;
        private const string FormationSessionKeyPrefix = "FormationPositions_";

        public FormationStateService(
            IHttpContextAccessor httpContextAccessor,
            IPlayerSessionService playerSessionService,
            ILogger<FormationStateService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _playerSessionService = playerSessionService;
            _logger = logger;
        }

        public async Task<List<FormationPosition>> GetFormationPositionsAsync(string formationName)
        {
            try
            {
                var sessionKey = FormationSessionKeyPrefix + formationName;
                var session = _httpContextAccessor.HttpContext?.Session;
                
                if (session == null) return new List<FormationPosition>();

                var raw = session.GetString(sessionKey);
                if (string.IsNullOrWhiteSpace(raw)) return new List<FormationPosition>();

                var dto = JsonSerializer.Deserialize<List<FormationPositionDto>>(raw);
                if (dto == null) return new List<FormationPosition>();

                var players = await _playerSessionService.GetPlayersAsync();

                return dto.Select(d => new FormationPosition(d.Index, d.Position)
                {
                    SelectedRole = d.SelectedRole,
                    SelectedPlayerId = d.SelectedPlayerId,
                    SelectedPlayer = d.SelectedPlayerId.HasValue ? players.FirstOrDefault(p => p.UID == d.SelectedPlayerId.Value) : null,
                    IsRoleLocked = d.IsRoleLocked,
                    IsPlayerLocked = d.IsPlayerLocked,
                    GridRow = d.GridRow,
                    GridColumn = d.GridColumn
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting formation positions for {FormationName}", formationName);
                return new List<FormationPosition>();
            }
        }

        public async Task SaveFormationPositionsAsync(string formationName, List<FormationPosition> positions)
        {
            try
            {
                var sessionKey = FormationSessionKeyPrefix + formationName;
                var session = _httpContextAccessor.HttpContext?.Session;
                
                if (session == null) return;

                var dto = positions.Select(p => new FormationPositionDto(
                    p.Index, p.Position, p.SelectedRole, p.SelectedPlayerId, 
                    p.IsRoleLocked, p.IsPlayerLocked, p.GridRow, p.GridColumn)).ToList();

                var json = JsonSerializer.Serialize(dto);
                session.SetString(sessionKey, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving formation positions for {FormationName}", formationName);
            }
        }

        public async Task ClearFormationPositionsAsync(string formationName)
        {
            try
            {
                var sessionKey = FormationSessionKeyPrefix + formationName;
                var session = _httpContextAccessor.HttpContext?.Session;
                session?.Remove(sessionKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing formation positions for {FormationName}", formationName);
            }
        }

        public List<FormationPosition> CreateFormationPositions(Formation formation)
        {
            var positions = new List<FormationPosition>();
            var positionCounts = new Dictionary<Position, int>();

            for (int i = 0; i < formation.Positions.Count; i++)
            {
                var pos = formation.Positions[i];
                
                if (!positionCounts.ContainsKey(pos))
                    positionCounts[pos] = 0;
                
                var positionIndex = positionCounts[pos];
                positionCounts[pos]++;

                var formationPosition = new FormationPosition(i, pos);
                CalculateGridPositionForFormation(formationPosition, positionIndex, formation);
                positions.Add(formationPosition);
            }

            return positions;
        }

        private void CalculateGridPositionForFormation(FormationPosition position, int positionIndex, Formation formation)
        {
            var positionCount = formation.Positions.Count(p => p == position.Position);

            switch (position.Position)
            {
                case Position.GK:
                    position.GridRow = 5;
                    position.GridColumn = 3;
                    break;
                case Position.DL:
                    position.GridRow = 4;
                    position.GridColumn = 1;
                    break;
                case Position.DC:
                    position.GridRow = 4;
                    if (positionCount == 1)
                        position.GridColumn = 3;
                    else if (positionCount == 2)
                        position.GridColumn = positionIndex == 0 ? 2 : 4;
                    else // 3 CB
                        position.GridColumn = positionIndex == 0 ? 2 : (positionIndex == 1 ? 3 : 4);
                    break;
                case Position.DR:
                    position.GridRow = 4;
                    position.GridColumn = 5;
                    break;
                case Position.WBL:
                    position.GridRow = 4;
                    position.GridColumn = 1;
                    break;
                case Position.WBR:
                    position.GridRow = 4;
                    position.GridColumn = 5;
                    break;
                case Position.DM:
                    position.GridRow = 3;
                    if (positionCount == 1)
                        position.GridColumn = 3;
                    else
                        position.GridColumn = positionIndex == 0 ? 2 : 4;
                    break;
                case Position.ML:
                    position.GridRow = 3;
                    position.GridColumn = 1;
                    break;
                case Position.MC:
                    position.GridRow = 3;
                    if (positionCount == 1)
                        position.GridColumn = 3;
                    else if (positionCount == 2)
                        position.GridColumn = positionIndex == 0 ? 2 : 4;
                    else // 3 MC
                        position.GridColumn = 2 + positionIndex;
                    break;
                case Position.MR:
                    position.GridRow = 3;
                    position.GridColumn = 5;
                    break;
                case Position.AML:
                    position.GridRow = 2;
                    position.GridColumn = 1;
                    break;
                case Position.AMC:
                    position.GridRow = 2;
                    position.GridColumn = 3;
                    break;
                case Position.AMR:
                    position.GridRow = 2;
                    position.GridColumn = 5;
                    break;
                case Position.ST:
                    position.GridRow = 1;
                    if (positionCount == 1)
                        position.GridColumn = 3;
                    else
                        position.GridColumn = positionIndex == 0 ? 2 : 4;
                    break;
                default:
                    position.GridRow = 1;
                    position.GridColumn = 1;
                    break;
            }
        }

        private record FormationPositionDto(int Index, Position Position, string? SelectedRole, long? SelectedPlayerId, bool IsRoleLocked, bool IsPlayerLocked, int GridRow, int GridColumn);
    }
}