namespace Share.PDF;

using PdfSharpCore.Pdf;


internal static class Common
{
    internal static void DocumentInfo(PdfDocument document, string title)
    {
        document.Info.Title = title;
        document.Info.Author = "Christophe Peugnet";
        document.Info.Subject = "Sample";
    }
}
