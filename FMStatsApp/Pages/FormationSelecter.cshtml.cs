using FMStatsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.CompilerServices;

namespace FMStatsApp.Pages
{
    public class FormationSelecterModel : PageModel
    {
		//private readonly IHttpContextAccessor _httpContextAccessor;
		public SelectList AvailableFormations { get; set; }
		public Formation SelectedFormation { get; set; } = null;

		[BindProperty]
		public string SelectedFormationName { get; set; }

		/*public FormationSelecterModel(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
			
		} */

		//public List<Player> Players { get; set; }

		public List<Position> AvailablePositions { get; set; } = Enum.GetValues<Position>().ToList();

		private SelectList GetFormations()
		{
			List<Formation> formations = FormationCatalog.AllFormations;
			return new SelectList(formations, nameof(Formation.Name), nameof(Formation.Name));
		}



		public IActionResult OnGet()
        {
			/*AvailableFormations = FormationCatalog.AllFormations
				.Select(f => new SelectListItem { Value = f.Name, Text = f.Name })
				.ToList(); */
			AvailableFormations = GetFormations();

			return Page();
		}

		public IActionResult OnPost()
		{
			//SelectedFormation = FormationCatalog.AllFormations.Find(f => f.Name == SelectedFormationName);
			return RedirectToPage("RoleSelecter", new { formationName = SelectedFormationName });
		}

	}
}
