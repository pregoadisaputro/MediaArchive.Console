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
        return mediaItems.OrderByDescending(i => i.CreatedAt).ToList().AsReadOnly();
    }

    public bool CreateMedia(MediaItem item)
    {
        if (item == null)
            return false;

        var isItemExist = mediaItems.Any(i =>
            i.Title.Equals(item.Title, StringComparison.OrdinalIgnoreCase)
            && i.Year.Equals(item.Year, StringComparison.OrdinalIgnoreCase)
        );

        if (isItemExist)
            return false;

        mediaItems.Add(item);
        Save();

        return true;
    }

    public bool DeleteMedia(MediaItem item)
    {
        if (item == null)
            return false;

        var existingItem = FindItem(item.Title, item.Year);

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

        var existingItem = FindItem(item.Title, item.Year);

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

        var existingItem = FindItem(item.Title, item.Year);

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

    private MediaItem? FindItem(string title, string year)
    {
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(year))
        {
            return null;
        }

        return mediaItems.FirstOrDefault(i =>
            i.Title.Equals(title, StringComparison.OrdinalIgnoreCase)
            && i.Year.Equals(year, StringComparison.OrdinalIgnoreCase)
        );
    }
}
