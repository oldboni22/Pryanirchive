using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Common.Data;

public sealed class TimeStampInterceptor : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        SetTimeStamps(eventData);
        
        return result;
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        SetTimeStamps(eventData);
        
        return new ValueTask<InterceptionResult<int>>(result);
    }

    private static void SetTimeStamps(DbContextEventData eventData)
    {
        var entities = eventData!.Context!.ChangeTracker
            .Entries()
            .Where(entry => entry is
            {
                Entity: IEntityWithTimestamps,
                State: EntityState.Added or EntityState.Modified
            });

        foreach (var entry in entities)
        {
            var entity = (IEntityWithTimestamps)entry.Entity;
            
            entity.UpdatedAt = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }
}
