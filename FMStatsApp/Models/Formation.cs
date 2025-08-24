namespace FMStatsApp.Models
{

	public enum Position
	{
		GK,  // Målvakt
		DL, DC, DR, // Försvarare
		WBL, WBR, // Wingbacks
		DM, // Defensiv mittfältare
		ML, MC, MR, // Centrala och breda mittfältare
		AML, AMC, AMR, // Offensiva mittfältare
		ST // Anfallare
	}

	public class Formation
	{
		public string Name { get; set; } = string.Empty;
		public List<Position> Positions { get; set; } = new();
		public string? Description { get; set; } // Kort beskrivning
		public string? ImageUrl { get; set; } // Plats för taktik-bild

		// Parameterless constructor for model binding
		public Formation()
		{
		}

		public Formation(string name, List<Position> positions, string? description = null, string? imageUrl = null)
		{
			Name = name;
			Positions = positions;
			Description = description;
			ImageUrl = imageUrl;
		}
	}

	// Exempel på formationer
	public static class FormationCatalog
	{
		public static List<Formation> AllFormations { get; } = new List<Formation>
		{
			new Formation(
				"4-4-2",
				new List<Position> { Position.GK, Position.DL, Position.DC, Position.DC, Position.DR, Position.ML, Position.MC, Position.MC, Position.MR, Position.ST, Position.ST },
				"Balans mellan försvar och anfall. Två anfallare för konstant hot."),
			new Formation(
				"4-2-3-1",
				new List<Position> { Position.GK, Position.DL, Position.DC, Position.DC, Position.DR, Position.DM, Position.DM, Position.AML, Position.AMC, Position.AMR, Position.ST },
				"Stark offensiv fyrväg bakom ensam striker och dubbel pivot."),
			new Formation(
				"5-3-2",
				new List<Position> { Position.GK, Position.WBL, Position.DC, Position.DC, Position.DC, Position.WBR, Position.MC, Position.MC, Position.MC, Position.ST, Position.ST },
				"Tre mittbackar och wingbacks ger bredd och defensiv stabilitet."),
			new Formation(
				"4-3-3",
				new List<Position> { Position.GK, Position.DL, Position.DC, Position.DC, Position.DR, Position.MC, Position.MC, Position.MC, Position.AML, Position.AMR, Position.ST },
				"Pressande spel med bred fronttrea och kontrollerat mittfält."),
			new Formation(
				"3-5-2",
				new List<Position> { Position.GK, Position.DC, Position.DC, Position.DC, Position.WBL, Position.WBR, Position.MC, Position.MC, Position.MC, Position.ST, Position.ST },
				"Wingbacks driver bredd; tre centrala mittfältare för kontroll."),
			new Formation(
				"4-1-4-1",
				new List<Position> { Position.GK, Position.DL, Position.DC, Position.DC, Position.DR, Position.DM, Position.ML, Position.MC, Position.MC, Position.MR, Position.ST },
				"Defensiv pivot skyddar backlinjen; kompakt mittfält."),
			new Formation(
				"4-3-1-2",
				new List<Position> { Position.GK, Position.DL, Position.DC, Position.DC, Position.DR, Position.MC, Position.MC, Position.MC, Position.AMC, Position.ST, Position.ST },
				"Smalt spel genom mitten med kreativ tia bakom två forwards."),
			new Formation(
				"4-4-1-1",
				new List<Position> { Position.GK, Position.DL, Position.DC, Position.DC, Position.DR, Position.ML, Position.MC, Position.MC, Position.MR, Position.AMC, Position.ST },
				"Klassisk struktur med understödjande tia bakom striker."),
			new Formation(
				"3-4-3",
				new List<Position> { Position.GK, Position.DC, Position.DC, Position.DC, Position.WBL, Position.WBR, Position.MC, Position.MC, Position.AML, Position.AMR, Position.ST },
				"Aggressiv offensiv med tre forwards och två wingbacks."),
		};
	}
}
