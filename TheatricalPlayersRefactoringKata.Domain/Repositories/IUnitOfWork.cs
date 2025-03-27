namespace TheatricalPlayersRefactoringKata.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync(CancellationToken cancellationToken);
        void Dispose();
    }
}
