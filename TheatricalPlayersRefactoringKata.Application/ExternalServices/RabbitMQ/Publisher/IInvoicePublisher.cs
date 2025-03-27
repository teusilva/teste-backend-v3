namespace TheatricalPlayersRefactoringKata.Application.ExternalServices.RabbitMQ.Publisher
{
    public interface IInvoicePublisher
    {
        void Publish(Guid invoiceId);
    }
}