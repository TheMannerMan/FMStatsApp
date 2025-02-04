using FMStatsApp.Models;

namespace FMStatsApp.Models
{

	public enum Position
	{
		Goalkeeper,
		Defender,
		Midfielder,
		Forward
	}

	public record RoleDefinition(string Name, string ShortName, Position Position, Dictionary<string, int> AttributeWeights);

	public static class RoleCatalog
	{
		public static List<RoleDefinition> AllRoles { get; } = new List<RoleDefinition>
		{

			new RoleDefinition(
				Name: "Advance Forward Attack",
				ShortName: "afa",
				Position: Position.Forward,
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Agility", 1 },
					{ "Anticipation", 1 },
					{ "Balance", 1 },
					{ "Composure", 3 },
					{ "Decisions", 1 },
					{ "Dribbling", 3 },
					{ "Finishing", 5 },
					{ "FirstTouch", 3 },
					{ "OffTheBall", 3 },
					{ "Pace", 5 },
					{ "Passing", 1 },
					{ "Stamina", 1 },
					{ "Technique", 3 },
					{ "WorkRate", 1 },
				}
			),
			new RoleDefinition(
				Name: "Advance Playmaker Attack",
				ShortName: "apa",
				Position: Position.Midfielder,
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Agility", 1 },
					{ "Anticipation", 1 },
					{ "Composure", 3 },
					{ "Decisions", 3 },
					{ "Dribbling", 1 },
					{ "FirstTouch", 3 },
					{ "Flair", 1 },
					{ "OffTheBall", 3 },
					{ "Pace", 5 },
					{ "Passing", 3 },
					{ "Stamina", 5 },
					{ "Teamwork", 3},
					{ "Technique", 3 },
					{ "Vision", 3 },
					{ "WorkRate", 5 },
				}
				),
			new RoleDefinition(
				Name: "Advance Playmaker Support",
				ShortName: "aps",
				Position: Position.Midfielder,
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Agility", 1 },
					{ "Anticipation", 1 },
					{ "Composure", 3 },
					{ "Decisions", 3 },
					{ "Dribbling", 1 },
					{ "FirstTouch", 3 },
					{ "Flair", 1 },
					{ "OffTheBall", 3 },
					{ "Pace", 5 },
					{ "Passing", 3 },
					{ "Stamina", 5 },
					{ "Teamwork", 3},
					{ "Technique", 3 },
					{ "Vision", 3 },
					{ "WorkRate", 5 },
				}
				),
			new RoleDefinition(
				Name: "Anchor Defend",
				ShortName: "ad",
				Position: Position.Midfielder,
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Anticipation", 3 },
					{ "Concentration", 3 },
					{ "Composure", 1 },
					{ "Decisions", 3 },
					{ "Marking", 3 },
					{ "Pace", 5 },
					{ "Positioning", 3 },
					{ "Stamina", 5 },
					{ "Strength", 1 },
					{ "Tackling", 3 },
					{ "Teamwork", 1},
					{ "WorkRate", 5 }
				}
				),
			new RoleDefinition(
				Name: "Attacking Midfielder Attack",
				ShortName: "ama",
				Position: Position.Midfielder,
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Agility", 1 },
					{ "Anticipation", 3 },
					{ "Composure", 1 },
					{ "Decisions", 3 },
					{ "Dribbling", 3 },
					{ "Finishing", 1 },
					{ "FirstTouch", 3 },
					{ "Flair", 3 },
					{ "LongShots", 3 },
					{ "OffTheBall", 3 },
					{ "Pace", 5 },
					{ "Passing", 3 },
					{ "Stamina", 5 },
					{ "Technique", 3 },
					{ "Vision", 1 },
					{ "WorkRate", 5 },
				}
				),
			new RoleDefinition(
				Name: "Complete Forward Attack",
				ShortName: "cfa",
				Position: Position.Forward,
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Agility", 3 },
					{ "Anticipation", 3 },
					{ "Balance", 1 },
					{ "Composure", 3 },
					{ "Decisions", 1 },
					{ "Dribbling", 3 },
					{ "Finishing", 5 },
					{ "FirstTouch", 3 },
					{ "Heading", 3 },
					{ "JumpingReach", 1 },
					{ "LongShots", 1 },
					{ "OffTheBall", 3 },
					{ "Pace", 5 },
					{ "Passing", 1 },
					{ "Stamina", 1 },
					{ "Strength", 3 },
					{ "Teamwork", 1 },
					{ "Technique", 3 },
					{ "Vision", 1 },
					{ "WorkRate", 1 },
				}
			),
			new RoleDefinition(
				Name:"Complete Forward Support",
				ShortName: "cfs",
				Position: Position.Forward,
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Agility", 3 },
					{ "Anticipation", 3 },
					{ "Balance", 1 },
					{ "Composure", 3 },
					{ "Decisions", 3 },
					{ "Dribbling", 3 },
					{ "Finishing", 5 },
					{ "FirstTouch", 3 },
					{ "Heading", 3 },
					{ "JumpingReach", 1 },
					{ "LongShots", 3 },
					{ "OffTheBall", 3 },
					{ "Pace", 5 },
					{ "Passing", 3 },
					{ "Stamina", 1 },
					{ "Strength", 3 },
					{ "Teamwork", 1 },
					{ "Technique", 3 },
					{ "Vision", 3 },
					{ "WorkRate", 1 }
				}
				),

			new RoleDefinition(

				Name: "False Nine Support",
				ShortName: "f9s",
				Position: Position.Forward,
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Pace", 5 },
					{ "Finishing", 5 },
					{ "Dribbling", 3 },
					{ "FirstTouch", 3 },
					{ "Passing", 3 },
					{ "Technique", 3 },
					{ "Composure", 3 },
					{ "Decisions", 3 },
					{ "OffTheBall", 3 },
					{ "Vision", 3 },
					{ "Agility", 3 },
					{ "Anticipation", 1 },
					{ "Flair", 1 },
					{ "Teamwork", 1 },
					{ "Balance", 1 },
				}
			),
		};
	}
}
