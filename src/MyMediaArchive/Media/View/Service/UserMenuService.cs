using MyMediaArchive.Data.Entity;
using MyMediaArchive.Data.Enum;
using MyMediaArchive.Media.Components;
using MyMediaArchive.Media.Service;
using Spectre.Console;

namespace MyMediaArchive.Media.View.Service;

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

        var selectedItem = AnsiConsole.Prompt(
            new MultiSelectionPrompt<MediaItem>()
                .Title("Select Media to Delete:")
                .UseConverter(i => $"{i.Title} | {i.Year}")
                .AddChoices(existingItem)
        );

        if (selectedItem.Count == 0)
        {
            AnsiConsole.WriteLine("Cancelled!");
            return;
        }

        if (AnsiConsole.Confirm($"Are you sure want to delete these {selectedItem.Count} items?"))
        {
            foreach (var choice in selectedItem)
            {
                if (choice != null)
                {
                    _mediaService.DeleteMedia(choice);
                    AnsiConsole.MarkupLine($"Removed item: {choice.Title}");
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

        var selectedItem = AnsiConsole.Prompt(
            new SelectionPrompt<MediaItem>()
                .Title("Rating to Update:")
                .UseConverter(i => $"{i.Title} | {i.Year}")
                .AddChoices(existingItem)
        );

        AnsiConsole.WriteLine();

        var rating = AnsiConsole.Ask<double>("Enter new Rating: (e.g 1.0 - 10)");

        if (AnsiConsole.Confirm($"Update the rating for {selectedItem.Title} to {rating}?"))
        {
            var ratingUpdated = new MediaItem
            {
                Title = selectedItem.Title,
                Rating = rating,
                Year = selectedItem.Year,
            };

            _mediaService.UpdateRatingMedia(ratingUpdated);
            AnsiConsole.MarkupLine($"Rating Updated for {selectedItem.Title} | {rating}");
        }
        else
        {
            AnsiConsole.MarkupLine("Cancelled!");
        }
    }

    public void HandleUpdateStatusMedia()
    {
        AnsiConsole.Clear();

        var existingItem = _mediaService.GetAll();

        if (existingItem == null || !existingItem.Any())
        {
            AnsiConsole.MarkupLine("Media does not exist yet.");
            return;
        }

        RenderTable.Table(existingItem, "Update Media Status:");

        AnsiConsole.WriteLine();

        var selectedItem = AnsiConsole.Prompt(
            new SelectionPrompt<MediaItem>()
                .Title("Select:")
                .UseConverter(i => $"{i.Title} | {i.Year} | {i.Status}")
                .AddChoices(existingItem)
        );

        AnsiConsole.WriteLine();

        var selectedStatus = AnsiConsole.Prompt(
            new SelectionPrompt<MediStatus>()
                .Title("Status:")
                .AddChoices(Enum.GetValues<MediStatus>())
        );

        if (
            AnsiConsole.Confirm(
                $"Update the Status for {selectedItem.Title} from {selectedItem.Status} to {selectedStatus}?"
            )
        )
        {
            var statusUpdated = new MediaItem
            {
                Title = selectedItem.Title,
                Year = selectedItem.Year,
                Status = selectedStatus,
            };

            _mediaService.UpdateStatusMedia(statusUpdated);
            AnsiConsole.MarkupLine(
                $"Status Updated for {selectedItem.Title} | {statusUpdated.Status}"
            );
        }
        else
        {
            AnsiConsole.MarkupLine("Cancelled!");
        }
    }
}
