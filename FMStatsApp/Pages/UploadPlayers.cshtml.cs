using FMStatsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;


namespace FMStatsApp.Pages
{
	public class UploadPlayersModel : PageModel
	{
		private readonly PlayerStorageService _service;
		private readonly HtmlParser _htmlParser;

		[BindProperty]
		public IFormFile UploadedFile { get; set; }

		public IActionResult OnGet()
		{
			return Page();
		}
		public IActionResult OnPostUpload()
		{
			if (UploadedFile != null && UploadedFile.Length > 0)
			{
				using var stream = UploadedFile.OpenReadStream();
				var parsedPlayers = _htmlParser.ParsedPlayers(stream);
				_service.AddPlayers(parsedPlayers);
			}

			return RedirectToPage("/DisplayPlayers");
		}
		public UploadPlayersModel(HtmlParser htmlParser, PlayerStorageService service)
		{
			_htmlParser = htmlParser;
			_service = service;
		}
	}
}
