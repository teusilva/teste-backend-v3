using TheatricalPlayersRefactoringKata.Domain.Enums;

namespace TheatricalPlayersRefactoringKata.Domain.Entities
{
    public class Play : BaseEntity
    {
        public string Name { get; set; } //Nome da peça 
        public int Lines { get; set; } // falas
        public TypesPlays Type { get; set; }
    }
}
