using MyMediaArchive.Data.Enum;

namespace MyMediaArchive.Data.Entity;

public class MediaItem
{
    public int Id { get; set; }
    public required string Title { get; set; } = string.Empty;
    public double Rating { get; set; }
    public required string Year { get; set; } = string.Empty;
    public MediaType Type { get; set; }
    public MediStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
