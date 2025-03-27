using TheatricalPlayersRefactoringKata.Domain.Repositories;
using TheatricalPlayersRefactoringKata.Infrastructure.DbContexts;

namespace TheatricalPlayersRefactoringKata.Infrastructure.Repositories
{
    public class PerformanceRepository : RepositoryBase<Domain.Entities.Performance>, IPerformanceRepository
    {
        public PerformanceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
