using FMStatsApp.Extensions;
using FMStatsApp.Models;
using FMStatsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FMStatsApp.Pages
{
	public class DisplayPlayersModel : PageModel
	{
		//private readonly PlayerStorageService _service;

		public List<Player> Players { get; set; }
		private readonly IHttpContextAccessor _httpContextAccessor;
		
		public DisplayPlayersModel(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public IActionResult OnGet()
		{
			Players = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<Player>>("Players");
			return Page();
		}
	}
}
