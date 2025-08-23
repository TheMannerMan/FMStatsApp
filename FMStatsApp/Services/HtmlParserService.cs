using FMStatsApp.Models;
using HtmlAgilityPack;
using static FMStatsApp.Models.Player;

namespace FMStatsApp.Services
{
	public interface IHtmlParserService
	{
		Task<ParseResult> ParsePlayersAsync(Stream htmlFileStream);
	}

	public class HtmlParserService : IHtmlParserService
	{
		private readonly IPlayerScoringService _scoringService;
		private readonly ILogger<HtmlParserService> _logger;

		public HtmlParserService(IPlayerScoringService scoringService, ILogger<HtmlParserService> logger)
		{
			_scoringService = scoringService;
			_logger = logger;
		}

		public async Task<ParseResult> ParsePlayersAsync(Stream htmlFileStream)
		{
			try
			{
				var htmlDoc = new HtmlDocument();
				htmlDoc.Load(htmlFileStream);

				var players = new List<Player>();
				var errors = new List<string>();

				var rows = htmlDoc.DocumentNode.SelectNodes("//table/tr[position()>1]");
				if (rows == null)
				{
					return new ParseResult { Success = false, ErrorMessage = "No table rows found in HTML file." };
				}

				foreach (var (row, index) in rows.Select((row, i) => (row, i)))
				{
					try
					{
						var player = await ParsePlayerFromRowAsync(row, index);
						if (player != null)
						{
							player.Roles = _scoringService.CalculateRoleScores(player);
							players.Add(player);
						}
					}
					catch (Exception ex)
					{
						var error = $"Error parsing row {index + 1}: {ex.Message}";
						_logger.LogWarning(ex, error);
						errors.Add(error);
					}
				}

				return new ParseResult
				{
					Success = true,
					Players = players,
					ParseErrors = errors,
					TotalRowsProcessed = rows.Count,
					SuccessfullyParsed = players.Count
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Critical error during HTML parsing");
				return new ParseResult
				{
					Success = false,
					ErrorMessage = $"Critical parsing error: {ex.Message}"
				};
			}
		}

		private async Task<Player?> ParsePlayerFromRowAsync(HtmlNode row, int rowIndex)
		{
			var columns = row.SelectNodes("td");
			if (columns == null || columns.Count < 54)
			{
				_logger.LogWarning($"Row {rowIndex + 1} has insufficient columns: {columns?.Count ?? 0}");
				return null;
			}

			return new Player
			{
				Reg = GetSafeText(columns[0]),
				Inf = GetSafeText(columns[1]),
				Name = GetSafeText(columns[2]),
				Age = ParseInt(columns[3]),
				Wage = ParseWage(columns[4]),
				TransferValue = PlayerParser.ParseTransferValue(GetSafeText(columns[5])),
				Nationality = GetSafeText(columns[6]),
				SecondNationality = GetSafeText(columns[7]),
				Position = GetSafeText(columns[8]),
				Personality = GetSafeText(columns[9]),
				MediaHandling = GetSafeText(columns[10]),
				AverageRating = ParseDouble(columns[11]),
				LeftFoot = GetSafeText(columns[12]),
				RightFoot = GetSafeText(columns[13]),
				Height = ParseHeight(columns[14]),
				OneVsOne = ParseInt(columns[15]),
				Acceleration = ParseInt(columns[16]),
				AerialAbility = ParseInt(columns[17]),
				Aggression = ParseInt(columns[18]),
				Agility = ParseInt(columns[19]),
				Anticipation = ParseInt(columns[20]),
				Balance = ParseInt(columns[21]),
				Bravery = ParseInt(columns[22]),
				CommandOfArea = ParseInt(columns[23]),
				Concentration = ParseInt(columns[24]),
				Composure = ParseInt(columns[25]),
				Crossing = ParseInt(columns[26]),
				Decisions = ParseInt(columns[27]),
				Determination = ParseInt(columns[28]),
				Dribbling = ParseInt(columns[29]),
				Finishing = ParseInt(columns[30]),
				FirstTouch = ParseInt(columns[31]),
				Flair = ParseInt(columns[32]),
				Handling = ParseInt(columns[33]),
				Heading = ParseInt(columns[34]),
				JumpingReach = ParseInt(columns[35]),
				Kicking = ParseInt(columns[36]),
				Leadership = ParseInt(columns[37]),
				LongShots = ParseInt(columns[38]),
				Marking = ParseInt(columns[39]),
				OffTheBall = ParseInt(columns[40]),
				Pace = ParseInt(columns[41]),
				Passing = ParseInt(columns[42]),
				Positioning = ParseInt(columns[43]),
				Reflexes = ParseInt(columns[44]),
				Stamina = ParseInt(columns[45]),
				Strength = ParseInt(columns[46]),
				Tackling = ParseInt(columns[47]),
				Teamwork = ParseInt(columns[48]),
				Technique = ParseInt(columns[49]),
				Throwing = ParseInt(columns[50]),
				ThrowOuts = ParseInt(columns[51]),
				Vision = ParseInt(columns[52]),
				WorkRate = ParseInt(columns[53]),
				UID = ParseLong(columns[54]),
				Corners = ParseInt(columns[55]),
				Club = GetSafeText(columns[56])
			};
		}

		private string GetSafeText(HtmlNode node) => node?.InnerText?.Trim() ?? string.Empty;
		private int ParseInt(HtmlNode node) => int.TryParse(GetSafeText(node), out var result) ? result : 0;
		private long ParseLong(HtmlNode node) => long.TryParse(GetSafeText(node), out var result) ? result : 0;
		private double ParseDouble(HtmlNode node) => double.TryParse(GetSafeText(node), out var result) ? result : 0;
		private int ParseWage2(HtmlNode node) => int.TryParse(GetSafeText(node).Replace("€", "").Replace("p/w", "").Replace(",", "").Trim(), out var result) ? result : 0;

		private int ParseWage(HtmlNode node)
		{
			var wageInPlaneText = GetSafeText(node);
			var wageTrimmed = wageInPlaneText.Replace("€", "").Replace("p/w", "").Replace(",", "").Trim();

			return int.TryParse(wageTrimmed, out var result) ? result : 0;

			//if (text.EndsWith("K", StringComparison.OrdinalIgnoreCase))
			//{
			//	if (double.TryParse(text[..^1], out var value))
			//	{
			//		return (int)(value * 1000);
			//	}
			//}
			//else if (text.EndsWith("M", StringComparison.OrdinalIgnoreCase))
			//{
			//	if (double.TryParse(text[..^1], out var value))
			//	{
			//		return (int)(value * 1_000_000);
			//	}
			//}
			//else if (int.TryParse(text.Replace(",", ""), out var value))
			//{
			//	return value;
			//}
			//return 0;
		}

		private int ParseHeight(HtmlNode node) => int.TryParse(GetSafeText(node).Replace(" cm", ""), out var result) ? result : 0;
	}
}