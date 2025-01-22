using FMStatsApp.Models;
using FMStatsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FMStatsApp.Pages
{
	public class DisplayPlayersModel : PageModel
	{
		private readonly PlayerStorageService _service;

		public List<Player> Players { get; set; }
		public IActionResult OnGet()
		{
			Players = _service.GetAllPlayers();
			return Page();
		}

		public DisplayPlayersModel(PlayerStorageService service)
		{
			_service = service;
		}
	}
}
