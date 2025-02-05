using FMStatsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FMStatsApp.Pages
{
    public class FormationSelecterModel : PageModel
    {
		private readonly IHttpContextAccessor _httpContextAccessor;
		public FormationSelecterModel(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}



		public List<SelectListItem> AvailableFormations { get; set; } = new();
		[BindProperty]
		public string SelectedFormation { get; set; }

		//public List<Player> Players { get; set; }

		public List<Position> AvailablePositions { get; set; } = Enum.GetValues<Position>().ToList();





		public IActionResult OnGet()
        {
			/*AvailableFormations = FormationCatalog.AllFormations
				.Select(f => new SelectListItem { Value = f.Name, Text = f.Name })
				.ToList(); */

			return Page();
		}
    }
}
