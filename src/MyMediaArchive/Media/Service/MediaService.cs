using MyMediaArchive.Data.Entity;
using MyMediaArchive.Media.Shared;

namespace MyMediaArchive.Media.Service;

public sealed class MediaService
{
    private readonly string _filePath = FilePath.filePath;
    private readonly List<MediaItem> mediaItems;

    public MediaService()
    {
        mediaItems = JsonService.LoadData<List<MediaItem>>(_filePath) ?? [];
    }

    public IReadOnlyList<MediaItem> GetAll()
    {
        return mediaItems
            .OrderByDescending(i => i.CreatedAt)
            .Select(i => new MediaItem
            {
                Title = i.Title,
                Rating = i.Rating,
                Year = i.Year,
                Type = i.Type,
                Status = i.Status,
                CreatedAt = i.CreatedAt,
                UpdatedAt = i.UpdatedAt,
            })
            .ToList()
            .AsReadOnly();
    }

    public void CreateMedia(MediaItem item)
    {
        if (item == null)
            return;

        mediaItems.Add(item);
        Save();
    }

    public bool DeleteMedia(MediaItem item)
    {
        if (item == null)
            return false;

        var existingItem = mediaItems.FirstOrDefault(i =>
            i.Title.Equals(item.Title, StringComparison.OrdinalIgnoreCase) && i.Year == item.Year
        );

        if (existingItem == null)
            return false;

        mediaItems.Remove(existingItem);
        Save();

        return true;
    }

    public bool UpdateRatingMedia(MediaItem item)
    {
        if (item == null)
            return false;

        var existingItem = mediaItems.FirstOrDefault(i =>
            i.Title.Equals(item.Title, StringComparison.OrdinalIgnoreCase) && i.Year == item.Year
        );

        if (existingItem == null)
            return false;

        existingItem.Rating = item.Rating;
        existingItem.UpdatedAt = DateTime.UtcNow;
        Save();

        return true;
    }

    public bool UpdateStatusMedia(MediaItem item)
    {
        if (item == null)
            return false;

        var existingItem = mediaItems.FirstOrDefault(i =>
            i.Title.Equals(item.Title, StringComparison.OrdinalIgnoreCase) && i.Year == item.Year
        );

        if (existingItem == null)
            return false;

        existingItem.Status = item.Status;
        existingItem.UpdatedAt = DateTime.UtcNow;
        Save();

        return true;
    }

    private void Save()
    {
        JsonService.SaveData(mediaItems, _filePath);
    }
}
