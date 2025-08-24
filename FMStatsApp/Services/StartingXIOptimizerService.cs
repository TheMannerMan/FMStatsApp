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
				// Separera låsta och olåsta positioner
				var lockedPositions = positions.Where(p => p.IsLocked).ToList();
				var unlockedPositions = positions.Where(p => !p.IsLocked).ToList();

				// Skapa lista över spelare som redan är assignade till låsta positioner
				var assignedPlayerIds = lockedPositions
					.Where(p => p.HasSelectedPlayer)
					.Select(p => p.SelectedPlayerId!.Value)
					.ToHashSet();

				// Filtrera tillgängliga spelare för optimering (exkludera redan assignade)
				var availablePlayersForOptimization = availablePlayers
					.Where(p => !assignedPlayerIds.Contains(p.UID))
					.ToList();

				if (!unlockedPositions.Any())
				{
					// Alla positioner är låsta, returnera befintliga assignments
					return CreateResultFromLockedPositions(lockedPositions);
				}

				if (!availablePlayersForOptimization.Any())
				{
					return new OptimizationResult 
					{ 
						Success = false, 
						ErrorMessage = "Inga spelare tillgängliga för optimering efter låsningar."
					};
				}

				// Skapa kostmatris för olåsta positioner
				var costMatrix = new double[unlockedPositions.Count, availablePlayersForOptimization.Count];
				
				for (int i = 0; i < unlockedPositions.Count; i++)
				{
					var position = unlockedPositions[i];
					var bestRole = GetBestRoleForPosition(position.Position, position.SelectedRole, position.IsRoleLocked);
					
					for (int j = 0; j < availablePlayersForOptimization.Count; j++)
					{
						var player = availablePlayersForOptimization[j];
						var score = CalculatePlayerRoleScore(player, bestRole);
						costMatrix[i, j] = -score; // Negativ eftersom Hungarian minimerar
					}
				}

				// Kör Hungarian algoritm
				var (assignment, totalCost) = HungarianAlgorithm.Solve(costMatrix);
				
				// Skapa resultat för optimerade positioner
				var optimizedAssignments = new List<PlayerAssignment>();
				
				for (int i = 0; i < Math.Min(assignment.Length, unlockedPositions.Count); i++)
				{
					if (assignment[i] < availablePlayersForOptimization.Count)
					{
						var position = unlockedPositions[i];
						var player = availablePlayersForOptimization[assignment[i]];
						var bestRole = GetBestRoleForPosition(position.Position, position.SelectedRole, position.IsRoleLocked);
						var score = CalculatePlayerRoleScore(player, bestRole);
						
						optimizedAssignments.Add(new PlayerAssignment
						{
							Position = position,
							Player = player,
							Role = bestRole,
							Score = score
						});
					}
				}

				// Kombinera låsta och optimerade assignments
				var allAssignments = new List<PlayerAssignment>();
				
				// Lägg till låsta assignments
				foreach (var lockedPos in lockedPositions)
				{
					if (lockedPos.HasSelectedPlayer)
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
				}
				
				// Lägg till optimerade assignments
				allAssignments.AddRange(optimizedAssignments);

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

		private OptimizationResult CreateResultFromLockedPositions(List<FormationPosition> lockedPositions)
		{
			var assignments = new List<PlayerAssignment>();
			
			foreach (var position in lockedPositions.Where(p => p.HasSelectedPlayer))
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
			// Om rollen är låst och specificerad, använd den
			if (isRoleLocked && !string.IsNullOrEmpty(selectedRole))
			{
				var lockedRole = RoleCatalog.AllRoles.FirstOrDefault(r => r.Name == selectedRole);
				if (lockedRole != null && lockedRole.Positions.Contains(position))
				{
					return lockedRole;
				}
			}

			// Om en roll är specificerad (men inte låst), använd den
			if (!string.IsNullOrEmpty(selectedRole))
			{
				var specificRole = RoleCatalog.AllRoles.FirstOrDefault(r => r.Name == selectedRole);
				if (specificRole != null && specificRole.Positions.Contains(position))
				{
					return specificRole;
				}
			}

			// Annars hitta den första rollen som matchar positionen
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