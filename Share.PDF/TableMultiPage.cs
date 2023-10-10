namespace Share.PDF;

using MigraDocCore.DocumentObjectModel.Tables;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.Rendering;

public static class TableMultiPage
{
    public static byte[] GetPDF()
    {
        Document doc = new();
        doc.Info.Author = "me";
        doc.Info.Subject = "MigraDoc PDF";
        doc.Info.Title = "My PDF";

        Section sec = doc.AddSection();

        //sec.PageSetup = doc.DefaultPageSetup.Clone();
        //sec.PageSetup.TopMargin = Unit.FromCentimeter(5);
        //sec.PageSetup.BottomMargin = Unit.FromCentimeter(5);
        sec.PageSetup.PageFormat = PageFormat.A4;
        sec.PageSetup.Orientation = Orientation.Portrait;
        CreateTableMultiPage( sec);


        DefineContentSection(doc);

        PdfDocumentRenderer renderer = new(true);
        renderer.Document = doc;

        renderer.RenderDocument();

        MemoryStream PdfStream = new();
        renderer.PdfDocument.Save(PdfStream);

        return PdfStream.ToArray();
    }


    private static void CreateTableMultiPage(Section sec)
    {
        Table table = new();

        table.Borders.Width = 0.75;

        table.AddColumn(Unit.FromCentimeter(1.5));
        table.AddColumn(Unit.FromCentimeter(4));

        Row row = table.AddRow();
        row.HeadingFormat = true;
        row.Shading.Color = Colors.PaleGoldenrod;
        Cell cell = row.Cells[0];
        cell.AddParagraph("N.");
        cell = row.Cells[1];
        cell.AddParagraph("Info");

        for (int line = 0; line < 100; ++line)
        {
            row = table.AddRow();
            row.Borders.Top.Width = 1;

            cell = row.Cells[0];
            cell.AddParagraph(line.ToString());

            cell = row.Cells[1];
            cell.Format.Alignment = ParagraphAlignment.Center;
            cell.AddParagraph("blablablaa");
        }

        sec.Add(table);
    }


    static void DefineContentSection(Document document)
    {
        Section section = document.AddSection();
        section.PageSetup.OddAndEvenPagesHeaderFooter = true;
        section.PageSetup.StartingNumber = 1;

        HeaderFooter header = section.Headers.Primary;
        header.AddParagraph("\tOdd Page Header");

        header = section.Headers.EvenPage;
        header.AddParagraph("Even Page Header");

        // Create a paragraph with centered page number. See definition of style "Footer".
        Paragraph paragraph = new Paragraph();
        paragraph.AddTab();
        paragraph.AddPageField();

        // Add paragraph to footer for odd pages.
        section.Footers.Primary.Add(paragraph);
        // Add clone of paragraph to footer for odd pages. Cloning is necessary because an object must
        // not belong to more than one other object. If you forget cloning an exception is thrown.
        section.Footers.EvenPage.Add(paragraph.Clone());
    }

}
