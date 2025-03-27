using TheatricalPlayersRefactoringKata.Domain.Enums;

namespace TheatricalPlayersRefactoringKata.Application.DTOs.Request
{
    public class PlayRequest
    {
        public string Name { get; set; }
        public int Lines { get; set; }
        public TypesPlays Type { get; set; }
    }
}
