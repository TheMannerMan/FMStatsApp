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

		public List<string> ForwardRoles { get; set; } = RoleCatalog.AllRoles
			.Where(r => r.Position == Position.Forward)
			.Select(r => r.Name)
			.ToList();

		public List<string> MidfielderRoles { get; set; } = RoleCatalog.AllRoles
			.Where(r => r.Position == Position.Midfielder)
			.Select(r => r.Name)
			.ToList();

		[BindProperty]
		public List<string> SelectedRoles { get; set; } = new List<string>();

		[BindProperty]
		public List<string> SelectedMidfielderRoles { get; set; } = new List<string>();

		[BindProperty]
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

		public IActionResult OnPost()
		{
			Players = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<Player>>("Players");
			return Page();
		}
	}
}
