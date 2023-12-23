namespace Share.PDF;

using MigraDocCore.DocumentObjectModel.Tables;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.Rendering;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;
using MigraDocCore.DocumentObjectModel.Shapes;
using PdfSharpCore.Utils;
using SixLabors.ImageSharp.PixelFormats;

public static class Order
{
    private static Document document;
    private static byte[] _imageArray;
    private static string _imagefile;
    private static string _fontName ;

    public static byte[] Edition(string imagefile)
    {
        // Call from Server
        _imagefile = imagefile;
        
        _fontName = "Arial";

        return CreatePDF();
    }

    public static byte[] Edition(byte[] imageArray)
    {
       // Call from webAssembly
       _imageArray = imageArray;

        // I force the font name because the font is not loaded in the server
        _fontName = "OpenSans-Regular";

        return CreatePDF();
    }

    private static byte[] CreatePDF()
    {
        // New Document
        document = new();
        document.Info.Title = "Order";
        document.Info.Author = "Christophe Peugnet";
        document.Info.Subject = "My order";

        Section section = document.AddSection();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.Orientation = Orientation.Portrait;
        section.PageSetup.TopMargin = "0.4cm";
        section.PageSetup.LeftMargin = "1cm";
        section.PageSetup.RightMargin = "1cm";
        //section.PageSetup.BottomMargin = "0cm";
        section.PageSetup.FooterDistance = "0.8cm";
        section.PageSetup.OddAndEvenPagesHeaderFooter = false;
        section.PageSetup.StartingNumber = 1;

        DefineStyles();
        DefineContentSection();

        RenderHeader();

        RenderReferences();

        RenderAddress();

        RenderOrderNumber();

        RenderContent();

        RenderTotal();

        RenderBankDetails();

        RenderTextBottom();



        PdfDocumentRenderer renderer = new(unicode: true)
        {
            Document = document,
        };

        renderer.RenderDocument();

        MemoryStream PdfStream = new();
        renderer.PdfDocument.Save(PdfStream);

        return PdfStream.ToArray();
    }

    private static void DefineStyles()
    {
        // Get the predefined style Normal.
        Style style = document.Styles["Normal"];
        // Because all styles are derived from Normal, the next line changes the
        // font of the whole document. Or, more exactly, it changes the font of
        // all styles and paragraphs that do not redefine the font.
        style.Font.Name = _fontName;

        style = document.Styles["Heading1"];
        style.Font.Size = 14;
        style.Font.Bold = true;
        style.Font.Color = Colors.White;
        style.ParagraphFormat.PageBreakBefore = false;
        style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        style.ParagraphFormat.Borders.Distance = "3pt";
        style.ParagraphFormat.Shading.Color = Color.FromRgbColor(255, new Color(151, 162, 216));
        style.ParagraphFormat.SpaceAfter = 4;

        style = document.Styles["Heading2"];
        style.Font.Size = 12;
        style.Font.Bold = true;
        style.Font.Color = Colors.Black;
        style.ParagraphFormat.PageBreakBefore = false;
        style.ParagraphFormat.Alignment = ParagraphAlignment.Center;
        style.ParagraphFormat.Borders.Distance = "2pt";
        style.ParagraphFormat.Shading.Color = Color.FromRgbColor(255, new Color(197, 202, 233));
        style.ParagraphFormat.AddTabStop("17cm", TabAlignment.Right, TabLeader.Spaces);
        style.ParagraphFormat.SpaceAfter = 3;


        style = document.Styles[StyleNames.Footer];
        style.Font.Size = 8;
        style.ParagraphFormat.AddTabStop("1cm", TabAlignment.Right);

        //// Create a new style called Reference based on style Normal
        //style = document.Styles.AddStyle("Reference", "Normal");
        //style.ParagraphFormat.SpaceBefore = "5mm";
        //style.ParagraphFormat.SpaceAfter = "5mm";
    }

