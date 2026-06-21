using MyMediaArchive.Media.View.Service;
using Spectre.Console;

namespace MyMediaArchive.Media.View.UI;

public sealed class MainMenu
{
    private readonly HandlingUserMenuService _userMenuService;
    private readonly MenuQueryService _menuQueryService;

    public MainMenu(HandlingUserMenuService userMenuService, MenuQueryService menuQueryService)
    {
        _userMenuService = userMenuService;
        _menuQueryService = menuQueryService;
    }

    public void Menu()
    {
        var isRunning = true;

        while (isRunning)
        {
            AnsiConsole.Clear();

            var header = new FigletText("My Media Archive");
            AnsiConsole.Write(header);

            var userChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose:")
                    .AddChoiceGroup(
                        "Media Management",
                        new[] { "Add Media", "Delete Media", "Update Rating", "Update Status" }
                    )
                    .AddChoiceGroup(
                        "See Media",
                        new[] { "See All Media", "Find", "See By Type", "See By Status" }
                    )
                    .AddChoices("Exit")
            );

            switch (userChoice)
            {
                case "Add Media":
                    _userMenuService.HandleCreateMedia();
                    break;

                case "Delete Media":
                    _userMenuService.HandleDeleteMedia();
                    break;

                case "Update Rating":
                    _userMenuService.HandleUpdateRatingMedia();
                    break;

                case "Update Status":
                    _userMenuService.HandleUpdateStatusMedia();
                    break;

                case "See All Media":
                    _menuQueryService.SeeAllMedia();
                    break;

                case "Find":
                    _menuQueryService.FindMedia();
                    break;

                case "See By Type":
                    _menuQueryService.SeeMediaByType();
                    break;

                case "See By Status":
                    _menuQueryService.SeeMediaByStatus();
                    break;

                case "Exit":
                    AnsiConsole.MarkupLine("See ya!");
                    AnsiConsole.Clear();
                    isRunning = false;
                    break;
            }

            if (isRunning)
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("Press enter to continue..");
                Console.ReadLine();
            }
        }
    }
}
