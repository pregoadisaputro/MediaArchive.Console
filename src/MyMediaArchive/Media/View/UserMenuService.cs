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

        var existingItem = _mediaService.GetAll();

        var isItemExist = existingItem.FirstOrDefault(i =>
            i.Title.Equals(title, StringComparison.OrdinalIgnoreCase) && i.Year == year
        );

        if (isItemExist != null)
        {
            AnsiConsole.WriteLine($"Media with Title: {title} | Year {year} already exist.");
            return;
        }

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
            _mediaService.CreateMedia(newMedia);
            AnsiConsole.WriteLine("Added!");
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

        foreach (var choice in userPrompt)
        {
            var isItemExist = existingItem.FirstOrDefault(i => $"{i.Title} | {i.Year}" == choice);

            if (isItemExist != null)
            {
                if (AnsiConsole.Confirm("Are you sure want to Delete?"))
                {
                    _mediaService.DeleteMedia(isItemExist);
                    AnsiConsole.MarkupLine("Removed!");
                }
                else
                {
                    AnsiConsole.MarkupLine("Cancelled!");
                }
            }
        }
    }
}
