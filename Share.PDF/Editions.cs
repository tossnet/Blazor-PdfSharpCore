namespace Share.PDF;

using PdfSharpCore;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

using MigraDocCore.Rendering;
using MigraDocCore.DocumentObjectModel;
using MigraDocCore.DocumentObjectModel.Tables;
using Section = MigraDocCore.DocumentObjectModel.Section;

using CommonModels;

public static class Editions
{

	private static PdfDocument? document;

    public static byte[] HelloWord()
	{
		// Create Document with info
		document = new();
		DocumentInfo("Hello world");

		// Create new page
		var page = document.AddPage();
		var gfx = XGraphics.FromPdfPage(page);
        XFont font = new("OpenSans-Regular", 20, XFontStyle.Regular);

		var textColor = XBrushes.Black;
		var layout = new XRect(0, 0, page.Width, page.Height);
		var format = XStringFormats.Center;

		gfx.DrawString("Hello World!", font, textColor, layout, format);

        SamplePage1();

        MemoryStream PdfStream = new();
		document.Save(PdfStream);

		return PdfStream.ToArray();
	}


	public static byte[] PDFTable(WeatherForecast[] forecasts)
	{
		// Create Document with info
		document = new();
		DocumentInfo("Table");

		// Create new page
		PdfPage page = document.AddPage();
		XGraphics gfx = XGraphics.FromPdfPage(page);
		XFont font = new("OpenSans-Regular", 20, XFontStyle.Regular);

        // You always need a MigraDocCore document for rendering.
        MigraDocCore.DocumentObjectModel.Document doc = new();
		// Each MigraDocCore document needs at least one section.
		Section sec = doc.AddSection();

		DefineStyles(doc);


        Table table = CreateTable(doc, forecasts);

		// Create a renderer and prepare (=layout) the document
		MigraDocCore.Rendering.DocumentRenderer docRenderer = new(doc);
		docRenderer.PrepareDocument();

		// Render the paragraph. You can render tables or shapes the same way.
		docRenderer.RenderObject(gfx, XUnit.FromCentimeter(1), XUnit.FromCentimeter(5), "12cm", table);


		MemoryStream PdfStream = new();
		document.Save(PdfStream);

		return PdfStream.ToArray();
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

	private static Table CreateTable(Document document, WeatherForecast[] forecasts)
	{
		document.LastSection.AddParagraph("Simple Tables", "Heading2");

		var table = new Table();
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
			

		table.SetEdge(0, 0, 4, 1, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

		document.LastSection.Add(table);

		return table;
		//// Each MigraDocCore document needs at least one section.
		//Section section = doc.AddSection();


		//// Create footer
		//Paragraph paragraph = section.Footers.Primary.AddParagraph();
		//paragraph.AddText("PowerBooks Inc · Sample Street 42 · 56789 Cologne · Germany");
		//paragraph.Format.Font.Size = 9;
		//paragraph.Format.Alignment = ParagraphAlignment.Center;

		//// Create the item table
		//var table = section.AddTable();
		//table.Style = "Table";
		//table.Borders.Color = MigraDocCore.DocumentObjectModel.Colors.Blue;
		//table.Borders.Width = 0.25;
		//table.Borders.Left.Width = 0.5;
		//table.Borders.Right.Width = 0.5;
		//table.Rows.LeftIndent = 0;

		//// Before you can add a row, you must define the columns
		//Column column = table.AddColumn("1cm");
		//column.Format.Alignment = ParagraphAlignment.Center;

		//column = table.AddColumn("2.5cm");
		//column.Format.Alignment = ParagraphAlignment.Right;

		//column = table.AddColumn("3cm");
		//column.Format.Alignment = ParagraphAlignment.Right;

		//column = table.AddColumn("3.5cm");
		//column.Format.Alignment = ParagraphAlignment.Right;

		//column = table.AddColumn("2cm");
		//column.Format.Alignment = ParagraphAlignment.Center;

		//column = table.AddColumn("4cm");
		//column.Format.Alignment = ParagraphAlignment.Right;

		//// Create the header of the table
		//Row row = table.AddRow();
		//row.HeadingFormat = true;
		//row.Format.Alignment = ParagraphAlignment.Center;
		//row.Format.Font.Bold = true;
		//row.Shading.Color = MigraDocCore.DocumentObjectModel.Colors.Green;
		//row.Cells[0].AddParagraph("Item");
		//row.Cells[0].Format.Font.Bold = false;
		//row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
		//row.Cells[0].VerticalAlignment = MigraDocCore.DocumentObjectModel.Tables.VerticalAlignment.Bottom;
		//row.Cells[0].MergeDown = 1;
		//row.Cells[1].AddParagraph("Title and Author");
		//row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
		//row.Cells[1].MergeRight = 3;
		//row.Cells[5].AddParagraph("Extended Price");
		//row.Cells[5].Format.Alignment = ParagraphAlignment.Left;
		//row.Cells[5].VerticalAlignment = MigraDocCore.DocumentObjectModel.Tables.VerticalAlignment.Bottom;
		//row.Cells[5].MergeDown = 1;

		//row = table.AddRow();
		//row.HeadingFormat = true;
		//row.Format.Alignment = ParagraphAlignment.Center;
		//row.Format.Font.Bold = true;
		//row.Shading.Color = MigraDocCore.DocumentObjectModel.Colors.Orange;
		//row.Cells[1].AddParagraph("Quantity");
		//row.Cells[1].Format.Alignment = ParagraphAlignment.Left;
		//row.Cells[2].AddParagraph("Unit Price");
		//row.Cells[2].Format.Alignment = ParagraphAlignment.Left;
		//row.Cells[3].AddParagraph("Discount (%)");
		//row.Cells[3].Format.Alignment = ParagraphAlignment.Left;
		//row.Cells[4].AddParagraph("Taxable");
		//row.Cells[4].Format.Alignment = ParagraphAlignment.Left;

		//table.SetEdge(0, 0, 6, 2, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);

		//doc.LastSection.Add(table);
	}


	private static  void FillContent(Document doc)
	{

		//// Iterate the invoice items
		//double totalExtendedPrice = 0;
		//XPathNodeIterator iter = navigator.Select("/invoice/items/*");
		//while (iter.MoveNext())
		//{
		//	item = iter.Current;
		//	double quantity = GetValueAsDouble(item, "quantity");
		//	double price = GetValueAsDouble(item, "price");
		//	double discount = GetValueAsDouble(item, "discount");

		//	// Each item fills two rows
		//	Row row1 = table.AddRow();
		//	Row row2 = table.AddRow();
		//	row1.TopPadding = 1.5;
		//	row1.Cells[0].Shading.Color = TableGray;
		//	row1.Cells[0].VerticalAlignment = VerticalAlignment.Center;
		//	row1.Cells[0].MergeDown = 1;
		//	row1.Cells[1].Format.Alignment = ParagraphAlignment.Left;
		//	row1.Cells[1].MergeRight = 3;
		//	row1.Cells[5].Shading.Color = TableGray;
		//	row1.Cells[5].MergeDown = 1;

		//	row1.Cells[0].AddParagraph(GetValue(item, "itemNumber"));
		//	paragraph = row1.Cells[1].AddParagraph();
		//	paragraph.AddFormattedText(GetValue(item, "title"), TextFormat.Bold);
		//	paragraph.AddFormattedText(" by ", TextFormat.Italic);
		//	paragraph.AddText(GetValue(item, "author"));
		//	row2.Cells[1].AddParagraph(GetValue(item, "quantity"));
		//	row2.Cells[2].AddParagraph(price.ToString("0.00") + " €");
		//	row2.Cells[3].AddParagraph(discount.ToString("0.0"));
		//	row2.Cells[4].AddParagraph();
		//	row2.Cells[5].AddParagraph(price.ToString("0.00"));
		//	double extendedPrice = quantity * price;
		//	extendedPrice = extendedPrice * (100 - discount) / 100;
		//	row1.Cells[5].AddParagraph(extendedPrice.ToString("0.00") + " €");
		//	row1.Cells[5].VerticalAlignment = VerticalAlignment.Bottom;
		//	totalExtendedPrice += extendedPrice;

		//	table.SetEdge(0, table.Rows.Count - 2, 6, 2, Edge.Box, BorderStyle.Single, 0.75);
		//}


	}

	public static byte[] DrawGraphics()
	{
		// Create Document with info
		document = new();
		DocumentInfo("Hello world");

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


	private static void DocumentInfo(string title)
	{
		document.Info.Title = title;
		document.Info.Author = "Christophe Peugnet";
		document.Info.Subject = "Sample";
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