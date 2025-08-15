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
			// GOALKEEPER ROLES
			new RoleDefinition(
				Name: "Goalkeeper (Defend)",
				ShortName: "GKD",
				GeneralPosition: GeneralPosition.Goalkeeper,
				Positions: new List<Position> { Position.GK },
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
					{ "JumpingReach", 0 },
					{ "Kicking", 3 },
					{ "Passing", 3 },
					{ "Positioning", 5 },
					{ "Reflexes", 5 },
								{ "Throwing", 1 }
				}),

			new RoleDefinition(
				Name: "Sweeper Keeper (Defend)",
				ShortName: "SKD",
				GeneralPosition: GeneralPosition.Goalkeeper,
				Positions: new List<Position> { Position.GK },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "OneVsOne", 3 },
					{ "Acceleration", 1 },
					{ "AerialAbility", 1 },
					{ "Agility", 5 },
					{ "Anticipation", 3 },
					{ "CommandOfArea", 3 },
					{ "Concentration", 3 },
					{ "Decisions", 1 },
					{ "FirstTouch", 1 },
					{ "Handling", 1 },
					{ "Kicking", 3 },
					{ "Passing", 1 },
					{ "Positioning", 3 },
					{ "Reflexes", 5 },
					{ "ThrowOuts", 1 },
					{ "Vision", 1 }
				}),

			new RoleDefinition(
				Name: "Sweeper Keeper (Support)",
				ShortName: "SKS",
				GeneralPosition: GeneralPosition.Goalkeeper,
				Positions: new List<Position> { Position.GK },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "OneVsOne", 3 },
					{ "Acceleration", 1 },
					{ "AerialAbility", 1 },
					{ "Agility", 5 },
					{ "Anticipation", 3 },
					{ "CommandOfArea", 3 },
					{ "Concentration", 3 },
					{ "Decisions", 1 },
					{ "FirstTouch", 1 },
					{ "Handling", 1 },
					{ "Kicking", 3 },
					{ "Passing", 1 },
					{ "Positioning", 3 },
					{ "Reflexes", 5 },
					{ "ThrowOuts", 1 },
					{ "Vision", 1 }
				}),

			new RoleDefinition(
				Name: "Sweeper Keeper (Attack)",
				ShortName: "SKA",
				GeneralPosition: GeneralPosition.Goalkeeper,
				Positions: new List<Position> { Position.GK },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "OneVsOne", 3 },
					{ "Acceleration", 1 },
					{ "AerialAbility", 1 },
					{ "Agility", 5 },
					{ "Anticipation", 3 },
					{ "CommandOfArea", 3 },
					{ "Concentration", 3 },
					{ "Decisions", 1 },
					{ "FirstTouch", 1 },
					{ "Handling", 1 },
					{ "Kicking", 3 },
					{ "Passing", 1 },
					{ "Positioning", 3 },
					{ "Reflexes", 5 },
					{ "ThrowOuts", 1 },
					{ "Vision", 1 }
				}),

			// CENTRAL DEFENDER ROLES
			new RoleDefinition(
				Name: "Central Defender Defend",
				ShortName: "CDD",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Aggression", 1 },
					{ "Anticipation", 1 },
					{ "Bravery", 1 },
					{ "Concentration", 1 },
					{ "Composure", 5 },
					{ "Decisions", 1 },
					{ "Heading", 3 },
					{ "JumpingReach", 5 },
					{ "Marking", 3 },
					{ "Pace", 5 },
					{ "Positioning", 3 },
					{ "Strength", 3 },
					{ "Tackling", 3 }
				}),

			new RoleDefinition(
				Name: "Central Defender Cover",
				ShortName: "CDC",
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
					{ "Heading", 1 },
					{ "JumpingReach", 5 },
					{ "Marking", 3 },
					{ "Pace", 5 },
					{ "Positioning", 3 },
					{ "Strength", 1 },
					{ "Tackling", 3 }
				}),

			new RoleDefinition(
				Name: "Central Defender Stopper",
				ShortName: "CDS",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Aggression", 3 },
					{ "Anticipation", 1 },
					{ "Bravery", 3 },
					{ "Concentration", 1 },
					{ "Composure", 5 },
					{ "Decisions", 3 },
					{ "Heading", 3 },
					{ "JumpingReach", 5 },
					{ "Marking", 1 },
					{ "Pace", 5 },
					{ "Positioning", 3 },
					{ "Strength", 3 },
					{ "Tackling", 3 }
				}),

			new RoleDefinition(
				Name: "Ball-Playing Defender (Defend)",
				ShortName: "BPDD",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 3 },
					{ "AerialAbility", 3 },
					{ "Anticipation", 3 },
					{ "Bravery", 1 },
					{ "Concentration", 3 },
					{ "Composure", 5 },
					{ "Decisions", 3 },
					{ "FirstTouch", 3 },
					{ "Heading", 3 },
					{ "JumpingReach", 3 },
					{ "Marking", 3 },
					{ "Pace", 3 },
					{ "Passing", 5 },
					{ "Positioning", 3 },
					{ "Strength", 1 },
					{ "Tackling", 3 },
					{ "Technique", 3 },
					{ "Vision", 3 }
				}),

			new RoleDefinition(
				Name: "Ball Playing Defender Cover",
				ShortName: "BPDC",
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
					{ "Strength", 1 },
					{ "Tackling", 3 },
					{ "Technique", 1 },
					{ "Vision", 1 }
				}),

			new RoleDefinition(
				Name: "Ball Playing Defender Stopper",
				ShortName: "BPDS",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Aggression", 3 },
					{ "Anticipation", 1 },
					{ "Bravery", 3 },
					{ "Concentration", 1 },
					{ "Composure", 5 },
					{ "Decisions", 3 },
					{ "FirstTouch", 1 },
					{ "Heading", 3 },
					{ "JumpingReach", 5 },
					{ "Marking", 1 },
					{ "Pace", 5 },
					{ "Passing", 3 },
					{ "Positioning", 3 },
					{ "Strength", 3 },
					{ "Tackling", 3 },
					{ "Technique", 1 },
					{ "Vision", 1 }
				}),

			new RoleDefinition(
				Name: "Nonsense Centre Back Cover",
				ShortName: "NCBC",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Anticipation", 3 },
					{ "Bravery", 1 },
					{ "Concentration", 3 },
					{ "Composure", 5 },
					{ "Heading", 1 },
					{ "JumpingReach", 5 },
					{ "Marking", 3 },
					{ "Pace", 5 },
					{ "Positioning", 3 },
					{ "Strength", 1 },
					{ "Tackling", 3 }
				}),

			new RoleDefinition(
				Name: "Nonsense Centre Back Defend",
				ShortName: "NCBD",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Aggression", 1 },
					{ "Anticipation", 1 },
					{ "Bravery", 1 },
					{ "Concentration", 1 },
					{ "Composure", 5 },
					{ "Heading", 3 },
					{ "JumpingReach", 5 },
					{ "Marking", 3 },
					{ "Pace", 5 },
					{ "Positioning", 3 },
					{ "Strength", 3 },
					{ "Tackling", 3 }
				}),

			new RoleDefinition(
				Name: "Nonsense Centre Back Stopper",
				ShortName: "NCBS",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Aggression", 3 },
					{ "Anticipation", 1 },
					{ "Bravery", 3 },
					{ "Concentration", 1 },
					{ "Composure", 5 },
					{ "Heading", 3 },
					{ "JumpingReach", 5 },
					{ "Marking", 1 },
					{ "Pace", 5 },
					{ "Positioning", 3 },
					{ "Strength", 3 },
					{ "Tackling", 3 }
				}),

			new RoleDefinition(
				Name: "Libero Defend",
				ShortName: "LD",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Anticipation", 1 },
					{ "Bravery", 1 },
					{ "Concentration", 1 },
					{ "Composure", 5 },
					{ "Decisions", 3 },
					{ "FirstTouch", 3 },
					{ "Heading", 3 },
					{ "JumpingReach", 5 },
					{ "Marking", 3 },
					{ "Pace", 5 },
					{ "Passing", 3 },
					{ "Positioning", 3 },
					{ "Stamina", 1 },
					{ "Strength", 3 },
					{ "Tackling", 3 },
					{ "Teamwork", 3 },
					{ "Technique", 3 }
				}),

			new RoleDefinition(
				Name: "Libero Support",
				ShortName: "LS",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Anticipation", 1 },
					{ "Bravery", 1 },
					{ "Concentration", 1 },
					{ "Composure", 5 },
					{ "Decisions", 3 },
					{ "Dribbling", 1 },
					{ "FirstTouch", 3 },
					{ "Heading", 3 },
					{ "JumpingReach", 5 },
					{ "Marking", 3 },
					{ "Pace", 5 },
					{ "Passing", 3 },
					{ "Positioning", 3 },
					{ "Stamina", 1 },
					{ "Strength", 3 },
					{ "Tackling", 3 },
					{ "Teamwork", 3 },
					{ "Technique", 3 },
					{ "Vision", 1 }
				}),

			new RoleDefinition(
				Name: "Wide Centre Back Defend",
				ShortName: "WCBD",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Aggression", 1 },
					{ "Agility", 1 },
					{ "Anticipation", 1 },
					{ "Bravery", 1 },
					{ "Concentration", 1 },
					{ "Composure", 5 },
					{ "Decisions", 1 },
					{ "Dribbling", 1 },
					{ "FirstTouch", 1 },
					{ "Heading", 3 },
					{ "JumpingReach", 5 },
					{ "Marking", 3 },
					{ "Pace", 5 },
					{ "Passing", 1 },
					{ "Positioning", 3 },
					{ "Strength", 3 },
					{ "Tackling", 3 },
					{ "Technique", 1 },
					{ "WorkRate", 1 }
				}),

			new RoleDefinition(
				Name: "Wide Centre Back Support",
				ShortName: "WCBS",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Aggression", 1 },
					{ "Agility", 1 },
					{ "Anticipation", 1 },
					{ "Bravery", 1 },
					{ "Concentration", 1 },
					{ "Composure", 5 },
					{ "Crossing", 1 },
					{ "Decisions", 1 },
					{ "Dribbling", 3 },
					{ "FirstTouch", 1 },
					{ "Heading", 3 },
					{ "JumpingReach", 5 },
					{ "Marking", 3 },
					{ "OffTheBall", 1 },
					{ "Pace", 5 },
					{ "Passing", 1 },
					{ "Positioning", 3 },
					{ "Stamina", 1 },
					{ "Strength", 3 },
					{ "Tackling", 3 },
					{ "Technique", 1 },
					{ "WorkRate", 1 }
				}),

			new RoleDefinition(
				Name: "Wide Centre Back Attack",
				ShortName: "WCBA",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Aggression", 1 },
					{ "Agility", 1 },
					{ "Anticipation", 1 },
					{ "Bravery", 1 },
					{ "Concentration", 1 },
					{ "Composure", 5 },
					{ "Crossing", 3 },
					{ "Decisions", 1 },
					{ "Dribbling", 3 },
					{ "FirstTouch", 1 },
					{ "Heading", 3 },
					{ "JumpingReach", 5 },
					{ "Marking", 3 },
					{ "OffTheBall", 3 },
					{ "Pace", 5 },
					{ "Passing", 1 },
					{ "Positioning", 1 },
					{ "Stamina", 3 },
					{ "Strength", 3 },
					{ "Tackling", 3 },
					{ "Technique", 1 },
					{ "WorkRate", 1 }
				}),

			// FULL-BACK ROLES
			new RoleDefinition(
				Name: "Full-Back (Defend)",
				ShortName: "FBD",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DL, Position.DR },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Anticipation", 3 },
					{ "Concentration", 3 },
					{ "Marking", 3 },
					{ "Pace", 5 },
					{ "Positioning", 3 },
					{ "Stamina", 3 },
					{ "Tackling", 5 },
					{ "Teamwork", 3 }
				}),

			new RoleDefinition(
				Name: "Full-Back (Support)",
				ShortName: "FBS",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DL, Position.DR },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Anticipation", 3 },
					{ "Concentration", 1 },
					{ "Crossing", 3 },
					{ "Decisions", 1 },
					{ "Marking", 3 },
					{ "OffTheBall", 1 },
					{ "Pace", 5 },
					{ "Passing", 1 },
					{ "Positioning", 3 },
					{ "Stamina", 5 },
					{ "Tackling", 3 },
					{ "Teamwork", 3 },
					{ "WorkRate", 3 }
				}),

			new RoleDefinition(
				Name: "Full-Back (Attack)",
				ShortName: "FBA",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.DL, Position.DR },
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
				}),

			// WING-BACK ROLES
			new RoleDefinition(
				Name: "Wing-Back (Support)",
				ShortName: "WBS",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.WBL, Position.WBR },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Agility", 1 },
					{ "Anticipation", 1 },
					{ "Concentration", 1 },
					{ "Crossing", 5 },
					{ "Decisions", 1 },
					{ "Dribbling", 1 },
					{ "FirstTouch", 1 },
					{ "Marking", 1 },
					{ "OffTheBall", 3 },
					{ "Pace", 5 },
					{ "Passing", 3 },
					{ "Positioning", 1 },
					{ "Stamina", 5 },
					{ "Tackling", 1 },
					{ "Teamwork", 3 },
					{ "Technique", 1 },
					{ "WorkRate", 5 }
				}),

			new RoleDefinition(
				Name: "Wing-Back (Attack)",
				ShortName: "WBA",
				GeneralPosition: GeneralPosition.Defender,
				Positions: new List<Position> { Position.WBL, Position.WBR },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Agility", 3 },
					{ "Anticipation", 1 },
					{ "Crossing", 5 },
					{ "Decisions", 1 },
					{ "Dribbling", 3 },
					{ "FirstTouch", 3 },
					{ "Flair", 1 },
					{ "OffTheBall", 5 },
					{ "Pace", 5 },
					{ "Passing", 3 },
					{ "Stamina", 5 },
					{ "Teamwork", 1 },
					{ "Technique", 3 },
					{ "WorkRate", 5 }
				}),

			// DEFENSIVE MIDFIELDER ROLES
			new RoleDefinition(
				Name: "Defensive Midfielder (Defend)",
				ShortName: "DMD",
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.DM },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 3 },
					{ "Anticipation", 5 },
					{ "Concentration", 3 },
					{ "Decisions", 3 },
					{ "Marking", 5 },
					{ "Pace", 3 },
					{ "Positioning", 5 },
					{ "Stamina", 3 },
					{ "Strength", 3 },
					{ "Tackling", 5 },
					{ "Teamwork", 3 },
					{ "WorkRate", 3 }
				}),

			new RoleDefinition(
				Name: "Anchor Man (Defend)",
				ShortName: "AND",
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.DM },
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
				}),

			new RoleDefinition(
				Name: "Deep-Lying Playmaker (Defend)",
				ShortName: "DLPD",
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.DM },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Anticipation", 3 },
					{ "Composure", 5 },
					{ "Concentration", 3 },
					{ "Decisions", 5 },
					{ "FirstTouch", 5 },
					{ "Marking", 1 },
					{ "Passing", 5 },
					{ "Positioning", 3 },
					{ "Stamina", 3 },
					{ "Tackling", 1 },
					{ "Teamwork", 3 },
					{ "Technique", 5 },
					{ "Vision", 5 }
				}),

			// CENTRAL MIDFIELDER ROLES
			new RoleDefinition(
				Name: "Central Midfielder (Support)",
				ShortName: "CMS",
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.MC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Anticipation", 3 },
					{ "Composure", 3 },
					{ "Decisions", 5 },
					{ "FirstTouch", 3 },
					{ "Passing", 5 },
					{ "Positioning", 3 },
					{ "Stamina", 5 },
					{ "Tackling", 3 },
					{ "Teamwork", 5 },
					{ "Technique", 3 },
					{ "Vision", 3 },
					{ "WorkRate", 5 }
				}),

			new RoleDefinition(
				Name: "Box-to-Box Midfielder (Support)",
				ShortName: "BBMS",
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.MC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 3 },
					{ "Anticipation", 3 },
					{ "Decisions", 3 },
					{ "FirstTouch", 3 },
					{ "Marking", 3 },
					{ "OffTheBall", 3 },
					{ "Pace", 3 },
					{ "Passing", 3 },
					{ "Positioning", 3 },
					{ "Stamina", 5 },
					{ "Tackling", 3 },
					{ "Teamwork", 5 },
					{ "Technique", 3 },
					{ "WorkRate", 5 }
				}),

			// ATTACKING MIDFIELDER ROLES
			new RoleDefinition(
				Name: "Advanced Playmaker (Support)",
				ShortName: "APS",
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.AMC, Position.MC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 3 },
					{ "Agility", 1 },
					{ "Anticipation", 1 },
					{ "Composure", 3 },
					{ "Decisions", 3 },
					{ "Dribbling", 1 },
					{ "FirstTouch", 3 },
					{ "Flair", 1 },
					{ "OffTheBall", 3 },
					{ "Pace", 3 },
					{ "Passing", 5 },
					{ "Stamina", 3 },
					{ "Teamwork", 3 },
					{ "Technique", 3 },
					{ "Vision", 5 },
					{ "WorkRate", 3 }
				}),

			new RoleDefinition(
				Name: "Advanced Playmaker (Attack)",
				ShortName: "APA",
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.AMC, Position.MC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 3 },
					{ "Agility", 1 },
					{ "Anticipation", 1 },
					{ "Composure", 3 },
					{ "Decisions", 3 },
					{ "Dribbling", 3 },
					{ "FirstTouch", 3 },
					{ "Flair", 3 },
					{ "OffTheBall", 3 },
					{ "Pace", 3 },
					{ "Passing", 5 },
					{ "Stamina", 3 },
					{ "Teamwork", 1 },
					{ "Technique", 3 },
					{ "Vision", 5 },
					{ "WorkRate", 3 }
				}),

			new RoleDefinition(
				Name: "Attacking Midfielder (Attack)",
				ShortName: "AMA",
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.AMC },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 3 },
					{ "Agility", 1 },
					{ "Anticipation", 3 },
					{ "Composure", 1 },
					{ "Decisions", 3 },
					{ "Dribbling", 3 },
					{ "Finishing", 3 },
					{ "FirstTouch", 3 },
					{ "Flair", 3 },
					{ "LongShots", 3 },
					{ "OffTheBall", 3 },
					{ "Pace", 3 },
					{ "Passing", 3 },
					{ "Stamina", 3 },
					{ "Technique", 3 },
					{ "Vision", 1 },
					{ "WorkRate", 3 }
				}),

			// WINGER ROLES
			new RoleDefinition(
				Name: "Winger (Support)",
				ShortName: "WGS",
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.MR, Position.ML },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Agility", 3 },
					{ "Balance", 1 },
					{ "Crossing", 5 },
					{ "Decisions", 1 },
					{ "Dribbling", 3 },
					{ "FirstTouch", 3 },
					{ "Flair", 1 },
					{ "OffTheBall", 1 },
					{ "Pace", 5 },
					{ "Passing", 1 },
					{ "Stamina", 3 },
					{ "Teamwork", 3 },
					{ "Technique", 3 },
					{ "WorkRate", 3 }
				}),

			new RoleDefinition(
				Name: "Winger (Attack)",
				ShortName: "WGA",
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.MR, Position.ML },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 5 },
					{ "Agility", 3 },
					{ "Balance", 3 },
					{ "Crossing", 3 },
					{ "Decisions", 1 },
					{ "Dribbling", 5 },
					{ "Finishing", 1 },
					{ "FirstTouch", 3 },
					{ "Flair", 3 },
					{ "OffTheBall", 3 },
					{ "Pace", 5 },
					{ "Stamina", 3 },
					{ "Technique", 3 },
					{ "WorkRate", 1 }
				}),

			new RoleDefinition(
				Name: "Inverted Winger (Support)",
				ShortName: "IWS",
				GeneralPosition: GeneralPosition.Midfielder,
				Positions: new List<Position> { Position.MR, Position.ML },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 3 },
					{ "Agility", 3 },
					{ "Balance", 1 },
					{ "Composure", 1 },
					{ "Decisions", 3 },
					{ "Dribbling", 5 },
					{ "FirstTouch", 3 },
					{ "Flair", 1 },
					{ "LongShots", 1 },
					{ "OffTheBall", 1 },
					{ "Pace", 3 },
					{ "Passing", 3 },
					{ "Stamina", 3 },
					{ "Technique", 5 },
					{ "Vision", 1 },
					{ "WorkRate", 1 }
				}),

			// STRIKER ROLES
			new RoleDefinition(
				Name: "Advanced Forward (Attack)",
				ShortName: "AFA",
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
				}),

			new RoleDefinition(
				Name: "Complete Forward (Attack)",
				ShortName: "CFA",
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
				}),

			new RoleDefinition(
				Name:"Complete Forward (Support)",
				ShortName: "CFS",
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
				}),

			new RoleDefinition(
				Name: "False Nine (Support)",
				ShortName: "F9S",
				GeneralPosition: GeneralPosition.Forward,
				Positions: new List<Position> { Position.ST},
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 3 },
					{ "Pace", 3 },
					{ "Finishing", 3 },
					{ "Dribbling", 5 },
					{ "FirstTouch", 5 },
					{ "Passing", 5 },
					{ "Technique", 5 },
					{ "Composure", 3 },
					{ "Decisions", 5 },
					{ "OffTheBall", 1 },
					{ "Vision", 5 },
					{ "Agility", 3 },
					{ "Anticipation", 3 },
					{ "Flair", 3 },
					{ "Teamwork", 3 },
					{ "Balance", 1 },
				}),

			new RoleDefinition(
				Name: "Target Man (Attack)",
				ShortName: "TMA",
				GeneralPosition: GeneralPosition.Forward,
				Positions: new List<Position> { Position.ST },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "AerialAbility", 5 },
					{ "Anticipation", 1 },
					{ "Balance", 3 },
					{ "Bravery", 1 },
					{ "Composure", 1 },
					{ "Decisions", 1 },
					{ "Finishing", 5 },
					{ "Heading", 5 },
					{ "JumpingReach", 5 },
					{ "OffTheBall", 3 },
					{ "Positioning", 3 },
					{ "Strength", 5 },
					{ "Teamwork", 1 }
				}),

			new RoleDefinition(
				Name: "Poacher (Attack)",
				ShortName: "POA",
				GeneralPosition: GeneralPosition.Forward,
				Positions: new List<Position> { Position.ST },
				AttributeWeights: new Dictionary<string, int>
				{
					{ "Acceleration", 3 },
					{ "Anticipation", 5 },
					{ "Composure", 3 },
					{ "Decisions", 1 },
					{ "Finishing", 5 },
					{ "FirstTouch", 3 },
					{ "OffTheBall", 5 },
					{ "Pace", 3 },
					{ "Positioning", 5 }
				})
		};
	}
}
