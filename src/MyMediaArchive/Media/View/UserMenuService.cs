using MyMediaArchive.Data.Entity;
using MyMediaArchive.Data.Enum;
using MyMediaArchive.Media.Components;
using MyMediaArchive.Media.Service;
using Spectre.Console;

namespace MyMediaArchive.Media.View;

public sealed class UserMenuService
{
    private readonly MediaService _mediaService;

    public UserMenuService(MediaService mediaService) => _mediaService = mediaService;

    public void HandleCreateMedia()
    {
        AnsiConsole.Clear();

        var title = AnsiConsole.Ask<string>("Title:");
        var rating = AnsiConsole.Ask<double>("Rating:");
        var year = AnsiConsole.Ask<string>("Year:");
        var type = AnsiConsole.Prompt(
            new SelectionPrompt<MediaType>().Title("Type:").AddChoices(Enum.GetValues<MediaType>())
        );
        var status = AnsiConsole.Prompt(
            new SelectionPrompt<MediStatus>()
                .Title("Status:")
                .AddChoices(Enum.GetValues<MediStatus>())
        );

        var newMedia = new MediaItem
        {
            Title = title,
            Rating = rating,
            Year = year,
            Type = type,
            Status = status,
        };

        AnsiConsole.WriteLine();
        var summary = new Panel(
            new Rows(
                new Markup($"Title: {newMedia.Title}"),
                new Markup($"Rating: {newMedia.Rating}"),
                new Markup($"Year: {newMedia.Year}"),
                new Markup($"Type: {newMedia.Type}"),
                new Markup($"Status: {newMedia.Status}")
            )
        )
            .Header("Summary")
            .Border(BoxBorder.Rounded);
        AnsiConsole.Write(summary);
        AnsiConsole.WriteLine();

        if (AnsiConsole.Confirm("Add?"))
        {
            var isAdded = _mediaService.CreateMedia(newMedia);

            if (isAdded)
            {
                AnsiConsole.MarkupLine("Added!");
            }
            else
            {
                AnsiConsole.MarkupLine($"Media with Title: {title} | Year {year} already exist.");
            }
        }
        else
        {
            AnsiConsole.WriteLine("Cancelled.");
        }
    }

    public void HandleDeleteMedia()
    {
        AnsiConsole.Clear();

        var existingItem = _mediaService.GetAll();

        if (existingItem == null || !existingItem.Any())
        {
            AnsiConsole.WriteLine("Media does not exist yet.");
            return;
        }

        RenderTable.Table(existingItem, "Media to Delete:");

        AnsiConsole.WriteLine();

        var userPrompt = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("Select Media to Delete:")
                .AddChoices(existingItem.Select(i => $"{i.Title} | {i.Year}"))
        );

        if (userPrompt.Count == 0)
        {
            AnsiConsole.WriteLine("Cancelled!");
            return;
        }

        if (AnsiConsole.Confirm($"Are you sure want to delete these {userPrompt.Count} items?"))
        {
            foreach (var choice in userPrompt)
            {
                var removedItem = existingItem.FirstOrDefault(i =>
                    $"{i.Title} | {i.Year}" == choice
                );

                if (removedItem != null)
                {
                    _mediaService.DeleteMedia(removedItem);
                    AnsiConsole.MarkupLine($"Removed item: {removedItem.Title}");
                }
            }
        }
    }

    public void HandleUpdateRatingMedia()
    {
        AnsiConsole.Clear();

        var existingItem = _mediaService.GetAll();

        if (existingItem == null || !existingItem.Any())
        {
            AnsiConsole.MarkupLine("Media does not exist yet.");
            return;
        }

        RenderTable.Table(existingItem, "Update Media Rating:");

        AnsiConsole.WriteLine();

        var userPrompt = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Rating to Update:")
                .AddChoices(existingItem.Select(i => $"{i.Title} | {i.Year}"))
        );

        var isItemExist = existingItem.FirstOrDefault(i => $"{i.Title} | {i.Year}" == userPrompt);

        if (isItemExist != null)
        {
            AnsiConsole.WriteLine();

            var rating = AnsiConsole.Ask<double>("Enter new Rating: (e.g 1.0 - 10)");

            if (AnsiConsole.Confirm("Sure want to update?"))
            {
                var ratingUpdated = new MediaItem
                {
                    Title = isItemExist.Title,
                    Rating = rating,
                    Year = isItemExist.Year,
                };

                _mediaService.UpdateRatingMedia(ratingUpdated);
                AnsiConsole.MarkupLine($"Rating Updated for {isItemExist.Title}!");
            }
            else
            {
                AnsiConsole.MarkupLine("Cancelled!");
            }
        }
    }
}
