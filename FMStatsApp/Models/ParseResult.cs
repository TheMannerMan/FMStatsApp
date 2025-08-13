using FMStatsApp.Models;

namespace FMStatsApp.Models
{
    public class ParseResult
    {
        public bool Success { get; set; }
        public List<Player> Players { get; set; } = new();
        public List<string> ParseErrors { get; set; } = new();
        public int TotalRowsProcessed { get; set; }
        public int SuccessfullyParsed { get; set; }
        public string? ErrorMessage { get; set; }
    }
}