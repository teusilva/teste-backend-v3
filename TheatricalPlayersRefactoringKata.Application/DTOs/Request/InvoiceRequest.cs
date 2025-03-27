namespace TheatricalPlayersRefactoringKata.Application.DTOs.Request
{
    public class InvoiceRequest
    {
        public string Customer { get; set; }
        public List<Guid> PerformancesId { get; set; }
    }
}
