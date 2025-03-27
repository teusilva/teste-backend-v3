namespace TheatricalPlayersRefactoringKata.Domain.Entities
{
    public class Performance : BaseEntity
    {
        public Guid PlayId { get; set; }
        public int Audience { get; set; }
        public virtual Play Play { get; set; }
    }
}
