namespace Share.PDF;

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
}
