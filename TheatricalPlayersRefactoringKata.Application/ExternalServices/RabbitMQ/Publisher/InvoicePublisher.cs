using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace TheatricalPlayersRefactoringKata.Application.ExternalServices.RabbitMQ.Publisher
{
    public class InvoicePublisher : IInvoicePublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public InvoicePublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "invoice_queue",
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        public void Publish(Guid invoiceId)
        {
            var message = JsonSerializer.Serialize(new { InvoiceId = invoiceId });
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: "invoice_queue",
                                  basicProperties: null,
                                  body: body);
        }
    }
}
