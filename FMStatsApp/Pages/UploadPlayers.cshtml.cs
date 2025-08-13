using FMStatsApp.Models;
using FMStatsApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using System.Text.Json;


namespace FMStatsApp.Pages
{
	public class UploadPlayersModel : PageModel
	{
		private readonly IHtmlParserService _htmlParser;
		private readonly IPlayerSessionService _playerSession;
		private readonly ILogger<UploadPlayersModel> _logger;

		[BindProperty]
		public IFormFile UploadedFile { get; set; }

		public string StatusMessage { get; set; } = string.Empty;
		public bool HasExistingData { get; private set; }
		public int ExistingPlayerCount { get; private set; }

		public UploadPlayersModel(
			IHtmlParserService htmlParser, 
			IPlayerSessionService playerSession,
			ILogger<UploadPlayersModel> logger)
		{
			_htmlParser = htmlParser;
			_playerSession = playerSession;
			_logger = logger;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			HasExistingData = _playerSession.HasPlayers();
			if (HasExistingData)
			{
				ExistingPlayerCount = await _playerSession.GetPlayerCountAsync();
			}
			return Page();
		}

		public async Task<IActionResult> OnPostUploadAsync()
		{
			if (UploadedFile == null || UploadedFile.Length == 0)
			{
				StatusMessage = "Vänligen välj en HTML-fil att ladda upp.";
				return Page();
			}

			if (!UploadedFile.FileName.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
			{
				StatusMessage = "Endast HTML-filer är tillåtna.";
				return Page();
			}

			try
			{
				using var stream = UploadedFile.OpenReadStream();
				var parseResult = await _htmlParser.ParsePlayersAsync(stream);

				if (!parseResult.Success)
				{
					StatusMessage = $"Fel vid parsning: {parseResult.ErrorMessage}";
					return Page();
				}

				await _playerSession.SavePlayersAsync(parseResult.Players);
				
				TempData["SuccessMessage"] = $"Lyckades ladda upp {parseResult.SuccessfullyParsed} spelare!";
				if (parseResult.ParseErrors.Any())
				{
					TempData["WarningMessage"] = $"Varningar: {parseResult.ParseErrors.Count} rader kunde inte parsas.";
				}

				return RedirectToPage("/DisplayPlayers");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error uploading file: {FileName}", UploadedFile.FileName);
				StatusMessage = "Ett oväntat fel inträffade vid uppladdning.";
				return Page();
			}
		}

		public async Task<IActionResult> OnPostClearDataAsync()
		{
			await _playerSession.ClearPlayersAsync();
			TempData["InfoMessage"] = "Speldata har rensats.";
			return RedirectToPage();
		}
	}
}
