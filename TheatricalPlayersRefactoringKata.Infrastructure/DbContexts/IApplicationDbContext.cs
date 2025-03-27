using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data;

namespace TheatricalPlayersRefactoringKata.Infrastructure.DbContexts
{
    public interface IApplicationDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        bool HasChanges { get; }
        IDbConnection Connection { get; }
        EntityEntry Entry(object enity);
    }
}
