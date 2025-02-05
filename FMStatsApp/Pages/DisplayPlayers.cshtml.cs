using FMStatsApp.Extensions;
using FMStatsApp.Models;
using FMStatsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FMStatsApp.Pages
{
	public class DisplayPlayersModel : PageModel
	{
		//private readonly PlayerStorageService _service;

		//public List<SelectListItem> AvailableFormations { get; set; } = new();

		//[BindProperty]
		//public string SelectedFormation { get; set; }

		//public List<Position> AvailablePositions { get; set; } = Enum.GetValues<Position>().ToList();


		//[BindProperty]
		//public Dictionary<Position, string> SelectedRoles { get; set; } = new();

		public List<string> ForwardRoles { get; set; } = RoleCatalog.AllRoles
			.Where(r => r.GeneralPosition == GeneralPosition.Forward)
			.Select(r => r.Name)
			.ToList();

		public List<string> MidfielderRoles { get; set; } = RoleCatalog.AllRoles
			.Where(r => r.GeneralPosition == GeneralPosition.Midfielder)
			.Select(r => r.Name)
			.ToList();

		[BindProperty]
		public List<string> SelectedRolesToList { get; set; } = new List<string>();

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

			/*AvailableFormations = FormationCatalog.AllFormations
				.Select(f => new SelectListItem { Value = f.Name, Text = f.Name })
				.ToList(); */

			return Page();
		}

		public IActionResult OnPost()
		{
			Players = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<Player>>("Players");
			return Page();
		}

		/*public IActionResult OnPostFormation()
		{
			if (string.IsNullOrEmpty(SelectedFormation))
			{
				ModelState.AddModelError("SelectedFormation", "Välj en formation.");
				return Page();
			}

			// Här kan vi hantera vad som händer när en formation väljs
			return RedirectToPage("NextPage"); // Ändra till rätt sida
		} */
	}
}
