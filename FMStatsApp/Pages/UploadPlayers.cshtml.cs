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
				StatusMessage = "Please select an HTML file to upload.";
				return Page();
			}

			if (!UploadedFile.FileName.EndsWith(".html", StringComparison.OrdinalIgnoreCase))
			{
				StatusMessage = "Only HTML files are allowed.";
				return Page();
			}

			try
			{
				using var stream = UploadedFile.OpenReadStream();
				var parseResult = await _htmlParser.ParsePlayersAsync(stream);

				if (!parseResult.Success)
				{
					StatusMessage = $"Parsing error: {parseResult.ErrorMessage}";
					return Page();
				}

				await _playerSession.SavePlayersAsync(parseResult.Players);
				
				TempData["SuccessMessage"] = $"Successfully uploaded {parseResult.SuccessfullyParsed} players!";
				if (parseResult.ParseErrors.Any())
				{
					TempData["WarningMessage"] = $"Warnings: {parseResult.ParseErrors.Count} rows could not be parsed.";
				}

				return RedirectToPage("/DisplayPlayers");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error uploading file: {FileName}", UploadedFile.FileName);
				StatusMessage = "An unexpected error occurred while uploading.";
				return Page();
			}
		}

		public async Task<IActionResult> OnPostClearDataAsync()
		{
			await _playerSession.ClearPlayersAsync();
			TempData["InfoMessage"] = "Player data has been cleared.";
			return RedirectToPage();
		}
	}
}
