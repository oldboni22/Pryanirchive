namespace Common.Data;

public abstract class EntityWithTimestamps : EntityBase
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
