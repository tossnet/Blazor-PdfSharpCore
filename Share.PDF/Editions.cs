namespace Share.PDF;

using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;

public static class Editions
{
	public static byte[] HelloWord()
	{
		var PdfStream = new MemoryStream();

		var document = new PdfDocument(PdfStream);
		var page = document.AddPage();

		var gfx = XGraphics.FromPdfPage(page);
		var font = new XFont("OpenSans-Regular", 20, XFontStyle.Regular);

		var textColor = XBrushes.Black;
		var layout = new XRect(20, 20, page.Width, page.Height);
		var format = XStringFormats.Center;

		gfx.DrawString("Hello World!", font, textColor, layout, format);

		document.Save(PdfStream);

		return PdfStream.ToArray();
	}
}