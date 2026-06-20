using MyMediaArchive.Data.Enum;

namespace MyMediaArchive.Data.Entity;

public class MediaItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Year { get; set; } = string.Empty;
    public MediaType Type { get; set; }
    public MediStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
