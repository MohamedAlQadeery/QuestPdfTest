using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SkiaSharp;
using Microcharts;
using QuestPDF.Previewer;
using QuestPdfTest;
using System.Drawing;

QuestPDF.Settings.License = LicenseType.Community;
var document = Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(x => x.FontSize(10));

        page.Header().Text("Team Rating").FontSize(20).Bold().FontColor(Colors.Blue.Medium);

        page.Content().Column(column =>
        {
            column.Spacing(20);

            AddChartRow(column, "Pressure", "Territory", "Fight");
            AddChartRow(column, "Role/Game Impact","","");
            AddPlayerTable(column);
        });
    });
});

void AddChartRow(ColumnDescriptor column, params string[] chartTitles)
{
    column.Item().Row(row =>
    {
        foreach (var title in chartTitles)
        {
            row.RelativeItem().Column(col =>
            {
                col.Spacing(5);
                col.Item().Text(title).FontSize(14).Bold();
                col.Item().Height(300).SkiaSharpRasterized((canvas, size) =>
                {
                    var chart = new BarChart
                    {
                        Entries = GetChartEntries(title),
                        LabelOrientation = Orientation.Vertical, // Use vertical labels on the left
                        ValueLabelOrientation = Orientation.Horizontal, // Values on the right
                        IsAnimated = false,
                        Margin = 10,
                        BarAreaAlpha = 255,
                        BackgroundColor = SKColors.Transparent,
                        MaxValue = 40,  // Set this value based on your data
                        MinValue = 0,
                        LabelTextSize = 10,
                    };

                    chart.DrawContent(canvas, (int)size.Width, (int)size.Height);
                });
            });
        }
    });
}

ChartEntry[] GetChartEntries(string chartType)
{
    var colors = new[] { "#2c3e50", "#77d065", "#f1c40f", "#e67e22", "#e74c3c" };
    switch (chartType)
    {
        case "Pressure":
            return new[]
            {
                new ChartEntry(4) { Label = "Elite", ValueLabel = "4", Color = SKColor.Parse(colors[0]) },
                new ChartEntry(19) { Label = "Above Average", ValueLabel = "19", Color = SKColor.Parse(colors[1]) },
                new ChartEntry(30) { Label = "Average", ValueLabel = "30", Color = SKColor.Parse(colors[2]) },
                new ChartEntry(31) { Label = "Capable", ValueLabel = "31", Color = SKColor.Parse(colors[3]) },
                new ChartEntry(8) { Label = "Poor", ValueLabel = "8", Color = SKColor.Parse(colors[4]) }
            };
        case "Territory":
            return new[]
            {
                new ChartEntry(8) { Label = "Elite", ValueLabel = "8", Color = SKColor.Parse(colors[0]) },
                new ChartEntry(20) { Label = "Above Average", ValueLabel = "20", Color = SKColor.Parse(colors[1]) },
                new ChartEntry(26) { Label = "Average", ValueLabel = "26", Color = SKColor.Parse(colors[2]) },
                new ChartEntry(30) { Label = "Capable", ValueLabel = "30", Color = SKColor.Parse(colors[3]) },
                new ChartEntry(8) { Label = "Poor", ValueLabel = "8", Color = SKColor.Parse(colors[4]) }
            };
        case "Fight":
            return new[]
            {
                new ChartEntry(3) { Label = "Elite", ValueLabel = "3", Color = SKColor.Parse(colors[0]) },
                new ChartEntry(28) { Label = "Above Average", ValueLabel = "28", Color = SKColor.Parse(colors[1]) },
                new ChartEntry(22) { Label = "Average", ValueLabel = "22", Color = SKColor.Parse(colors[2]) },
                new ChartEntry(28) { Label = "Capable", ValueLabel = "28", Color = SKColor.Parse(colors[3]) },
                new ChartEntry(10) { Label = "Poor", ValueLabel = "10", Color = SKColor.Parse(colors[4]) }
            };
        case "Role/Game Impact":
            return new[]
            {
                new ChartEntry(7) { Label = "Elite", ValueLabel = "7", Color = SKColor.Parse(colors[0]) },
                new ChartEntry(26) { Label = "Above Average", ValueLabel = "26", Color = SKColor.Parse(colors[1]) },
                new ChartEntry(18) { Label = "Average", ValueLabel = "18", Color = SKColor.Parse(colors[2]) },
                new ChartEntry(30) { Label = "Capable", ValueLabel = "30", Color = SKColor.Parse(colors[3]) },
                new ChartEntry(10) { Label = "Poor", ValueLabel = "10", Color = SKColor.Parse(colors[4]) }
            };
        default:
            return Array.Empty<ChartEntry>();
    }
}

