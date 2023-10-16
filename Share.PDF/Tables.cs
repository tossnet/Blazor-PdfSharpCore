namespace Share.PDF;

using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

using CommonModels;
using MigraDocCore.DocumentObjectModel.Tables;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.Rendering;
using MigraDocCore.DocumentObjectModel.Shapes;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using PdfSharpCore.Utils;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System;

public static class Tables
{
    private static PdfDocument? document;
    private static WeatherForecast[] _forecasts;
    private static byte[] _imageArray;
    private static string _imagefile;

    public static byte[] PDFTable(WeatherForecast[] forecasts, string imagefile)
    {
        _forecasts = forecasts;
        _imagefile = imagefile;

        return CreatePDF();
    }

    public static byte[] PDFTable(WeatherForecast[] forecasts, byte[] imageArray)
    {
        _forecasts = forecasts;
        _imageArray = imageArray;

        return CreatePDF();
    }

    private static byte[] CreatePDF()
    {
        // Create Document with info
        document = new();
        Common.DocumentInfo(document, "Table");


        // Set font encoding to unicode
        XPdfFontOptions options = new(PdfFontEncoding.Unicode);

        // Create new page
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new("OpenSans-Regular", 20, XFontStyle.Regular, options);

        // You always need a MigraDocCore document for rendering.
        MigraDocCore.DocumentObjectModel.Document doc = new();
        // Each MigraDocCore document needs at least one section.
        Section sec = doc.AddSection();

        DefineStyles(doc);


        Table table = CreateTable(doc, _forecasts);

        // Create a renderer and prepare (=layout) the document
        MigraDocCore.Rendering.DocumentRenderer docRenderer = new(doc);
        docRenderer.PrepareDocument();

        // Render the paragraph. You can render tables or shapes the same way.
        docRenderer.RenderObject(gfx, XUnit.FromCentimeter(1), XUnit.FromCentimeter(5), "12cm", table);


        MemoryStream PdfStream = new();
        document.Save(PdfStream);

        return PdfStream.ToArray();
    }


    public static byte[] PDFAdvancedTable()
    {
        // Create Document with info
        document = new PdfDocument();
        Common.DocumentInfo(document, "Advanced Table");

        // Set font encoding to unicode
        XPdfFontOptions options = new(PdfFontEncoding.Unicode);

        // Create new page
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new("Arial", 20, XFontStyle.Regular, options);

        // You always need a MigraDocCore document for rendering.
        MigraDocCore.DocumentObjectModel.Document doc = new();
        // Each MigraDocCore document needs at least one section.
        Section sec = doc.AddSection();

        DefineStyles(doc);


        Table table = CreateAdvancedTable(doc);

        // Create a renderer and prepare (=layout) the document
        MigraDocCore.Rendering.DocumentRenderer docRenderer = new(doc);
        docRenderer.PrepareDocument();

        // Render the paragraph. You can render tables or shapes the same way.
        docRenderer.RenderObject(gfx, XUnit.FromCentimeter(1), XUnit.FromCentimeter(5), "12cm", table);


        MemoryStream PdfStream = new();
        document.Save(PdfStream);

        return PdfStream.ToArray();
    }

    

