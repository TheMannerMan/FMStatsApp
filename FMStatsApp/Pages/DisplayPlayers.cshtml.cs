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
		private readonly IPlayerSessionService _playerSession;

		[BindProperty]
		public PlayerFilter Filter { get; set; } = new();

		[BindProperty]
		public List<string> SelectedRoles { get; set; } = new();

		public List<Player> Players { get; set; } = new();
		public List<RoleDefinition> AvailableRoles { get; set; } = new();
		public List<string> AvailablePositions { get; set; } = new();
		public List<string> AvailableNationalities { get; set; } = new();

		public List<string> ForwardRoles { get; set; } = RoleCatalog.AllRoles
			.Where(r => r.GeneralPosition == GeneralPosition.Forward)
			.Select(r => r.Name)
			.ToList();

		public List<string> MidfielderRoles { get; set; } = RoleCatalog.AllRoles
			.Where(r => r.GeneralPosition == GeneralPosition.Midfielder)
			.Select(r => r.Name)
			.ToList();

		public List<string> DefenderRoles { get; set; } = RoleCatalog.AllRoles
			.Where(r => r.GeneralPosition == GeneralPosition.Defender)
			.Select(r => r.Name)
			.ToList();

		public List<string> GoalkeeperRoles { get; set; } = RoleCatalog.AllRoles
			.Where(r => r.GeneralPosition == GeneralPosition.Goalkeeper)
			.Select(r => r.Name)
			.ToList();

		public DisplayPlayersModel(IPlayerSessionService playerSession)
		{
			_playerSession = playerSession;
		}

		public async Task<IActionResult> OnGetAsync()
		{
			await LoadDataAsync();
			return Page();
		}

		public async Task<IActionResult> OnPostFilterAsync()
		{
			var allPlayers = await _playerSession.GetPlayersAsync();
			Players = allPlayers.Where(p => Filter.Matches(p)).ToList();
			await LoadDropdownDataAsync();
			return Page();
		}

		public async Task<IActionResult> OnPostShowRoleScoresAsync()
		{
			var allPlayers = await _playerSession.GetPlayersAsync();
			
			// Filter players and show only selected role scores (ensure Wage etc copied)
			Players = allPlayers.Select(p => new Player
			{
				// Basic info
				Name = p.Name,
				Age = p.Age,
				Position = p.Position,
				Nationality = p.Nationality,
				Club = p.Club,
				Wage = p.Wage, // ensure wage persists
				TransferValue = p.TransferValue,
				Reg = p.Reg,
				Inf = p.Inf,
				SecondNationality = p.SecondNationality,
				Personality = p.Personality,
				MediaHandling = p.MediaHandling,
				AverageRating = p.AverageRating,
				LeftFoot = p.LeftFoot,
				RightFoot = p.RightFoot,
				Height = p.Height,
				UID = p.UID,
				Corners = p.Corners,
				// Attributes
				Acceleration = p.Acceleration,
				Agility = p.Agility,
				Anticipation = p.Anticipation,
				Balance = p.Balance,
				Composure = p.Composure,
				Decisions = p.Decisions,
				Dribbling = p.Dribbling,
				Finishing = p.Finishing,
				FirstTouch = p.FirstTouch,
				Heading = p.Heading,
				JumpingReach = p.JumpingReach,
				LongShots = p.LongShots,
				Marking = p.Marking,
				OffTheBall = p.OffTheBall,
				Pace = p.Pace,
				Passing = p.Passing,
				Positioning = p.Positioning,
				Stamina = p.Stamina,
				Strength = p.Strength,
				Tackling = p.Tackling,
				Teamwork = p.Teamwork,
				Technique = p.Technique,
				Vision = p.Vision,
				WorkRate = p.WorkRate,
				AerialAbility = p.AerialAbility,
				Aggression = p.Aggression,
				Bravery = p.Bravery,
				CommandOfArea = p.CommandOfArea,
				Concentration = p.Concentration,
				Determination = p.Determination,
				Flair = p.Flair,
				Handling = p.Handling,
				Kicking = p.Kicking,
				Leadership = p.Leadership,
				Reflexes = p.Reflexes,
				Throwing = p.Throwing,
				ThrowOuts = p.ThrowOuts,
				OneVsOne = p.OneVsOne,
				// Roles filtered
				Roles = p.Roles.Where(r => SelectedRoles.Contains(r.ShortName) || SelectedRoles.Contains(r.Name)).ToList()
			}).ToList();

			await LoadDropdownDataAsync();
			return Page();
		}

		public async Task<IActionResult> OnPostDeletePlayerAsync(long uid)
		{
			var players = await _playerSession.GetPlayersAsync();
			var toRemove = players.FirstOrDefault(p => p.UID == uid);
			if (toRemove != null)
			{
				players.Remove(toRemove);
				await _playerSession.SavePlayersAsync(players);
			}
			await LoadDropdownDataAsync();
			Players = players;
			return Page();
		}

		private async Task LoadDataAsync()
		{
			Players = await _playerSession.GetPlayersAsync();
			await LoadDropdownDataAsync();
		}

		private async Task LoadDropdownDataAsync()
		{
			AvailableRoles = RoleCatalog.AllRoles;
			AvailablePositions = Players.Select(p => p.Position).Distinct().OrderBy(p => p).ToList();
			AvailableNationalities = Players.Select(p => p.Nationality).Distinct().OrderBy(n => n).ToList();
		}
	}
}
