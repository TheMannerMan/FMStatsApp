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
				// Filtrera bort positioner som redan har en manuellt vald spelare
				var positionsToOptimize = positions.Where(p => p.SelectedPlayerId == null).ToList();
				var availablePlayersForOptimization = availablePlayers.Where(p => !positions.Any(pos => pos.SelectedPlayerId == p.UID)).ToList();

				if (!positionsToOptimize.Any() || !availablePlayersForOptimization.Any())
				{
					return new OptimizationResult 
					{ 
						Success = true, 
						Assignments = new List<PlayerAssignment>(),
						TotalScore = 0,
						AverageRating = 0
					};
				}

				// Skapa kostmatris (vi vill maximera score, så använder negativa värden)
				var costMatrix = new double[positionsToOptimize.Count, availablePlayersForOptimization.Count];
				
				for (int i = 0; i < positionsToOptimize.Count; i++)
				{
					var position = positionsToOptimize[i];
					var bestRole = GetBestRoleForPosition(position.Position, position.SelectedRole);
					
					for (int j = 0; j < availablePlayersForOptimization.Count; j++)
					{
						var player = availablePlayersForOptimization[j];
						var score = CalculatePlayerRoleScore(player, bestRole);
						costMatrix[i, j] = -score; // Negativ eftersom Hungarian minimerar
					}
				}

				// Kör Hungarian algoritm
				var (assignment, totalCost) = HungarianAlgorithm.Solve(costMatrix);
				
				// Skapa resultat
				var assignments = new List<PlayerAssignment>();
				double totalScore = 0;
				
				for (int i = 0; i < Math.Min(assignment.Length, positionsToOptimize.Count); i++)
				{
					if (assignment[i] < availablePlayersForOptimization.Count)
					{
						var position = positionsToOptimize[i];
						var player = availablePlayersForOptimization[assignment[i]];
						var bestRole = GetBestRoleForPosition(position.Position, position.SelectedRole);
						var score = CalculatePlayerRoleScore(player, bestRole);
						
						assignments.Add(new PlayerAssignment
						{
							Position = position,
							Player = player,
							Role = bestRole,
							Score = score
						});
						
						totalScore += score;
					}
				}

				return new OptimizationResult
				{
					Success = true,
					Assignments = assignments,
					TotalScore = totalScore,
					AverageRating = assignments.Any() ? totalScore / assignments.Count : 0
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error optimizing starting XI");
				return new OptimizationResult { Success = false, ErrorMessage = ex.Message };
			}
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

		private RoleDefinition GetBestRoleForPosition(Position position, string? selectedRole = null)
		{
			// Om en roll är specifikt vald, använd den
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