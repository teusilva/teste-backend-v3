using Microsoft.EntityFrameworkCore;
using TheatricalPlayersRefactoringKata.Domain.Entities;
using TheatricalPlayersRefactoringKata.Domain.Repositories;
using TheatricalPlayersRefactoringKata.Infrastructure.DbContexts;

namespace TheatricalPlayersRefactoringKata.Infrastructure.Repositories
{
    public class InvoiceRepository : RepositoryBase<Domain.Entities.Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Invoice> GetByIdThenIncludeAsync(Guid id)
        {
            return Query()
                .Include(x => x.Performances)
                .ThenInclude(x => x.Play)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
