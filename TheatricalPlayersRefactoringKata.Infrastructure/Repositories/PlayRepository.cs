using TheatricalPlayersRefactoringKata.Domain.Repositories;
using TheatricalPlayersRefactoringKata.Infrastructure.DbContexts;

namespace TheatricalPlayersRefactoringKata.Infrastructure.Repositories
{
    public class PlayRepository : RepositoryBase<Domain.Entities.Play>, IPlayRepository
    {
        public PlayRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
