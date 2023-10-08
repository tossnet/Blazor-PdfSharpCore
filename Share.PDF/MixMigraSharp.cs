// https://github.com/ststeiger/PdfSharpCore/blob/master/docs/MigraDocCore/samples/MixMigraDocCoreAndPDFsharpCore.md

namespace Share.PDF;

using MigraDocCore.DocumentObjectModel;
using MigraDocCore.Rendering;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class MixMigraSharp
{


    private static double A4Width = XUnit.FromCentimeter(21).Point;
    private static double A4Height = XUnit.FromCentimeter(29.7).Point;

    // Helper Routine : Calculates the area of a scaled down page:
    private static XRect GetRect(int index)
    {
        XRect rect = new XRect(0, 0, A4Width / 3 * 0.9, A4Height / 3 * 0.9);
        rect.X = (index % 3) * A4Width / 3 + A4Width * 0.05 / 3;
        rect.Y = (index / 3) * A4Height / 3 + A4Height * 0.05 / 3;
        return rect;
    }

    public static byte[] GetRenderer()
    {
        DateTime now = DateTime.Now;

        string filename = "MixMigraDocCoreAndPdfSharpCore.pdf";
        filename = Guid.NewGuid().ToString("D").ToUpper() + ".pdf";

        PdfDocument document = new PdfDocument();
        document.Info.Title = "PdfSharpCore XGraphic Sample";
        document.Info.Author = "Stefan Lange";
        document.Info.Subject = "Created with code snippets that show the use of graphical functions";
        document.Info.Keywords = "PdfSharpCore, XGraphics";

        SamplePage1(document);
        SamplePage2(document);

        Debug.WriteLine("seconds=" + (DateTime.Now - now).TotalSeconds.ToString());


        MemoryStream PdfStream = new();
        document.Save(PdfStream);

        return PdfStream.ToArray();
    }

    static void SamplePage1(PdfDocument document)
    {
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        // HACK
        gfx.MUH = PdfFontEncoding.Unicode;
        //gfx.MFEH = PdfFontEmbedding.Default;

        XFont font = new XFont("Verdana", 13, XFontStyle.Bold);

        gfx.DrawString("The following paragraph was rendered using MigraDocCore:", font, XBrushes.Black,
        new XRect(100, 100, page.Width - 200, 300), XStringFormats.Center);

        // You always need a MigraDocCore document for rendering.
        Document doc = new Document();
        Section sec = doc.AddSection();
        // Add a single paragraph with some text and format information.
        Paragraph para = sec.AddParagraph();
        para.Format.Alignment = ParagraphAlignment.Justify;
        para.Format.Font.Name = "Times New Roman";
        para.Format.Font.Size = 12;
        para.Format.Font.Color = MigraDocCore.DocumentObjectModel.Colors.DarkGray;
        para.Format.Font.Color = MigraDocCore.DocumentObjectModel.Colors.DarkGray;
        para.AddText("Duisism odigna acipsum delesenisl ");
        para.AddFormattedText("ullum in velenit", TextFormat.Bold);
        para.AddText(" ipit iurero dolum zzriliquisis nit wis dolore vel et nonsequipit, velendigna " +
        "auguercilit lor se dipisl duismod tatem zzrit at laore magna feummod oloborting ea con vel " +
        "essit augiati onsequat luptat nos diatum vel ullum illummy nonsent nit ipis et nonsequis " +
        "niation utpat. Odolobor augait et non etueril landre min ut ulla feugiam commodo lortie ex " +
        "essent augait el ing eumsan hendre feugait prat augiatem amconul laoreet. ≤≥≈≠");
        para.Format.Borders.Distance = "5pt";
        para.Format.Borders.Color = Colors.Gold;

        // Create a renderer and prepare (=layout) the document
        MigraDocCore.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(doc);
        docRenderer.PrepareDocument();

        // Render the paragraph. You can render tables or shapes the same way.
        docRenderer.RenderObject(gfx, XUnit.FromCentimeter(5), XUnit.FromCentimeter(10), "12cm", para);
    }

    static void SamplePage2(PdfDocument document)
    {
        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        // HACK
        gfx.MUH = PdfFontEncoding.Unicode;
        //gfx.MFEH = PdfFontEmbedding.Default;

        // Create document from HalloMigraDoc sample
        Document doc = CreateDocument();

        // Create a renderer and prepare (=layout) the document
        MigraDocCore.Rendering.DocumentRenderer docRenderer = new DocumentRenderer(doc);
        docRenderer.PrepareDocument();

        // For clarity we use point as unit of measure in this sample.
        // A4 is the standard letter size in Germany (21cm x 29.7cm).
        XRect A4Rect = new XRect(0, 0, A4Width, A4Height);

        int pageCount = docRenderer.FormattedDocument.PageCount;
        for (int idx = 0; idx < pageCount; idx++)
        {
            XRect rect = GetRect(idx);

            // Use BeginContainer / EndContainer for simplicity only. You can naturally use you own transformations.
            XGraphicsContainer container = gfx.BeginContainer(rect, A4Rect, XGraphicsUnit.Point);

            // Draw page border for better visual representation
            gfx.DrawRectangle(XPens.LightGray, A4Rect);

            // Render the page. Note that page numbers start with 1.
            docRenderer.RenderPage(gfx, idx + 1);

            // Note: The outline and the hyperlinks (table of content) does not work in the produced PDF document.

            // Pop the previous graphical state
            gfx.EndContainer(container);
        }

    }


    /// <summary>
    /// Creates an absolutely minimalistic document.
    /// </summary>
    private static Document CreateDocument()
    {
        // Create a new MigraDocCore document
        Document document = new Document();

        // Add a section to the document
        Section section = document.AddSection();

        // Add a paragraph to the section
        Paragraph paragraph = section.AddParagraph();
        paragraph.Format.Font.Color = Color.FromCmyk(100, 30, 20, 50);

        // Add some text to the paragraph
        paragraph.AddFormattedText("Hello, World!", TextFormat.Bold);

        return document;
    }
}
