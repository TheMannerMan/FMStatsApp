using FMStatsApp.Models;
using System.Runtime.CompilerServices;

namespace FMStatsApp.Services
{
	public class ScoringCalculator
	{

		public List<Role> AddRoleScoring(Player player)
		{
			
			List<Role> roles = [];

			foreach (var roleDef in RoleCatalog.AllRoles)
			{
				int totalscore = 0;
				int weightSum = roleDef.AttributeWeights.Values.Sum();

				foreach (var (attribute, weight) in roleDef.AttributeWeights)
				{
					var propertyInfo = typeof(Player).GetProperty(attribute);
					if (propertyInfo == null)
					{
						// Hantera felet, t.ex. logga ett meddelande eller kasta ett undantag.
						throw new Exception($"Property '{attribute}' saknas i Player-klassen.");
					}

					// Hämta värdet och multiplicera med vikten.
					int attributeValue = (int)propertyInfo.GetValue(player);
					totalscore += attributeValue * weight;
				}

				float roleScore = (float)totalscore / weightSum; 
				var role = new Role(roleDef.Name, roleDef.ShortName, roleScore);
				roles.Add(role);
			}
			return roles;

			/*roles.Add(AdvanceForwardAttack(_player));
			roles.Add(CompleteForwardAttack(_player));
			roles.Add(CompleteForwardSupport(_player));
			roles.Add(FalseNine(_player));
			return roles;

		}

		

		private Role AdvanceForwardAttack(Player player)
		{
			int totalscore = (player.Acceleration * 5)
				+ (player.Agility * 1)
				+ (player.Anticipation * 1)
				+ (player.Balance * 1)
				+ (player.Composure * 3)
				+ (player.Decisions * 1)
				+ (player.Dribbling * 3)
				+ (player.Finishing * 5)
				+ (player.FirstTouch * 3)
				+ (player.OffTheBall * 3)
				+ (player.Pace * 5)
				+ (player.Passing * 1)
				+ (player.Stamina * 1)
				+ (player.Technique * 3)
				+ (player.WorkRate * 1);
			float roleScore = ((float)totalscore / 37);

			return new Role("Advance Forward attack", "afa", roleScore);
		}

		

		private Role CompleteForwardSupport(Player player)
		{
			int totalscore = (player.Acceleration * 5)
				+ (player.Agility * 3)
				+ (player.Anticipation * 3)
				+ (player.Balance * 1)
				+ (player.Composure * 3)
				+ (player.Decisions * 3)
				+ (player.Dribbling * 3)
				+ (player.Finishing * 5)
				+ (player.FirstTouch * 3)
				+ (player.Heading * 3)
				+ (player.JumpingReach * 1)
				+ (player.LongShots * 3)
				+ (player.OffTheBall * 3)
				+ (player.Pace * 5)
				+ (player.Passing * 3)
				+ (player.Stamina * 1)
				+ (player.Strength * 3)
				+ (player.Teamwork * 1)
				+ (player.Technique * 3)
				+ (player.Vision * 3)
				+ (player.WorkRate * 1);
			float roleScore = ((float)totalscore / 51);

			return new Role("Complete Forward support", "cfs", roleScore); */
		}
	}
}