void AddPlayerTable(ColumnDescriptor column)
{
    column.Item().Table(table =>
    {
        table.ColumnsDefinition(columns =>
        {
            columns.ConstantColumn(100);
            columns.RelativeColumn();
            columns.RelativeColumn();
            columns.RelativeColumn();
            columns.RelativeColumn();
            columns.RelativeColumn();
        });

        static IContainer CellStyle(IContainer container) => container
            .Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(5)
            .AlignMiddle()
            .AlignCenter();

        table.Header(header =>
        {
            header.Cell().Element(CellStyle).Text("Player").Bold();
            header.Cell().Element(CellStyle).Text("Pressure").Bold();
            header.Cell().Element(CellStyle).Text("Territory").Bold();
            header.Cell().Element(CellStyle).Text("Fight").Bold();
            header.Cell().Element(CellStyle).Text("Role/Game Impact").Bold();
            header.Cell().Element(CellStyle).Text("B & F").Bold();
        });

        var players = new[]
        {
            new { Name = "P.Lipinski", Pressure = 2.0f, Territory = 1.5f, Fight = 1.5f, RoleGameImpact = 1.5f, BF = 1.6f },
            new { Name = "I.Quaynor", Pressure = 2.2f, Territory = 1.8f, Fight = 2.0f, RoleGameImpact = 1.8f, BF = 2.0f },
            new { Name = "B.Maynard", Pressure = 1.5f, Territory = 1.0f, Fight = 1.5f, RoleGameImpact = 1.0f, BF = 1.3f },
            new { Name = "J.Elliott", Pressure = 2.0f, Territory = 1.0f, Fight = 1.0f, RoleGameImpact = 1.0f, BF = 1.3f },
            new { Name = "J.Daicos", Pressure = 1.0f, Territory = 1.8f, Fight = 1.5f, RoleGameImpact = 1.8f, BF = 1.5f },
            new { Name = "L.Schultz", Pressure = 2.8f, Territory = 3.2f, Fight = 3.2f, RoleGameImpact = 3.2f, BF = 3.1f },
            new { Name = "J.Noble", Pressure = 1.2f, Territory = 3.0f, Fight = 2.5f, RoleGameImpact = 2.5f, BF = 2.3f },
            new { Name = "S.Pendlebury", Pressure = 3.0f, Territory = 2.0f, Fight = 2.5f, RoleGameImpact = 2.2f, BF = 2.4f },
        };

        foreach (var player in players)
        {
            table.Cell().Element(CellStyle).Text(player.Name);
            table.Cell().Element(CellStyle).Text(player.Pressure.ToString("0.0"));
            table.Cell().Element(CellStyle).Text(player.Territory.ToString("0.0"));
            table.Cell().Element(CellStyle).Text(player.Fight.ToString("0.0"));
            table.Cell().Element(CellStyle).Text(player.RoleGameImpact.ToString("0.0"));
            table.Cell().Element(CellStyle).Text(player.BF.ToString("0.0"));
        }
    });
}

//document.ShowInPreviewer();
document.GeneratePdf("TeamsRating.pdf");