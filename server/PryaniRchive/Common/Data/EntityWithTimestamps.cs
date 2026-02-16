namespace Common.Data;

public interface IEntityWithTimestamps
{
    public DateTime CreatedAt { get; set; } 
    
    public DateTime UpdatedAt { get; set; }
}
