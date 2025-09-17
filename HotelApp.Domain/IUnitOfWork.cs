namespace HotelApp.Domain
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;

		Task<int> CommitAsync(CancellationToken cancellationToken = default);
		Task<int> CommitAsync(bool skipAuditFields, CancellationToken cancellationToken = default);

		Task BeginTransactionAsync();
		Task CommitTransactionAsync();
		Task RollbackTransactionAsync();
	}
}
