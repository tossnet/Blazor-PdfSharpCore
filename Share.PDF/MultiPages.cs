namespace Share.PDF;

using CommonModels;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.Tables;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Share.PDF.Helpers;
using System;


public class MultiPages
{
    public static byte[] GetRenderer()
    {
        PdfDocument document = new();

        // Sample uses DIN A4, page height is 29.7 cm. We use margins of 2.5 cm.
        LayoutHelper helper = new(document, XUnit.FromCentimeter(2.5), XUnit.FromCentimeter(29.7 - 2.5));
        XUnit left = XUnit.FromCentimeter(2.5);

        // Random generator with seed value, so created document will always be the same.
        Random rand = new(42);

        const int headerFontSize = 20;
        const int normalFontSize = 10;

        XFont fontHeader = new("Verdana", headerFontSize, XFontStyle.BoldItalic);
        XFont fontNormal = new("Verdana", normalFontSize, XFontStyle.Regular);

        const int totalLines = 666;
        bool washeader = false;
        for (int line = 0; line < totalLines; ++line)
        {
            bool isHeader = line == 0 || !washeader && line < totalLines - 1 && rand.Next(15) == 0;
            washeader = isHeader;
            // We do not want a single header at the bottom of the page,
            // so if we have a header we require space for header and a normal text line.
            XUnit top = helper.GetLinePosition(isHeader ? headerFontSize + 5 : normalFontSize + 2, isHeader ? headerFontSize + 5 + normalFontSize : normalFontSize);

            helper.Gfx.DrawString(isHeader ? "Sed massa libero, semper a nisi nec" : "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            isHeader ? fontHeader : fontNormal, XBrushes.Black, left, top, XStringFormats.TopLeft);
        }


        MemoryStream PdfStream = new();
        document.Save(PdfStream);

        return PdfStream.ToArray();
    }

    public static byte[] GetTableRenderer(WeatherForecast[] forecasts)
    {
        PdfDocument document = new();

        // Sample uses DIN A4, page height is 29.7 cm. We use margins of 2.5 cm.
        LayoutHelper helper = new(document, XUnit.FromCentimeter(2.5), XUnit.FromCentimeter(29.7 - 2.5));
        XUnit left = XUnit.FromCentimeter(2.5);


        // You always need a MigraDocCore document for rendering.
        MigraDocCore.DocumentObjectModel.Document doc = new();
        // Each MigraDocCore document needs at least one section.
        Section sec = doc.AddSection();

        Table table = CreateTable(doc, forecasts);

        const int headerFontSize = 20;
        const int normalFontSize = 10;

        XFont fontHeader = new("Verdana", headerFontSize, XFontStyle.BoldItalic);
        XFont fontNormal = new("Verdana", normalFontSize, XFontStyle.Regular);

        const int totalLines = 666;
        bool washeader = false;
        for (int line = 0; line < totalLines; ++line)
        {
            bool isHeader = line == 0 || !washeader && line < totalLines - 1 ;
            washeader = isHeader;
            // We do not want a single header at the bottom of the page,
            // so if we have a header we require space for header and a normal text line.
            XUnit top = helper.GetLinePosition(isHeader ? headerFontSize + 5 : normalFontSize + 2, isHeader ? headerFontSize + 5 + normalFontSize : normalFontSize);

            helper.Gfx.DrawString(isHeader ? "Sed massa libero, semper a nisi nec" : "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            isHeader ? fontHeader : fontNormal, XBrushes.Black, left, top, XStringFormats.TopLeft);
        }

        // Create a renderer and prepare (=layout) the document
        MigraDocCore.Rendering.DocumentRenderer docRenderer = new(doc);
        docRenderer.PrepareDocument();

        // Render the paragraph. You can render tables or shapes the same way.
        docRenderer.RenderObject(helper.Gfx, XUnit.FromCentimeter(1), XUnit.FromCentimeter(5), "12cm", table);

        MemoryStream PdfStream = new();
        document.Save(PdfStream);

        return PdfStream.ToArray();
    }

    private static Table CreateTable(Document document, WeatherForecast[] forecasts)
    {
        document.LastSection.AddParagraph("Simple Table", "Heading2");

        Table table = new();
        table.Borders.Width = 0.75;

        Column column = table.AddColumn(Unit.FromCentimeter(3));
        column.Format.Alignment = ParagraphAlignment.Center;

        table.AddColumn(Unit.FromCentimeter(2));
        table.AddColumn(Unit.FromCentimeter(2));
        table.AddColumn(Unit.FromCentimeter(3));

        Row row = table.AddRow();
        row.Shading.Color = Colors.PaleGoldenrod;
        Cell cell = row.Cells[0];
        cell.AddParagraph("Date");
        cell = row.Cells[1];
        cell.AddParagraph("Temps (C)");
        cell = row.Cells[2];
        cell.AddParagraph("Temps (F)");
        cell = row.Cells[3];
        cell.AddParagraph("Summary");

        int i = 0;
        for (int line = 0; line < 100; ++line)
        {
            var forecast = forecasts[i];
            i = i < forecasts.Length ? i++ : 0;

            row = table.AddRow();
            cell = row.Cells[0];
            cell.AddParagraph(forecast.Date.ToShortDateString());

            cell = row.Cells[1];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.AddParagraph(forecast.TemperatureC.ToString());

            cell = row.Cells[2];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.AddParagraph(forecast.TemperatureF.ToString());

            cell = row.Cells[3];
            cell.AddParagraph(forecast.Summary);
        }


        table.SetEdge(0, 0, 4, 1, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);


        document.LastSection.Add(table);

        return table;
    }
}
