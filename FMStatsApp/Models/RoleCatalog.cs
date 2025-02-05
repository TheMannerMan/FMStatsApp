using FMStatsApp.Models;

namespace FMStatsApp.Models
{

	public enum GeneralPosition
	{
		Goalkeeper,
		Defender,
		Midfielder,
		Forward
	}

	public record RoleDefinition(string Name, string ShortName, GeneralPosition GeneralPosition, List<Position> Positions, Dictionary<string, int> AttributeWeights);

	public static class RoleCatalog
	{
		public static List<RoleDefinition> AllRoles { get; } = new List<RoleDefinition>
		{

			new RoleDefinition(
				Name: "Advance Forward Attack",
				ShortName: "afa",
				GeneralPosition: GeneralPosition.Forward,
				Positions: new List<Position> { Position.ST },
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
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.AMC, Position.AMR, Position.AML, Position.MC },
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
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.AMC, Position.AMR, Position.AML, Position.MC },
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
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.DM},
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
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.AMC },
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
					{ "WorkRate", 5 }
				}
				),
			new RoleDefinition(
				Name: "Ball Playing Defender Cover",
				ShortName: "bpdc",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Anticipation", 3 },
					{ "Bravery", 1 },
					{ "Concentration", 3 },
					{ "Composure", 5 },
					{ "Decisions", 3 },
					{ "FirstTouch", 1 },
					{ "Heading", 1 },
					{ "JumpingReach", 5 },
					{ "Marking", 3 },
					{ "Pace", 5 },
					{ "Passing", 3 },
					{ "Positioning", 3 },
					{ "Strength", 1 },
					{ "Tackling", 3 },
					{ "Technique", 1 },
					{ "Vision", 1 }
				}
				),
			new RoleDefinition(
				Name: "Complete Forward Attack",
				ShortName: "cfa",
				GeneralPosition: GeneralPosition.Forward,
				Positions: new List<Position> { Position.ST},
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
				GeneralPosition: GeneralPosition.Forward,
				Positions: new List<Position> { Position.ST},
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
				GeneralPosition: GeneralPosition.Forward,
				Positions: new List<Position> { Position.ST},
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
				}),
			new RoleDefinition(
				Name: "Full Back Attack",
				ShortName: "fba",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DL },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Agility", 1 },
					{ "Anticipation", 3 },
					{ "Concentration", 1 },
					{ "Crossing", 3 },
					{ "Decisions", 1 },
					{ "Dribbling", 1 },
					{ "FirstTouch", 1 },
					{ "Marking", 3 },
					{ "OffTheBall", 1 },
					{ "Pace", 5 },
					{ "Passing", 1 },
					{ "Positioning", 3 },
					{ "Stamina", 5 },
					{ "Tackling", 3 },
					{ "Teamwork", 3 },
					{ "Technique", 1 },
					{ "Vision", 1 },
					{ "WorkRate", 5 }
				}
				),
			new RoleDefinition(

				Name: "Goalkeeper Defend",
				ShortName: "gkd",
				GeneralPosition: GeneralPosition.Goalkeeper,
				Positions: new List<Position> { Position.GK},
				AttributeWeights: new Dictionary<string, int>
				{
					{ "OneVsOne", 1 },
					{ "AerialAbility", 3 },
					{ "Agility", 5 },
					{ "Anticipation", 1 },
					{ "CommandOfArea", 3 },
					{ "Concentration", 3 },
					{ "Decisions", 1 },
					{ "Handling", 3 },
					{ "Kicking", 3 },
					{ "Positioning", 3 },
					{ "Reflexes", 5 },
					{ "Throwing", 1 }
				})
		};
	}
}
