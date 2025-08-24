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

		private const string FormationSessionKey = "FormationPositions"; // session key

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

				// Försök ladda tidigare sparade formation-positioner från session
				if (!LoadPositionsFromSession(SelectedFormation))
				{
					// Skapa nya om inga finns
					FormationPositions = CreateFormationPositions(SelectedFormation);
					SavePositionsToSession();
				}

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
				if (SelectedFormation == null) return BadRequest("Invalid formation");

				// Ladda sessionens senaste state (inkl lås)
				if (!LoadPositionsFromSession(SelectedFormation))
				{
					// Om vi inte kan ladda från session, skapa nya
					FormationPositions = CreateFormationPositions(SelectedFormation);
				}

				AvailablePlayers = await _playerSession.GetPlayersAsync();
				AllRoles = RoleCatalog.AllRoles;

				if (!AvailablePlayers.Any())
				{
					ModelState.AddModelError("", "Inga spelare tillgängliga för optimering.");
					return Page();
				}

				// Kör optimering med nuvarande FormationPositions (inkl låsningar)
				var result = _optimizerService.OptimizeStartingXI(SelectedFormation, AvailablePlayers, FormationPositions);

				if (!result.Success)
				{
					ModelState.AddModelError("", $"Optimering misslyckades: {result.ErrorMessage}");
					return Page();
				}

				// Uppdatera FormationPositions med optimeringsresultatet
				// Men respektera låsningar - skriv bara tillbaka till olåsta positioner
				foreach (var assignment in result.Assignments)
				{
					var pos = FormationPositions.FirstOrDefault(p => p.Index == assignment.Position.Index);
					if (pos != null)
					{
						// Uppdatera roll endast om den inte är låst
						if (!pos.IsRoleLocked)
						{
							pos.SelectedRole = assignment.Role.Name;
						}
						
						// Uppdatera spelare endast om den inte är låst
						if (!pos.IsPlayerLocked)
						{
							pos.SelectedPlayerId = assignment.Player.UID;
							pos.SelectedPlayer = assignment.Player;
						}
					}
				}

				SavePositionsToSession();

				var lockedCount = FormationPositions.Count(p => p.IsLocked);
				var lockedMessage = lockedCount > 0 ? $" ({lockedCount} låsta positioner respekterades)" : "";
				TempData["OptimizationResult"] = $"Optimering klar! Genomsnittligt betyg: {result.AverageRating:F1}{lockedMessage}";
				
				return Page();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during optimization");
				ModelState.AddModelError("", "Ett fel uppstod under optimeringen.");
				return Page();
			}
		}

		public async Task<IActionResult> OnPostUpdatePositionAsync([FromBody] UpdatePositionRequest request)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(request.FormationName))
				{
					SelectedFormationName = request.FormationName!;
				}
				SelectedFormation = FormationCatalog.AllFormations.FirstOrDefault(f => f.Name == SelectedFormationName);
				LoadPositionsFromSession(SelectedFormation); // se till att listan finns

				if (request.PositionIndex < 0 || request.PositionIndex >= FormationPositions.Count)
				{
					return BadRequest("Invalid position index");
				}

				AvailablePlayers = await _playerSession.GetPlayersAsync();
				
				var position = FormationPositions[request.PositionIndex];
				
				// Kontrollera om spelare redan är vald på annan position
				if (request.SelectedPlayerId.HasValue)
				{
					var existingPosition = FormationPositions.FirstOrDefault(p => 
						p.Index != request.PositionIndex && 
						p.SelectedPlayerId == request.SelectedPlayerId.Value);
					
					if (existingPosition != null)
					{
						// Släpp spelare från tidigare position om den inte är låst
						if (!existingPosition.IsPlayerLocked)
						{
							existingPosition.SelectedPlayerId = null;
							existingPosition.SelectedPlayer = null;
						}
						else
						{
							return new JsonResult(new { 
								success = false, 
								error = "Spelare är låst på annan position" 
							});
						}
					}
				}

				// Uppdatera positionen endast om den inte är låst
				if (!position.IsRoleLocked || string.IsNullOrEmpty(position.SelectedRole))
				{
					position.SelectedRole = request.SelectedRole;
				}

				if (!position.IsPlayerLocked)
				{
					position.SelectedPlayerId = request.SelectedPlayerId;
					
					if (request.SelectedPlayerId.HasValue)
					{
						position.SelectedPlayer = AvailablePlayers.FirstOrDefault(p => p.UID == request.SelectedPlayerId.Value);
					}
					else
					{
						position.SelectedPlayer = null;
					}
				}

				SavePositionsToSession();

				return new JsonResult(new { success = true });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating position");
				return new JsonResult(new { success = false, error = ex.Message });
			}
		}

		public async Task<IActionResult> OnPostToggleRoleLockAsync([FromBody] ToggleLockRequest request)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(request.FormationName))
				{
					SelectedFormationName = request.FormationName!;
				}
				SelectedFormation = FormationCatalog.AllFormations.FirstOrDefault(f => f.Name == SelectedFormationName);
				LoadPositionsFromSession(SelectedFormation);

				if (request.PositionIndex < 0 || request.PositionIndex >= FormationPositions.Count)
				{
					return BadRequest("Invalid position index");
				}

				var position = FormationPositions[request.PositionIndex];
				position.IsRoleLocked = !position.IsRoleLocked;

				SavePositionsToSession();

				return new JsonResult(new { 
					success = true, 
					isLocked = position.IsRoleLocked 
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error toggling role lock");
				return new JsonResult(new { success = false, error = ex.Message });
			}
		}

		public async Task<IActionResult> OnPostTogglePlayerLockAsync([FromBody] ToggleLockRequest request)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(request.FormationName))
				{
					SelectedFormationName = request.FormationName!;
				}
				SelectedFormation = FormationCatalog.AllFormations.FirstOrDefault(f => f.Name == SelectedFormationName);
				LoadPositionsFromSession(SelectedFormation);

				if (request.PositionIndex < 0 || request.PositionIndex >= FormationPositions.Count)
				{
					return BadRequest("Invalid position index");
				}

				var position = FormationPositions[request.PositionIndex];
				position.IsPlayerLocked = !position.IsPlayerLocked;

				SavePositionsToSession();

				return new JsonResult(new { 
					success = true, 
					isLocked = position.IsPlayerLocked 
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error toggling player lock");
				return new JsonResult(new { success = false, error = ex.Message });
			}
		}

		public async Task<IActionResult> OnGetPositionDataAsync(int positionIndex, string? formationName = null)
		{
			try
			{
				// Make sure we have the formation set
				if (string.IsNullOrWhiteSpace(SelectedFormationName))
				{
					SelectedFormationName = formationName ?? Request.Query["formationName"].ToString();
				}
				
				if (string.IsNullOrWhiteSpace(SelectedFormationName))
				{
					return BadRequest("Formation name is required");
				}
				
				SelectedFormation = FormationCatalog.AllFormations.FirstOrDefault(f => f.Name == SelectedFormationName);
				if (SelectedFormation == null)
				{
					return BadRequest("Invalid formation");
				}

				LoadPositionsFromSession(SelectedFormation);

				if (positionIndex < 0 || positionIndex >= FormationPositions.Count)
				{
					return BadRequest("Invalid position index");
				}

				AvailablePlayers = await _playerSession.GetPlayersAsync();
				AllRoles = RoleCatalog.AllRoles;
				
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
						SelectedPlayerId = position.SelectedPlayerId,
						IsRoleLocked = position.IsRoleLocked,
						IsPlayerLocked = position.IsPlayerLocked
					},
					roles = rolesForPosition.Select(r => new { name = r.Name, shortName = r.ShortName }).ToList(),
					players = playersForPosition.Select(p => new { uid = p.UID, name = p.Name, age = p.Age, club = p.Club, position = p.Position }).ToList()
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

		public double CalculatePlayerRoleRating(Player player, string roleName)
		{
			if (string.IsNullOrEmpty(roleName)) return 0;
			
			var role = AllRoles.FirstOrDefault(r => r.Name == roleName);
			if (role == null) return 0;
			
			return _optimizerService.CalculatePlayerRoleScore(player, role);
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

		private record FormationPositionDto(int Index, Position Position, string? SelectedRole, long? SelectedPlayerId, bool IsRoleLocked, bool IsPlayerLocked, int GridRow, int GridColumn);

		private void SavePositionsToSession()
		{
			try
			{
				var dto = FormationPositions.Select(p => new FormationPositionDto(p.Index, p.Position, p.SelectedRole, p.SelectedPlayerId, p.IsRoleLocked, p.IsPlayerLocked, p.GridRow, p.GridColumn)).ToList();
				HttpContext.Session.SetString(FormationSessionKey, JsonSerializer.Serialize(new
				{
					formation = SelectedFormationName,
					positions = dto
				}));
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to save formation positions to session");
			}
		}

		private bool LoadPositionsFromSession(Formation? currentFormation)
		{
			try
			{
				var raw = HttpContext.Session.GetString(FormationSessionKey);
				if (string.IsNullOrWhiteSpace(raw)) return false;
				
				var wrapper = JsonSerializer.Deserialize<JsonElement>(raw);
				if (!wrapper.TryGetProperty("formation", out var formNameEl)) return false;
				
				var storedFormationName = formNameEl.GetString();
				
				// Om vi saknar SelectedFormationName (t ex vid AJAX) sätt det från sessionen
				if (string.IsNullOrWhiteSpace(SelectedFormationName) && !string.IsNullOrWhiteSpace(storedFormationName))
					SelectedFormationName = storedFormationName!;
				
				// Fel formation => ignorera
				if (!string.Equals(storedFormationName, SelectedFormationName, StringComparison.OrdinalIgnoreCase)) return false;
				
				if (!wrapper.TryGetProperty("positions", out var positionsEl)) return false;
				
				var list = JsonSerializer.Deserialize<List<FormationPositionDto>>(positionsEl.GetRawText());
				if (list == null || currentFormation == null) return false;

				// Om antal mismatch med aktuell formation - skapa nya
				if (list.Count != currentFormation.Positions.Count) return false;

				// Ensure we have players and roles loaded
				if (!AvailablePlayers.Any())
				{
					AvailablePlayers = _playerSession.GetPlayersAsync().GetAwaiter().GetResult();
				}
				
				if (!AllRoles.Any())
				{
					AllRoles = RoleCatalog.AllRoles;
				}

				FormationPositions = list.Select(d => new FormationPosition(d.Index, d.Position)
				{
					SelectedRole = d.SelectedRole,
					SelectedPlayerId = d.SelectedPlayerId,
					SelectedPlayer = d.SelectedPlayerId.HasValue ? AvailablePlayers.FirstOrDefault(p => p.UID == d.SelectedPlayerId.Value) : null,
					IsRoleLocked = d.IsRoleLocked,
					IsPlayerLocked = d.IsPlayerLocked,
					GridRow = d.GridRow,
					GridColumn = d.GridColumn
				}).ToList();
				
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Failed to load formation positions from session");
				return false;
			}
		}

		public async Task<IActionResult> OnGetCalculateRatingAsync(long playerId, string roleName, string? formationName = null)
		{
			try
			{
				// Make sure we have the formation set
				if (string.IsNullOrWhiteSpace(SelectedFormationName))
				{
					SelectedFormationName = formationName ?? Request.Query["formationName"].ToString();
				}
				
				if (string.IsNullOrWhiteSpace(SelectedFormationName))
				{
					return BadRequest("Formation name is required");
				}
				
				SelectedFormation = FormationCatalog.AllFormations.FirstOrDefault(f => f.Name == SelectedFormationName);
				if (SelectedFormation == null)
				{
					return BadRequest("Invalid formation");
				}

				AvailablePlayers = await _playerSession.GetPlayersAsync();
				AllRoles = RoleCatalog.AllRoles;
				
				var player = AvailablePlayers.FirstOrDefault(p => p.UID == playerId);
				if (player == null)
				{
					return BadRequest("Player not found");
				}
				
				var role = AllRoles.FirstOrDefault(r => r.Name == roleName);
				if (role == null)
				{
					return BadRequest("Role not found");
				}
				
				var rating = _optimizerService.CalculatePlayerRoleScore(player, role);
				
				return new JsonResult(new { rating = rating });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error calculating rating");
				return BadRequest("Error calculating rating");
			}
		}

		public async Task<IActionResult> OnPostResetFormationAsync()
		{
			try
			{
				SelectedFormation = FormationCatalog.AllFormations.FirstOrDefault(f => f.Name == SelectedFormationName);
				if (SelectedFormation == null) return BadRequest("Invalid formation");

				// Skapa nya formation positions (rensar alla val och lås)
				FormationPositions = CreateFormationPositions(SelectedFormation);
				SavePositionsToSession();

				TempData["ResetResult"] = "Startelvan har nollställts!";
				return Page();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error resetting formation");
				ModelState.AddModelError("", "Ett fel uppstod när startelvan skulle nollställas.");
				return Page();
			}
		}
	}

	public class UpdatePositionRequest
	{
		public int PositionIndex { get; set; }
		public string? SelectedRole { get; set; }
		public long? SelectedPlayerId { get; set; }
		public string? FormationName { get; set; }
	}

	public class ToggleLockRequest
	{
		public int PositionIndex { get; set; }
		public string? FormationName { get; set; }
	}
}
