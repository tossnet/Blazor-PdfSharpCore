namespace Share.PDF;

using PdfSharpCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

public static class Editions
{
	public static byte[] HelloWord()
	{
		// Create Document with info
		PdfDocument document = new();
		DocumentInfo(document, "Hello world");

		// Create new page
		var page = document.AddPage();

		var gfx = XGraphics.FromPdfPage(page);

		var font = new XFont("OpenSans-Regular", 20, XFontStyle.Regular);

		var textColor = XBrushes.Black;
		var layout = new XRect(0, 0, page.Width, page.Height);
		var format = XStringFormats.Center;

		gfx.DrawString("Hello World!", font, textColor, layout, format);

		MemoryStream PdfStream = new();
		document.Save(PdfStream);

		return PdfStream.ToArray();
	}

	public static byte[] DrawGraphics()
	{
		// Create Document with info
		PdfDocument document = new();
		DocumentInfo(document, "Hello world");

		// Create new page
		var page = document.AddPage();

		var gfx = XGraphics.FromPdfPage(page);

		DrawTitle(document, page, gfx, "Some graphics");


		var font = new XFont("OpenSans-Regular", 20, XFontStyle.Regular);

		var textColor = XBrushes.Black;
		var layout = new XRect(0, 0, page.Width, page.Height);
		var format = XStringFormats.Center;

		gfx.DrawString("look in next page ;)", font, textColor, layout, format);


		// Create new page
		page = document.AddPage();
		page.Orientation = PageOrientation.Landscape;

		gfx = XGraphics.FromPdfPage(page);

		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(1, 0.68, 0, 0.12)), new XRect(30, 60, 50, 50));
		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0, 0.70, 1, 0)), new XRect(550, 60, 50, 50));

		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0, 0, 0, 0)), new XRect(90, 100, 50, 50));
		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0, 0, 0, 0)), new XRect(150, 100, 50, 50));

		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0.7, 0, 0.70, 1, 0)), new XRect(90, 100, 50, 50));
		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0.5, 0, 0.70, 1, 0)), new XRect(150, 100, 50, 50));

		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0.35, 0.15, 0, 0.08)), new XRect(50, 360, 50, 50));
		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0.25, 0.10, 0, 0.05)), new XRect(150, 360, 50, 50));
		gfx.DrawRectangle(new XSolidBrush(XColor.FromCmyk(0.15, 0.05, 0, 0)), new XRect(250, 360, 50, 50));

		MemoryStream PdfStream = new();
		document.Save(PdfStream);

		return PdfStream.ToArray();
	}


	private static void DocumentInfo(PdfDocument document, string title)
	{
		document.Info.Title = title;
		document.Info.Author = "Christophe Peugnet";
		document.Info.Subject = "Sample";
	}


	private static void DrawTitle(PdfDocument document, PdfPage page, XGraphics gfx, string title)
	{
		XRect rect = new(new XPoint(), gfx.PageSize);
		rect.Inflate(-10, -15);
		XFont font = new XFont("OpenSans-Regular", 14, XFontStyle.Bold);
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