    private static void DefineContentSection()
    {
        Section section = document.LastSection;

        Paragraph footerParagraph = section.Footers.Primary.AddParagraph();
        footerParagraph.Format.Alignment = ParagraphAlignment.Center;
        footerParagraph.Format.Font.Color = Colors.DimGray;
        footerParagraph.AddFormattedText("TVA acquittée sur les encaissements");
        footerParagraph.AddLineBreak();
        footerParagraph.AddFormattedText("Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
        footerParagraph.AddLineBreak();
        footerParagraph.AddFormattedText("Nullam turpis ante, congue eget quam vel.");



        // Create a paragraph for page number. See definition of style "Footer".
        Paragraph paragraph = new();
        paragraph.Format.Alignment = ParagraphAlignment.Right;
        paragraph.AddFormattedText("Page ");
        paragraph.AddPageField();
        paragraph.AddFormattedText(" / ");
        paragraph.AddNumPagesField();

        // Add paragraph to footer for odd pages.
        section.Footers.Primary.Add(paragraph);
        // Add clone of paragraph to footer for odd pages. Cloning is necessary because an object must
        // not belong to more than one other object. If you forget cloning an exception is thrown.
        section.Footers.EvenPage.Add(paragraph.Clone());
    }

    private static void RenderHeader()
    {
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

        Section section = document.LastSection;

        var imageM = section.Headers.Primary.AddImage(image);
        imageM.Height = "2cm";
        imageM.LockAspectRatio = true;
        imageM.RelativeVertical = RelativeVertical.Line;
        imageM.RelativeHorizontal = RelativeHorizontal.Margin;
        imageM.Top = ShapePosition.Top;
        //imageM.Left = ShapePosition.Right;
        imageM.WrapFormat.Style = WrapStyle.Through;
    }

    private static void RenderReferences()
    {
        Section section = document.LastSection;

        Paragraph paragraph = section.AddParagraph();

        paragraph.Format.Font.Size = 9;
        paragraph.Format.SpaceBefore = "4.5cm";

        paragraph.AddText("Date : " + DateTime.Today.ToShortDateString());
        paragraph.AddLineBreak();
        paragraph.AddText("Validité de l'offre : 30 jours");
    }

    private static void RenderAddress()
    {
        Section section = document.LastSection;

        Paragraph paragraph = section.AddParagraph();

        paragraph.Format.Font.Size = 9;
        paragraph.Format.LeftIndent = "10cm";

        paragraph.AddFormattedText("name/singleName", TextFormat.Bold);
        paragraph.AddLineBreak();
        paragraph.AddText("M. Dupont");
        paragraph.AddLineBreak();
        paragraph.AddText("address/line1");
        paragraph.AddLineBreak();
        paragraph.AddFormattedText("address/postalCode" + " " + "address/city", TextFormat.Bold);
        paragraph.AddLineBreak();
        paragraph.AddLineBreak();
        paragraph.AddText("email@email.com");
    }

    private static void RenderOrderNumber()
    {
        Section section = document.LastSection;

        Paragraph paragraph = section.AddParagraph();
        paragraph.Format.SpaceBefore = "1.4cm";
        paragraph.Format.Font.Bold = true;
        paragraph.Format.Font.Size = 14;
        paragraph.Format.Font.Color = MigraDocCore.DocumentObjectModel.Colors.DimGray;
        paragraph.Format.Alignment = ParagraphAlignment.Center;
        paragraph.AddText("Order Nbr 1122334455667788");
    }

    private static void RenderContent()
    {
        Table table = new();
        table.Borders.Width = 2;
        table.Borders.Color = Colors.White;

        table.AddColumn(Unit.FromCentimeter(12.3));
        Column column = table.AddColumn(Unit.FromCentimeter(2));
        column.Format.Alignment = ParagraphAlignment.Center;
        column = table.AddColumn(Unit.FromCentimeter(2));
        column.Format.Alignment = ParagraphAlignment.Center;
        column = table.AddColumn(Unit.FromCentimeter(2));
        column.Format.Alignment = ParagraphAlignment.Center;

        Row headingRow = table.AddRow();
        headingRow.Shading.Color = Colors.DarkGray;
        headingRow.Format.Font.Color = Colors.White;
        headingRow.Format.Font.Size = 11;
        headingRow.Format.Font.Bold = true;
        headingRow.Format.SpaceBefore = "0.15cm";
        headingRow.Format.SpaceAfter = "0.15cm";
        headingRow.HeadingFormat = true;

        headingRow.Cells[0].AddParagraph("Description");
        headingRow.Cells[1].AddParagraph("Prix");
        headingRow.Cells[2].AddParagraph("Qté");
        headingRow.Cells[3].AddParagraph("Total");

        for (int i = 0; i < 4; i++)
        {
            Row row = table.AddRow();
            row.Cells[0].AddParagraph($"{i}\tProduct {i}");

            row = table.AddRow();
            Cell cell = row.Cells[0]; // Remplacez par l'indice de votre cellule
            Paragraph paragraph = cell.AddParagraph("\t Texte en ");
            paragraph.AddFormattedText("gras", TextFormat.Bold);
            paragraph.AddText(" et ");
            paragraph.AddFormattedText("italique", TextFormat.Italic);
        }
        

        document.LastSection.Add(table);
    }

    private static void RenderTotal()
    {
        Table table = new();
        table.Rows.LeftIndent = "10cm";
        table.Borders.Color = Colors.Gray;

        table.AddColumn(Unit.FromCentimeter(5.0));
        Column column = table.AddColumn(Unit.FromCentimeter(3));
        column.Format.Alignment = ParagraphAlignment.Right;

        Row row = table.AddRow();
        row.Shading.Color = Colors.WhiteSmoke;
        row.Format.Font.Color = Colors.Black;
        row.Format.Font.Size = 11;
        row.Format.Font.Bold = false;
        row.Format.SpaceBefore = "0.15cm";
        row.Format.SpaceAfter = "0.15cm";

        row.Cells[0].AddParagraph("Total mensuel hors taxe");
        Cell cell = row.Cells[1];
        cell.Shading.Color = Colors.White;
        cell.Format.Alignment = ParagraphAlignment.Right;
        cell.AddParagraph("7,9 €");

        row = table.AddRow();
        row.Format.SpaceBefore = "0.15cm";
        row.Format.SpaceAfter = "0.15cm";
        cell = row.Cells[0];
            cell.Shading.Color = Colors.WhiteSmoke;
            cell.AddParagraph("Taxe");
        cell = row.Cells[1];
        cell.Shading.Color = Colors.White;
        cell.Format.Alignment = ParagraphAlignment.Right;
        cell.AddParagraph("20%");

        row = table.AddRow();
        row.Format.SpaceBefore = "0.15cm";
        row.Format.SpaceAfter = "0.15cm";
        cell = row.Cells[0];
        cell.Shading.Color = Colors.DarkGray;
        cell.Format.Font.Color = Colors.White;
        cell.AddParagraph("Total TTC");
        cell = row.Cells[1];
        cell.Shading.Color = Colors.WhiteSmoke;
        cell.Format.Font.Color = Colors.Black;
        cell.Format.Font.Bold = true;
        cell.Format.Alignment = ParagraphAlignment.Right;
        cell.AddParagraph("9,48 €");


        table.SetEdge(0, 0, table.Columns.Count, table.Rows.Count, Edge.Box, BorderStyle.Single, 1.1, Colors.Black);


        document.LastSection.Add(table);
    }

    private static void RenderBankDetails()
    {
        document.LastSection.AddParagraph("", "espace");

        Table table = new();
        table.Borders.Width = 2;
        table.Borders.Color = Colors.White;


        table.AddColumn(Unit.FromCentimeter(3.5));
        table.AddColumn(Unit.FromCentimeter(7));

        Row headingRow = table.AddRow();
        headingRow.Shading.Color = Colors.DarkGray;
        headingRow.Format.Font.Color = Colors.White;
        headingRow.Format.Font.Size = 9;
        headingRow.Format.Font.Bold = true;
        headingRow.Format.SpaceBefore = "0.1cm";
        headingRow.Format.SpaceAfter = "0.1cm";
        headingRow.HeadingFormat = true;

        Cell cell = headingRow.Cells[1];
        cell.Format.Alignment = ParagraphAlignment.Center;
        cell.AddParagraph("Bank details");

        Row row = table.AddRow();
        row.Format.SpaceBefore = "0.1cm";
        row.Format.SpaceAfter = "0.1cm";
        cell = row.Cells[0];
        cell.Shading.Color = Colors.WhiteSmoke;
        cell.AddParagraph("Address");
        cell = row.Cells[1];
        cell.Shading.Color = Colors.White;
        cell.AddParagraph("87 Hymoip Street");

        row = table.AddRow();
        row.Format.SpaceBefore = "0.1cm";
        row.Format.SpaceAfter = "0.1cm";
        cell = row.Cells[0];
        cell.Shading.Color = Colors.WhiteSmoke;
        cell.AddParagraph("SWIFT Code/BIC");
        cell = row.Cells[1];
        cell.Shading.Color = Colors.White;
        cell.AddParagraph("TGD16954SDGS");


        document.LastSection.Add(table);
    }

    private static void RenderTextBottom()
    {
        Section section = document.LastSection;

        Paragraph paragraph = section.AddParagraph();
        paragraph.Format.SpaceBefore = "1.0cm";
        paragraph.Format.Font.Bold = true;
        paragraph.Format.Font.Underline= Underline.Single;
        paragraph.Format.Font.Size = 12;
        paragraph.Format.Alignment = ParagraphAlignment.Center;
        paragraph.AddText("Please return this document, signed, to toto@email.com ");
    }

}
