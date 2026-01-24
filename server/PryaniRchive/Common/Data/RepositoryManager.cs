using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Common.Data;

public interface IRepositoryManager
{
    Task SaveAsync(CancellationToken cancellationToken = default);
}

public abstract class RepositoryManager(DbContext context) : IRepositoryManager
{
    protected DbContext Context { get; } = context;
    
    public Task SaveAsync(CancellationToken cancellationToken = default)
    {
        return Context.SaveChangesAsync(cancellationToken);
    }
}
