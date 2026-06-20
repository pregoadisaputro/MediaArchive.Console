using MyMediaArchive.Data.Entity;
using Spectre.Console;

namespace MyMediaArchive.Media.View;

public static class RenderTable
{
    public static void Table(IReadOnlyList<MediaItem> data, string title)
    {
        if (data == null || !data.Any())
        {
            return;
        }

        var table = new Table().Title($"{title}").RoundedBorder().Expand();

        table.AddColumn("Id");
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
                $"{item.Id}",
                $"{item.Title}",
                $"{item.Rating}",
                $"{item.Year}",
                $"{item.Type}",
                $"{item.Status}",
                $"{item.CreatedAt}",
                $"{item.UpdatedAt}"
            );
        }
    }
}
