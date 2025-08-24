using FMStatsApp.Models;
using FMStatsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FMStatsApp.Pages
{
	public class BestStartingXIModel : PageModel
	{
		private readonly IPlayerSessionService _playerSession;
		private readonly ILogger<BestStartingXIModel> _logger;

		public List<Formation> Formations { get; private set; } = new();
		public bool NoPlayersAvailable { get; private set; }

		public BestStartingXIModel(IPlayerSessionService playerSession, ILogger<BestStartingXIModel> logger)
		{
			_playerSession = playerSession;
			_logger = logger;
		}

		public async Task OnGetAsync()
		{
			Formations = FormationCatalog.AllFormations;
			
			try 
			{
				var players = await _playerSession.GetPlayersAsync();
				NoPlayersAvailable = players.Count == 0;
				_logger.LogInformation("BestStartingXI: Found {count} players", players.Count);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error checking for players in BestStartingXI");
				NoPlayersAvailable = true;
			}
		}

		public async Task<IActionResult> OnPostSelectAsync(string formationName)
		{
			if (string.IsNullOrWhiteSpace(formationName))
			{
				return BadRequest("Formation name is required");
			}

			var formation = FormationCatalog.AllFormations.FirstOrDefault(f => f.Name == formationName);
			if (formation == null)
			{
				return BadRequest("Invalid formation");
			}

			// Redirect to the role selector page with formation name
			return RedirectToPage("/RoleSelecter", new { formationName = formationName });
		}
	}
}
