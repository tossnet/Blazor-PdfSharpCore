namespace Share.PDF;

using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf;
public class Tools
{

    public static PdfDocument Combine(MemoryStream pdf, PdfDocument combineDocument)
    {
        // Open the document to import pages from it.
        PdfDocument inputDocument = PdfReader.Open(pdf, PdfDocumentOpenMode.Import);

        // Iterate pages
        int count = inputDocument.PageCount;
        for (int idx = 0; idx < count; idx++)
        {
            // Get the page from the external document...
            PdfPage page = inputDocument.Pages[idx];
            // ...and add it to the output document.
            combineDocument.AddPage(page);
        }

        return combineDocument;
    }
}
