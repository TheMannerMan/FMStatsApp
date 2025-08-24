using FMStatsApp.Models;

namespace FMStatsApp.Services
{
	public interface IStartingXIOptimizerService
	{
		OptimizationResult OptimizeStartingXI(Formation formation, List<Player> availablePlayers, List<FormationPosition> positions);
		double CalculatePlayerRoleScore(Player player, RoleDefinition role);
	}

	public class StartingXIOptimizerService : IStartingXIOptimizerService
	{
		private readonly ILogger<StartingXIOptimizerService> _logger;

		public StartingXIOptimizerService(ILogger<StartingXIOptimizerService> logger)
		{
			_logger = logger;
		}

		public OptimizationResult OptimizeStartingXI(Formation formation, List<Player> availablePlayers, List<FormationPosition> positions)
		{
			try
			{
				// Separera helt l�sta positioner fr�n delvis eller helt ol�sta
				var fullyLockedPositions = positions.Where(p => p.IsPlayerLocked && p.HasSelectedPlayer).ToList();
				var positionsToOptimize = positions.Where(p => !p.IsPlayerLocked || !p.HasSelectedPlayer).ToList();

				// Skapa lista �ver spelare som redan �r assignade till helt l�sta positioner
				var assignedPlayerIds = fullyLockedPositions
					.Select(p => p.SelectedPlayerId!.Value)
					.ToHashSet();

				// Filtrera tillg�ngliga spelare f�r optimering (exkludera redan assignade)
				var availablePlayersForOptimization = availablePlayers
					.Where(p => !assignedPlayerIds.Contains(p.UID))
					.ToList();

				if (!positionsToOptimize.Any())
				{
					// Alla positioner har l�sta spelare, returnera befintliga assignments
					return CreateResultFromExistingPositions(positions);
				}

				if (!availablePlayersForOptimization.Any())
				{
					return new OptimizationResult 
					{ 
						Success = false, 
						ErrorMessage = "Inga spelare tillg�ngliga f�r optimering efter l�sningar."
					};
				}

				// Skapa kostmatris f�r positioner som beh�ver optimeras
				var costMatrix = new double[positionsToOptimize.Count, availablePlayersForOptimization.Count];
				
				for (int i = 0; i < positionsToOptimize.Count; i++)
				{
					var position = positionsToOptimize[i];
					var bestRole = GetBestRoleForPosition(position.Position, position.SelectedRole, position.IsRoleLocked);
					
					for (int j = 0; j < availablePlayersForOptimization.Count; j++)
					{
						var player = availablePlayersForOptimization[j];
						var score = CalculatePlayerRoleScore(player, bestRole);
						costMatrix[i, j] = -score; // Negativ eftersom Hungarian minimerar
					}
				}

				// K�r Hungarian algoritm
				var (assignment, totalCost) = HungarianAlgorithm.Solve(costMatrix);
				
				// Skapa resultat f�r alla positioner
				var allAssignments = new List<PlayerAssignment>();
				
				// L�gg till l�sta assignments f�rst
				foreach (var lockedPos in fullyLockedPositions)
				{
					var role = GetBestRoleForPosition(lockedPos.Position, lockedPos.SelectedRole, lockedPos.IsRoleLocked);
					var score = CalculatePlayerRoleScore(lockedPos.SelectedPlayer!, role);
					
					allAssignments.Add(new PlayerAssignment
					{
						Position = lockedPos,
						Player = lockedPos.SelectedPlayer!,
						Role = role,
						Score = score
					});
				}
				
				// L�gg till optimerade assignments
				for (int i = 0; i < Math.Min(assignment.Length, positionsToOptimize.Count); i++)
				{
					if (assignment[i] < availablePlayersForOptimization.Count)
					{
						var position = positionsToOptimize[i];
						var player = availablePlayersForOptimization[assignment[i]];
						var bestRole = GetBestRoleForPosition(position.Position, position.SelectedRole, position.IsRoleLocked);
						var score = CalculatePlayerRoleScore(player, bestRole);
						
						allAssignments.Add(new PlayerAssignment
						{
							Position = position,
							Player = player,
							Role = bestRole,
							Score = score
						});
					}
				}

				var totalScore = allAssignments.Sum(a => a.Score);

				return new OptimizationResult
				{
					Success = true,
					Assignments = allAssignments,
					TotalScore = totalScore,
					AverageRating = allAssignments.Any() ? totalScore / allAssignments.Count : 0
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error optimizing starting XI");
				return new OptimizationResult { Success = false, ErrorMessage = ex.Message };
			}
		}

		private OptimizationResult CreateResultFromExistingPositions(List<FormationPosition> positions)
		{
			var assignments = new List<PlayerAssignment>();
			
			foreach (var position in positions.Where(p => p.HasSelectedPlayer))
			{
				var role = GetBestRoleForPosition(position.Position, position.SelectedRole, position.IsRoleLocked);
				var score = CalculatePlayerRoleScore(position.SelectedPlayer!, role);
				
				assignments.Add(new PlayerAssignment
				{
					Position = position,
					Player = position.SelectedPlayer!,
					Role = role,
					Score = score
				});
			}

			var totalScore = assignments.Sum(a => a.Score);

			return new OptimizationResult
			{
				Success = true,
				Assignments = assignments,
				TotalScore = totalScore,
				AverageRating = assignments.Any() ? totalScore / assignments.Count : 0
			};
		}

		public double CalculatePlayerRoleScore(Player player, RoleDefinition role)
		{
			double totalScore = 0;
			int totalWeight = 0;

			foreach (var attributeWeight in role.AttributeWeights)
			{
				var attributeName = attributeWeight.Key;
				var weight = attributeWeight.Value;
				
				var attributeValue = GetPlayerAttributeValue(player, attributeName);
				totalScore += attributeValue * weight;
				totalWeight += weight;
			}

			return totalWeight > 0 ? (totalScore / totalWeight) : 0;
		}

		private RoleDefinition GetBestRoleForPosition(Position position, string? selectedRole = null, bool isRoleLocked = false)
		{
			// Om rollen �r l�st och specificerad, anv�nd den
			if (isRoleLocked && !string.IsNullOrEmpty(selectedRole))
			{
				var lockedRole = RoleCatalog.AllRoles.FirstOrDefault(r => r.Name == selectedRole);
				if (lockedRole != null && lockedRole.Positions.Contains(position))
				{
					return lockedRole;
				}
			}

			// Om en roll �r specificerad (men inte l�st), anv�nd den
			if (!string.IsNullOrEmpty(selectedRole))
			{
				var specificRole = RoleCatalog.AllRoles.FirstOrDefault(r => r.Name == selectedRole);
				if (specificRole != null && specificRole.Positions.Contains(position))
				{
					return specificRole;
				}
			}

			// Annars hitta den f�rsta rollen som matchar positionen
			return RoleCatalog.AllRoles.FirstOrDefault(r => r.Positions.Contains(position)) 
				?? RoleCatalog.AllRoles.First(); // Fallback
		}

		private int GetPlayerAttributeValue(Player player, string attributeName)
		{
			var property = typeof(Player).GetProperty(attributeName);
			if (property != null && property.PropertyType == typeof(int))
			{
				return (int)(property.GetValue(player) ?? 0);
			}
			return 0;
		}
	}

	public class OptimizationResult
	{
		public bool Success { get; set; }
		public List<PlayerAssignment> Assignments { get; set; } = new();
		public double TotalScore { get; set; }
		public double AverageRating { get; set; }
		public string? ErrorMessage { get; set; }
	}

	public class PlayerAssignment
	{
		public FormationPosition Position { get; set; } = null!;
		public Player Player { get; set; } = null!;
		public RoleDefinition Role { get; set; } = null!;
		public double Score { get; set; }
	}
}