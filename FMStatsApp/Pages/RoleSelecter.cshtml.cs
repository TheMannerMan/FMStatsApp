using FMStatsApp.Models;
using FMStatsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FMStatsApp.Pages
{
	public class RoleSelecterModel : PageModel
	{

		[BindProperty]
		public string SelectedFormationName { get; set; }

		[BindProperty]
		public Formation SelectedFormation { get; set; }

		[BindProperty]
		public Dictionary<Position, string> SelectedRoles { get; set; } = new();

		public List<RoleDefinition> GetRolesForPosition(Position position) =>
			RoleService.GetRolesForPosition(position);

		public void OnGet(string formationName)
		{
			
			SelectedFormationName = formationName;
			SelectedFormation = FormationCatalog.AllFormations.FirstOrDefault(f => f.Name == formationName);
		}

		public IActionResult OnPost()
		{
			if (SelectedRoles.Count == SelectedFormation.Positions.Count)
			{
				return RedirectToPage("NextStep", new { formation = SelectedFormationName });
			}

			ModelState.AddModelError("", "All positions must have an assigned role.");
			return Page();
		}
	}
}
