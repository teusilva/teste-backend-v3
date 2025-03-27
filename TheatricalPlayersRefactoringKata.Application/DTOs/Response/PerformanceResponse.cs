namespace TheatricalPlayersRefactoringKata.Application.DTOs.Response
{
    public class PerformanceResponse
    {
        public Guid Id { get; set; }
        public Guid PlayId { get; set; }
        public Guid InvoceId { get; set; }
        public int Audience { get; set; }
    }
}
