using Microsoft.EntityFrameworkCore;

namespace Mongo.Interfaces;

public interface IDataBaseTransaction<out T> 
    where T : DbContext
{
    Task PerformTransactionAsync(Func<T, Task> contextAction, CancellationToken cancellationToken = default);
    Task<TData> PerformTransactionAsync<TData>(Func<T, Task<TData>> contextAction, CancellationToken cancellationToken = default);
}