    private static void DefineStyles(Document doc)
    {
        // Get the predefined style Normal.
        Style style = doc.Styles["Normal"];
        // Because all styles are derived from Normal, the next line changes the
        // font of the whole document. Or, more exactly, it changes the font of
        // all styles and paragraphs that do not redefine the font.
        style.Font.Name = "Arial";

        style = doc.Styles[StyleNames.Header];
        style.Font.Name = "OpenSans-Regular";
        style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

        style = doc.Styles[StyleNames.Footer];
        style.Font.Name = "OpenSans-Regular";
        style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

        // Create a new style called Table based on style Normal
        style = doc.Styles.AddStyle("Table", "Normal");
        style.Font.Name = "OpenSans-Regular";
        style.Font.Size = 9;

        // Create a new style called Reference based on style Normal
        style = doc.Styles.AddStyle("Reference", "Normal");
        style.ParagraphFormat.SpaceBefore = "5mm";
        style.ParagraphFormat.SpaceAfter = "5mm";

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
        row.Shading.Color = Colors.MediumPurple;
        Cell cell = row.Cells[0];
        cell.AddParagraph("Date");
        cell = row.Cells[1];
        cell.AddParagraph("Temps (C)");
        cell = row.Cells[2];
        cell.AddParagraph("Temps (F)");
        cell = row.Cells[3];
        cell.AddParagraph("Summary");


        foreach (var forecast in forecasts)
        {
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

        // Play with AddImage()
        ImageSource.ImageSourceImpl ??= new ImageSharpImageSource<Rgba32>();
        ImageSource.IImageSource image;

        if (_imageArray is null)
        {
            image = ImageSource.FromFile(_imagefile);
        }
        else
        {
            var _path = "*" + Guid.NewGuid().ToString("B");
            image = ImageSource.FromBinary(_path, () => _imageArray);
        }


        row = table.AddRow();
        cell = row.Cells[0];
        var p = cell.AddParagraph("");
        p.Format.SpaceBefore = -1;
        //p.Format.LeftIndent = 0;
        //p.Format.FirstLineIndent = 0;
        var imageAdded = p.AddImage(image);
        //imageAdded.LockAspectRatio = true;
        //p.Format.SpaceBefore = -20;
        //p.Format.RightIndent = "0.5cm";
        imageAdded.Width = table.Columns[0].Width;
        //imageAdded.WrapFormat.Style = WrapStyle.Through;
        imageAdded.FillFormat.Color = Colors.Aquamarine;
        
        //imageAdded.PictureFormat.CropBottom = "0.5cm";
        //imageAdded.LineFormat.DashStyle = DashStyle.DashDot;
        //imageAdded.LineFormat.Color = Colors.Navy;
        //imageAdded.LineFormat.Width = 1;
        //imageaded.FillFormat = Shape
        //imageAdded.RelativeVertical = RelativeVertical.Line;
        //imageAdded.RelativeHorizontal = RelativeHorizontal.Margin;
        var pp = cell.AddParagraph("A background!");
        pp.Format.SpaceBefore = "-0.5cm";
        pp.Format.Shading.Color = Colors.Transparent;


        table.SetEdge(0, 0, 4, 1, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

        
        document.LastSection.Add(table);

        return table;
    }

    private static Table CreateAdvancedTable(Document document)
    {
        document.LastSection.AddParagraph("Advanced Table", "Heading2");

        Table table = new();
        table.Borders.Width = 0.75;

        Column column = table.AddColumn(Unit.FromCentimeter(3));
        column.Format.Alignment = ParagraphAlignment.Center;

        table.AddColumn(Unit.FromCentimeter(2));
        table.AddColumn(Unit.FromCentimeter(2));
        table.AddColumn(Unit.FromCentimeter(3));
        table.AddColumn(Unit.FromCentimeter(4));
        table.AddColumn(Unit.FromCentimeter(5));

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



        // Create the header of the table
        row = table.AddRow();
        row.HeadingFormat = true;
        row.Format.Alignment = ParagraphAlignment.Center;
        row.Format.Font.Bold = true;
        row.Shading.Color = MigraDocCore.DocumentObjectModel.Colors.Green;
        row.Cells[0].AddParagraph("Item");
        row.Cells[0].Format.Font.Bold = false;
        row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
        row.Cells[0].VerticalAlignment = MigraDocCore.DocumentObjectModel.Tables.VerticalAlignment.Bottom;
        row.Cells[0].MergeDown = 1;
        row.Cells[1].AddParagraph("Title and Author");
        row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
        row.Cells[1].MergeRight = 3;
        row.Cells[5].AddParagraph("Extended Price");
        row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
        row.Cells[5].VerticalAlignment = MigraDocCore.DocumentObjectModel.Tables.VerticalAlignment.Bottom;
        row.Cells[5].MergeDown = 1;

        row = table.AddRow();
        row.HeadingFormat = true;
        row.Format.Alignment = ParagraphAlignment.Center;
        row.Shading.Color = MigraDocCore.DocumentObjectModel.Colors.Orange;
        row.Cells[1].AddParagraph("Quantity");
        row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
        row.Cells[2].AddParagraph("Unit Price");
        row.Cells[2].Format.Font.Bold = true;
        row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
        row.Cells[3].AddParagraph("Discount (%)");
        row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
        row.Cells[3].Shading.Color = MigraDocCore.DocumentObjectModel.Colors.Cyan;
        row.Cells[4].AddParagraph("Taxable");
        row.Cells[4].Format.Alignment = ParagraphAlignment.Left;
        row.Cells[4].Shading.Color = MigraDocCore.DocumentObjectModel.Colors.Transparent;
        row.Cells[4].Format.Font.Color = MigraDocCore.DocumentObjectModel.Colors.Red;

        table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);


        document.LastSection.Add(table);

        return table;
    }


   

}
