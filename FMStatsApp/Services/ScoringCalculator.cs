using FMStatsApp.Models;

namespace FMStatsApp.Services
{
	public static class ScoringCalculator
	{

		public static int FalseNine(Player player)
		{

			int score = (player.Acceleration * 5)
				+ (player.Pace * 5)
				+ (player.Finishing * 5)
				+ (player.Dribbling * 3)
				+ (player.FirstTouch * 3)
				+ (player.Passing * 3)
				+ (player.Technique * 3)
				+ (player.Composure * 3)
				+ (player.Decisions * 3)
				+ (player.OffTheBall * 3)
				+ (player.Vision * 3)
				+ (player.Agility * 3)
				+ (player.Anticipation)
				+ (player.Flair)
				+ (player.Teamwork)
				+ (player.Balance);
			return score;
		}
	}
}

