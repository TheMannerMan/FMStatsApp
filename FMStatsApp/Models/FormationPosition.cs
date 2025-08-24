using System.ComponentModel.DataAnnotations;

namespace FMStatsApp.Models
{
	public class FormationPosition
	{
		public int Index { get; set; }
		public Position Position { get; set; }
		public string? SelectedRole { get; set; }
		public long? SelectedPlayerId { get; set; }
		public Player? SelectedPlayer { get; set; }
		
		// L�sningsfunktionalitet
		public bool IsRoleLocked { get; set; }
		public bool IsPlayerLocked { get; set; }
		
		// F�r visual layout
		public int GridRow { get; set; }
		public int GridColumn { get; set; }
		
		public FormationPosition(int index, Position position)
		{
			Index = index;
			Position = position;
			CalculateGridPosition();
		}
		
		public FormationPosition() { } // Parameterless constructor f�r model binding
		
		// Helper properties
		public bool IsLocked => IsRoleLocked || IsPlayerLocked;
		public bool HasSelectedPlayer => SelectedPlayerId.HasValue && SelectedPlayer != null;
		public bool HasSelectedRole => !string.IsNullOrEmpty(SelectedRole);
		
		private void CalculateGridPosition()
		{
			// R�kna hur m�nga av samma position vi har innan denna
			int positionCount = Index; // Anv�nd index direkt f�r enklare ber�kning
			
			// Ber�kna grid position baserat p� position type
			// Grid �r 5 rader x 5 kolumner
			switch (Position)
			{
				case Position.GK:
					GridRow = 5;
					GridColumn = 3;
					break;
				case Position.DL:
					GridRow = 4;
					GridColumn = 1;
					break;
				case Position.DC:
					// F�rsta DC p� kolumn 2, andra p� kolumn 4
					GridRow = 4;
					GridColumn = (positionCount % 2 == 0) ? 2 : 4;
					break;
				case Position.DR:
					GridRow = 4;
					GridColumn = 5;
					break;
				case Position.WBL:
					GridRow = 4;
					GridColumn = 1;
					break;
				case Position.WBR:
					GridRow = 4;
					GridColumn = 5;
					break;
				case Position.DM:
					// F�rsta DM p� kolumn 2, andra p� kolumn 4
					GridRow = 3;
					GridColumn = (positionCount % 2 == 0) ? 2 : 4;
					break;
				case Position.ML:
					GridRow = 3;
					GridColumn = 1;
					break;
				case Position.MC:
					// F�rdela MC �ver kolumn 2, 3, 4
					GridRow = 3;
					GridColumn = 2 + (positionCount % 3);
					break;
				case Position.MR:
					GridRow = 3;
					GridColumn = 5;
					break;
				case Position.AML:
					GridRow = 2;
					GridColumn = 1;
					break;
				case Position.AMC:
					GridRow = 2;
					GridColumn = 3;
					break;
				case Position.AMR:
					GridRow = 2;
					GridColumn = 5;
					break;
				case Position.ST:
					// F�rsta ST p� kolumn 2, andra p� kolumn 4
					GridRow = 1;
					GridColumn = (positionCount % 2 == 0) ? 2 : 4;
					break;
				default:
					GridRow = 1;
					GridColumn = 1;
					break;
			}
		}
	}
}