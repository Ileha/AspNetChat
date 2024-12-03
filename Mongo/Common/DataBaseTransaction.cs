using System.Threading.Tasks.Schedulers;
using Microsoft.EntityFrameworkCore;
using Mongo.Interfaces;

namespace Mongo.Common;

public class DataBaseTransaction<T> : IDataBaseTransaction<T>
    where T : DbContext
{
    private readonly T _context;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly TaskScheduler _taskScheduler;
    
    public DataBaseTransaction(T context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _taskScheduler = new StaTaskScheduler(1);
    }

    public async Task PerformTransactionAsync(Func<T, Task> contextAction, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            await await Task.Factory.StartNew(
                () => contextAction(_context), 
                cancellationToken,
                TaskCreationOptions.DenyChildAttach, 
                _taskScheduler);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<TData> PerformTransactionAsync<TData>(Func<T, Task<TData>> contextAction, CancellationToken cancellationToken = default)
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var resultTask = await Task<Task<TData>>.Factory.StartNew(() => contextAction(_context), cancellationToken, TaskCreationOptions.DenyChildAttach, _taskScheduler);

            return await resultTask;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}