namespace Share.PDF;

using PdfSharpCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

using MigraDocCore.Rendering;
using MigraDocCore.DocumentObjectModel;
using Section = MigraDocCore.DocumentObjectModel.Section;
using PdfSharpCore.Drawing.Layout;

public static class Editions
{

	private static PdfDocument? document;

    public static byte[] HelloWord()
	{
		// Create Document with info
		document = new();
        Common.DocumentInfo(document, "Hello world");

		// Create new page
		var page = document.AddPage();
		var gfx = XGraphics.FromPdfPage(page);
        //XFont font = new("OpenSans-Regular", 20, XFontStyle.Regular);
        XFont font = new("Arial", 20, XFontStyle.Regular);

        var textColor = XBrushes.Black;
		var layout = new XRect(0, 0, page.Width, page.Height);
		var format = XStringFormats.Center;

		gfx.DrawString("Hello World!", font, textColor, layout, format);

        SamplePage1();

		SamplePage2();

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
		style.Font.Name = "OpenSans-Regular";

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
		style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
	}

	
    static void SamplePage1()
	{
		PdfPage page = document.AddPage();

		XGraphics gfx = XGraphics.FromPdfPage(page);
		// HACK
		gfx.MUH = PdfFontEncoding.Unicode;
		//gfx.MFEH = PdfFontEmbedding.Default;

		XFont font = new("OpenSans-Regular", 13, XFontStyle.Bold);

		//gfx.DrawString("The following paragraph was rendered using MigraDocCore:", font, XBrushes.Black,
		//new XRect(100, 100, page.Width - 200, 300), XStringFormats.Center);

		//// You always need a MigraDocCore document for rendering.
		Document doc = new();


        DefineStyles(doc);

        Section sec = doc.AddSection();
		// Add a single paragraph with some text and format information.
		Paragraph para = sec.AddParagraph();
		para.Format.Alignment = ParagraphAlignment.Justify;
		para.Format.Font.Name = "OpenSans-Regular";
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

    static void SamplePage2()
	{
         string text = "Facin exeraessisit la consenim iureet dignibh eu facilluptat vercil dunt autpat. " +
                "Ecte magna faccum dolor sequisc iliquat, quat, quipiss equipit accummy niate magna " +
                "facil iure eraesequis am velit, quat atis dolore dolent luptat nulla adio odipissectet " +
                "lan venis do essequatio conulla facillandrem zzriusci bla ad minim inis nim velit eugait " +
                "aut aut lor at ilit ut nulla ate te eugait alit augiamet ad magnim iurem il eu feuissi.\n" +
                "Guer sequis duis eu feugait luptat lum adiamet, si tate dolore mod eu facidunt adignisl in " +
                "henim dolorem nulla faccum vel inis dolutpatum iusto od min ex euis adio exer sed del " +
                "dolor ing enit veniamcon vullutat praestrud molenis ciduisim doloborem ipit nulla consequisi.\n" +
                "Nos adit pratetu eriurem delestie del ut lumsandreet nis exerilisit wis nos alit venit praestrud " +
                "dolor sum volore facidui blaor erillaortis ad ea augue corem dunt nis  iustinciduis euisi.\n" +
                "Ut ulputate volore min ut nulpute dolobor sequism olorperilit autatie modit wisl illuptat dolore " +
                "min ut in ute doloboreet ip ex et am dunt at.";

        PdfPage page = document.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XFont font = new XFont("Times New Roman", 10, XFontStyle.Bold);
        XTextFormatter tf = new XTextFormatter(gfx);

        XRect rect = new XRect(40, 100, 250, 220);
        gfx.DrawRectangle(XBrushes.SeaShell, rect);
        //tf.Alignment = ParagraphAlignment.Left;
        tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

        rect = new XRect(310, 100, 250, 220);
        gfx.DrawRectangle(XBrushes.SeaShell, rect);
        tf.Alignment = XParagraphAlignment.Right;
        tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

        rect = new XRect(40, 400, 250, 220);
        gfx.DrawRectangle(XBrushes.SeaShell, rect);
        tf.Alignment = XParagraphAlignment.Center;
        tf.DrawString(text, font, XBrushes.Black, rect, XStringFormats.TopLeft);

        rect = new XRect(310, 400, 250, 220);
        gfx.DrawRectangle(XBrushes.SeaShell, rect);
        tf.Alignment = XParagraphAlignment.Justify;
    }


    public static byte[] DrawGraphics()
	{
		// Create Document with info
		document = new();
		Common.DocumentInfo(document, "Hello world");

		// Create new page
		var page = document.AddPage();
		var gfx = XGraphics.FromPdfPage(page);

        DrawHeaderBottomText(page, gfx, "Some graphics");


        XFont font = new("OpenSans-Regular", 20, XFontStyle.Regular);

		var textColor = XBrushes.Black;
		var layout = new XRect(0, 0, page.Width, page.Height);
		var format = XStringFormats.Center;

		gfx.DrawString("look in next page ;)", font, textColor, layout, format);


		// Create new page
		page = document.AddPage();
		page.Orientation = PageOrientation.Landscape;

		gfx = XGraphics.FromPdfPage(page);


        DrawHeaderBottomText(page, gfx, "Some graphics");

        gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(1, 0.68, 0, 0.12)), new XRect(30, 60, 50, 50));
		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0, 0.70, 1, 0)), new XRect(550, 60, 50, 50));

		gfx.DrawRoundedRectangle(new XSolidBrush(XColor.FromArgb(255, 87, 202, 92)), new XRect(90, 100, 50, 50), new XSize(50, 50));
		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0, 1, 0, 0)), new XRect(150, 100, 50, 50));

		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0.7, 0, 0.70, 1, 0)), new XRect(90, 200, 50, 50));
		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0.5, 0, 0.70, 1, 0)), new XRect(150, 100, 50, 50));

		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0.35, 0.15, 0, 0.08)), new XRect(50, 360, 50, 50));
		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0.25, 0.10, 0, 0.05)), new XRect(150, 360, 50, 50));
		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0.15, 0.05, 0, 0)), new XRect(250, 360, 50, 50));

		MemoryStream PdfStream = new();
		document.Save(PdfStream);

		return PdfStream.ToArray();
	}





	private static void DrawHeaderBottomText(PdfPage page, XGraphics gfx, string title)
	{
		XRect rect = new(new XPoint(), gfx.PageSize);
		rect.Inflate(-10, -15);
		XFont font = new("OpenSans-Regular", 14, XFontStyle.Bold);
		gfx.DrawString(title, font, XBrushes.MidnightBlue, rect, XStringFormats.TopCenter);

		rect.Offset(0, 5);
		font = new XFont("OpenSans-Regular", 8, XFontStyle.Italic);
		XStringFormat format = new()
		{
			Alignment = XStringAlignment.Near,
			LineAlignment = XLineAlignment.Far
		};
		gfx.DrawString("Blazor", font, XBrushes.DarkOrchid, rect, format);

		font = new XFont("OpenSans-Regular", 8);
		format.Alignment = XStringAlignment.Center;
		gfx.DrawString(document.PageCount.ToString(), font, XBrushes.DarkOrchid, rect, format);

		document.Outlines.Add(title, page, true);
	}
}