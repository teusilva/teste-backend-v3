using MediatR;
using Microsoft.Extensions.Logging;
using System.Globalization;
using TheatricalPlayersRefactoringKata.Domain.Enums;
using TheatricalPlayersRefactoringKata.Domain.Repositories;

namespace TheatricalPlayersRefactoringKata.Application.UseCases.Invoice.GetByStatement
{
    public class CommandHandler : IRequestHandler<Command, string>
    {
        private readonly ILogger<CommandHandler> _logger;
        private readonly IInvoiceRepository _repository;
        public CommandHandler(ILogger<CommandHandler> logger,
            IInvoiceRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<string> Handle(Command request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(Handle)}");
            var invoice = await _repository.GetByIdThenIncludeAsync(request.id);
            if (invoice is null)
                throw new KeyNotFoundException("Invoice not found");

            var totalAmount = 0;
            var volumeCredits = 0;
            var cultureInfo = new CultureInfo("en-US");
            var result = $"Statement for {invoice.Customer}\n";

            foreach (var perf in invoice.Performances)
            {
                var thisAmount = await CalculateAmount(perf.Play, perf.Audience);
                volumeCredits += await CalculateVolumeCredits(perf.Play.Type, perf.Audience);

                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n",
                perf.Play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
                totalAmount += thisAmount;
            }

            result += string.Format(cultureInfo, $"Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += $"You earned {volumeCredits} credits\n";
            return result;
        }
        public Task<int> CalculateAmount(Domain.Entities.Play play, int audience)
        {
            int lines = play.Lines;
            if (lines < 1000) lines = 1000;
            if (lines > 4000) lines = 4000;
            int thisAmount = (lines / 10) * 100;

            switch (play.Type)
            {
                case TypesPlays.Tragedy:
                    if (audience > 30)
                        thisAmount += (audience - 30) * 1000;
                    break;

                case TypesPlays.Comedy:
                    if (audience > 20)
                        thisAmount += 10000 + (audience - 20) * 500;
                    thisAmount += 300 * audience;
                    break;

                case TypesPlays.History:
                    int tragedyAmount = (lines / 10) * 100;
                    if (audience > 30)
                        tragedyAmount += (audience - 30) * 1000;

                    int comedyAmount = (lines / 10) * 100;
                    if (audience > 20)
                        comedyAmount += 10000 + (audience - 20) * 500;

                    comedyAmount += 300 * audience;
                    thisAmount = tragedyAmount + comedyAmount;
                    break;

                default:
                    throw new Exception($"unknown type: {play.Type}");
            }
            return Task.FromResult(thisAmount);
        }

        public Task<int> CalculateVolumeCredits(TypesPlays typesPlays, int audience)
        {
            int volumeCredits = Math.Max(audience - 30, 0);
            if (typesPlays == TypesPlays.Comedy)
                volumeCredits += (int)Math.Floor((decimal)audience / 5);

            if (typesPlays == TypesPlays.Tragedy)
                volumeCredits += 0;

            if (typesPlays == TypesPlays.History)
            {
                volumeCredits += Math.Max(audience - 30, 0);
                volumeCredits += (int)Math.Floor((decimal)audience / 5);
            }
            return Task.FromResult(volumeCredits);
        }
    }
}