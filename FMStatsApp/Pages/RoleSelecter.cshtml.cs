using FMStatsApp.Models;
using FMStatsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FMStatsApp.Pages
{
	public class RoleSelecterModel : PageModel
	{
		private readonly IPlayerSessionService _playerSession;
		private readonly IStartingXIOptimizerService _optimizerService;
		private readonly ILogger<RoleSelecterModel> _logger;

		[BindProperty]
		public string SelectedFormationName { get; set; } = string.Empty;

		[BindProperty]
		public Formation? SelectedFormation { get; set; }

		[BindProperty]
		public List<FormationPosition> FormationPositions { get; set; } = new();

		public List<Player> AvailablePlayers { get; set; } = new();
		public List<RoleDefinition> AllRoles { get; set; } = new();

		public RoleSelecterModel(IPlayerSessionService playerSession, IStartingXIOptimizerService optimizerService, ILogger<RoleSelecterModel> logger)
		{
			_playerSession = playerSession;
			_optimizerService = optimizerService;
			_logger = logger;
		}

		public async Task<IActionResult> OnGetAsync(string formationName)
		{
			if (string.IsNullOrWhiteSpace(formationName))
			{
				return RedirectToPage("/BestStartingXI");
			}

			SelectedFormationName = formationName;
			SelectedFormation = FormationCatalog.AllFormations.FirstOrDefault(f => f.Name == formationName);
			
			if (SelectedFormation == null)
			{
				return RedirectToPage("/BestStartingXI");
			}

			try
			{
				AvailablePlayers = await _playerSession.GetPlayersAsync();
				AllRoles = RoleCatalog.AllRoles;

				// Skapa FormationPosition objekt för varje position i formationen
				FormationPositions = CreateFormationPositions(SelectedFormation);

				return Page();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error loading role selector page");
				return RedirectToPage("/BestStartingXI");
			}
		}

		public async Task<IActionResult> OnPostOptimizeAsync()
		{
			try
			{
				SelectedFormation = FormationCatalog.AllFormations.FirstOrDefault(f => f.Name == SelectedFormationName);
				if (SelectedFormation == null)
				{
					return BadRequest("Invalid formation");
				}

				AvailablePlayers = await _playerSession.GetPlayersAsync();
				AllRoles = RoleCatalog.AllRoles;
				
				if (!AvailablePlayers.Any())
				{
					ModelState.AddModelError("", "Inga spelare tillgängliga för optimering.");
					return await OnGetAsync(SelectedFormationName);
				}

				// Kör optimeringen
				var result = _optimizerService.OptimizeStartingXI(SelectedFormation, AvailablePlayers, FormationPositions);
				
				if (!result.Success)
				{
					ModelState.AddModelError("", $"Optimering misslyckades: {result.ErrorMessage}");
					return await OnGetAsync(SelectedFormationName);
				}

				// Uppdatera FormationPositions med optimerade resultat
				foreach (var assignment in result.Assignments)
				{
					var position = FormationPositions.FirstOrDefault(p => p.Index == assignment.Position.Index);
					if (position != null)
					{
						position.SelectedPlayerId = assignment.Player.UID;
						position.SelectedPlayer = assignment.Player;
						position.SelectedRole = assignment.Role.Name;
					}
				}

				// Lägg till meddelande om resultatet
				TempData["OptimizationResult"] = $"Optimering klar! Genomsnittligt betyg: {result.AverageRating:F1}";
				
				return Page();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during optimization");
				ModelState.AddModelError("", "Ett fel uppstod under optimeringen.");
				return await OnGetAsync(SelectedFormationName);
			}
		}

		public async Task<IActionResult> OnPostUpdatePositionAsync([FromBody] UpdatePositionRequest request)
		{
			try
			{
				if (request.PositionIndex < 0 || request.PositionIndex >= FormationPositions.Count)
				{
					return BadRequest("Invalid position index");
				}

				AvailablePlayers = await _playerSession.GetPlayersAsync();
				
				var position = FormationPositions[request.PositionIndex];
				position.SelectedRole = request.SelectedRole;
				position.SelectedPlayerId = request.SelectedPlayerId;

				if (request.SelectedPlayerId.HasValue)
				{
					position.SelectedPlayer = AvailablePlayers.FirstOrDefault(p => p.UID == request.SelectedPlayerId.Value);
				}
				else
				{
					position.SelectedPlayer = null;
				}

				return new JsonResult(new { success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating position");
				return new JsonResult(new { success = false, error = ex.Message });
			}
		}

		public async Task<IActionResult> OnGetPositionDataAsync(int positionIndex)
		{
			try
			{
				if (positionIndex < 0 || positionIndex >= FormationPositions.Count)
				{
					return BadRequest("Invalid position index");
				}

				AvailablePlayers = await _playerSession.GetPlayersAsync();
				var position = FormationPositions[positionIndex];
				
				var rolesForPosition = GetRolesForPosition(position.Position);
				var playersForPosition = GetPlayersForPosition(position.Position);

				return new JsonResult(new
				{
					position = new
					{
						Index = position.Index,
						Position = position.Position.ToString(),
						SelectedRole = position.SelectedRole,
						SelectedPlayerId = position.SelectedPlayerId
					},
					roles = rolesForPosition.Select(r => new { r.Name, r.ShortName }).ToList(),
					players = playersForPosition.Select(p => new { p.UID, p.Name, p.Age, p.Club, p.Position }).ToList()
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting position data");
				return BadRequest("Error loading position data");
			}
		}

		public List<RoleDefinition> GetRolesForPosition(Position position) =>
			AllRoles.Where(r => r.Positions.Contains(position)).ToList();

		public List<Player> GetPlayersForPosition(Position position)
		{
			// Filtrera spelare som kan spela på positionen (enkel filtrering baserat på huvudposition)
			return AvailablePlayers.Where(p => CanPlayerPlayPosition(p, position)).ToList();
		}

		private bool CanPlayerPlayPosition(Player player, Position position)
		{
			// Enkel mappning - kan förbättras med mer avancerad logik
			var playerPosition = player.Position?.ToUpper();
			
			return position switch
			{
				Position.GK => playerPosition == "GK",
				Position.DL or Position.DR or Position.DC => playerPosition?.Contains("D") == true || playerPosition == "CB" || playerPosition == "LB" || playerPosition == "RB",
				Position.WBL or Position.WBR => playerPosition?.Contains("WB") == true || playerPosition?.Contains("B") == true,
				Position.DM or Position.MC or Position.ML or Position.MR => playerPosition?.Contains("M") == true && !playerPosition.Contains("AM"),
				Position.AMC or Position.AML or Position.AMR => playerPosition?.Contains("AM") == true || playerPosition?.Contains("M") == true,
				Position.ST => playerPosition?.Contains("ST") == true || playerPosition?.Contains("CF") == true || playerPosition == "F",
				_ => true // Fallback - tillåt alla spelare
			};
		}

		private List<FormationPosition> CreateFormationPositions(Formation formation)
		{
			var positions = new List<FormationPosition>();
			var positionCounts = new Dictionary<Position, int>();

			for (int i = 0; i < formation.Positions.Count; i++)
			{
				var pos = formation.Positions[i];
				
				// Räkna hur många av denna position vi redan har
				if (!positionCounts.ContainsKey(pos))
					positionCounts[pos] = 0;
				
				var positionIndex = positionCounts[pos];
				positionCounts[pos]++;

				var formationPosition = new FormationPosition(i, pos);
				
				// Beräkna grid position baserat på position och antal
				CalculateGridPositionForFormation(formationPosition, positionIndex, formation);
				
				positions.Add(formationPosition);
			}

			return positions;
		}

		private void CalculateGridPositionForFormation(FormationPosition position, int positionIndex, Formation formation)
		{
			// Räkna totala antalet av varje position i formationen
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
	}

	public class UpdatePositionRequest
	{
		public int PositionIndex { get; set; }
		public string? SelectedRole { get; set; }
		public long? SelectedPlayerId { get; set; }
	}
}
