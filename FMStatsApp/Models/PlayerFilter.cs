using FMStatsApp.Models;

namespace FMStatsApp.Models
{
    public class PlayerFilter
    {
        public string? Position { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public string? Nationality { get; set; }
        public List<string> RequiredRoles { get; set; } = new();

        public bool Matches(Player player)
        {
            if (!string.IsNullOrEmpty(Position) && player.Position != Position) return false;
            if (MinAge.HasValue && player.Age < MinAge) return false;
            if (MaxAge.HasValue && player.Age > MaxAge) return false;
            if (!string.IsNullOrEmpty(Nationality) && player.Nationality != Nationality) return false;
            
            return true;
        }
    }
}