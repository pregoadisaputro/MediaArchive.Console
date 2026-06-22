using MyMediaArchive.Data.Entity;
using MyMediaArchive.Data.Enum;
using MyMediaArchive.Media.Components;
using MyMediaArchive.Media.Service;
using Spectre.Console;

namespace MyMediaArchive.Media.View.Service;

public sealed class MenuQueryService
{
    private readonly MediaService _mediaService;

    public MenuQueryService(MediaService mediaService) => _mediaService = mediaService;

    public IReadOnlyList<MediaItem> SeeAllMedia()
    {
        AnsiConsole.Clear();

        var mediaItem = _mediaService
            .GetAll()
            .OrderByDescending(i => i.CreatedAt)
            .ToList()
            .AsReadOnly();

        if (mediaItem.Count == 0)
        {
            AnsiConsole.MarkupLine("Media does not exist yet.");
        }

        RenderTable.Table(mediaItem, "All Media");

        return mediaItem;
    }

    public IReadOnlyList<MediaItem> FindMedia()
    {
        AnsiConsole.Clear();

        var existingItem = _mediaService.GetAll();

        if (existingItem.Count == 0)
        {
            AnsiConsole.MarkupLine("Media does not exist yet.");
            return [];
        }

        var search = AnsiConsole.Ask<string>("Find:");

        var searchResults = _mediaService
            .GetAll()
            .Where(i => i.Title.Contains(search, StringComparison.OrdinalIgnoreCase))
            .OrderBy(i => i.Title)
            .ToList()
            .AsReadOnly();

        if (searchResults.Count == 0)
        {
            AnsiConsole.MarkupLine("Media does not exist.");
            return [];
        }

        RenderTable.Table(searchResults, $"Found {searchResults.Count} for {search}");

        return searchResults;
    }

    public IReadOnlyList<MediaItem> SeeMediaByType()
    {
        AnsiConsole.Clear();

        var existingItem = _mediaService.GetAll();

        if (existingItem.Count == 0)
        {
            AnsiConsole.MarkupLine("Media does not exist yet");
            return [];
        }

        var userChoice = AnsiConsole.Prompt(
            new SelectionPrompt<MediaType>()
                .Title("Select the Type:")
                .AddChoices(Enum.GetValues<MediaType>())
        );

        var mediaItem = existingItem
            .Where(i => i.Type == userChoice)
            .OrderBy(i => i.Type)
            .ToList()
            .AsReadOnly();

        if (mediaItem.Count == 0)
        {
            AnsiConsole.MarkupLine($"Media with Type {userChoice} does not exist.");
            return mediaItem;
        }

        RenderTable.Table(mediaItem, $"Type: {userChoice}");

        return mediaItem;
    }

    public IReadOnlyList<MediaItem> SeeMediaByStatus()
    {
        AnsiConsole.Clear();

        var existingItem = _mediaService.GetAll();

        if (existingItem.Count == 0)
        {
            AnsiConsole.WriteLine("Media does not exist yet");
            return [];
        }

        var userChoice = AnsiConsole.Prompt(
            new SelectionPrompt<MediStatus>()
                .Title("Select the Status")
                .AddChoices(Enum.GetValues<MediStatus>())
        );

        var mediaItem = existingItem
            .Where(i => i.Status == userChoice)
            .OrderByDescending(i => i.UpdatedAt)
            .ToList()
            .AsReadOnly();

        if (mediaItem.Count == 0)
        {
            AnsiConsole.MarkupLine($"Media with Status {userChoice} does not exist.");
            return mediaItem;
        }

        RenderTable.Table(mediaItem, $"Status: {userChoice}");

        return mediaItem;
    }
}
