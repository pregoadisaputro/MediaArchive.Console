using MyMediaArchive.Data.Entity;
using MyMediaArchive.Data.Enum;
using Spectre.Console;

namespace MyMediaArchive.Media.Service;

public sealed class UserMenuService
{
    private readonly MediaService _mediaService;

    public UserMenuService(MediaService mediaService) => _mediaService = mediaService;

    public void HandleCreateMedia()
    {
        AnsiConsole.Clear();

        var existingMedia = _mediaService.GetAll();

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

        var isItemExist = existingMedia.FirstOrDefault(i =>
            i.Title.ToLower() == title.ToLower() && i.Year == year
        );

        if (isItemExist != null)
        {
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
}
