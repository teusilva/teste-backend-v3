using TheatricalPlayersRefactoringKata.Domain.Entities;

namespace TheatricalPlayersRefactoringKata.Domain.Repositories
{
    public interface IInvoiceRepository : IRepositoryBase<Invoice>
    {
        public Task<Invoice> GetByIdThenIncludeAsync(Guid id);
    }
}
