using MyMediaArchive.Data.Entity;
using Spectre.Console;

namespace MyMediaArchive.Media.Components;

public static class RenderTable
{
    public static void Table(IReadOnlyList<MediaItem> data, string title)
    {
        if (data == null || !data.Any())
        {
            return;
        }

        var table = new Table().Title($"{title}").RoundedBorder().Expand();

        table.AddColumn("Title");
        table.AddColumn("Rating");
        table.AddColumn("Year");
        table.AddColumn("Type");
        table.AddColumn("Status");
        table.AddColumn("Created At");
        table.AddColumn("Updated At");

        foreach (var item in data)
        {
            table.AddRow(
                $"{item.Title}",
                $"{item.Rating}",
                $"{item.Year}",
                $"{item.Type}",
                $"{item.Status}",
                $"{item.CreatedAt:yyyy-MM-dd HH:mm}",
                $"{item.UpdatedAt:yyyy-MM-dd HH:mm}"
            );
        }

        AnsiConsole.Write(table);
    }
}
