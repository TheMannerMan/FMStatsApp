using System.Text.Json.Serialization;

namespace FMStatsApp.Models
{
	public record Role
	{
		public string RoleName { get; init; }
		public string Name { get; init; }
		public string ShortRoleName { get; init; }
		public string ShortName { get; init; }
		public float RoleScore { get; init; }
		public float Score { get; init; }

		[JsonConstructor]
		public Role(string roleName, string shortRoleName, float roleScore)
		{
			RoleName = roleName;
			Name = roleName; // För bakåtkompatibilitet
			ShortRoleName = shortRoleName;
			ShortName = shortRoleName; // För bakåtkompatibilitet
			RoleScore = roleScore;
			Score = roleScore; // För bakåtkompatibilitet
		}

		public Role() { }
	}
}
