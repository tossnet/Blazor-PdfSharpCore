namespace Share.PDF.Helpers;

using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore;


public class LayoutHelper
{
    private readonly PdfDocument _document;
    private readonly XUnit _topPosition;
    private readonly XUnit _bottomMargin;
    private XUnit _currentPosition;

    public LayoutHelper(PdfDocument document, XUnit topPosition, XUnit bottomMargin)
    {
        _document = document;
        _topPosition = topPosition;
        _bottomMargin = bottomMargin;
        // Set a value outside the page - a new page will be created on the first request.
        _currentPosition = bottomMargin + 10000;
    }

    public XUnit GetLinePosition(XUnit requestedHeight)
    {
        return GetLinePosition(requestedHeight, -1f);
    }

    public XUnit GetLinePosition(XUnit requestedHeight, XUnit requiredHeight)
    {
        XUnit required = requiredHeight == -1f ? requestedHeight : requiredHeight;
        if (_currentPosition + required > _bottomMargin)
            CreatePage();
        XUnit result = _currentPosition;
        _currentPosition += requestedHeight;
        return result;
    }

    public XGraphics Gfx { get; private set; }
    public PdfPage Page { get; private set; }

    private void CreatePage()
    {
        Page = _document.AddPage();
        Page.Size = PageSize.A4;
        Gfx = XGraphics.FromPdfPage(Page);
        _currentPosition = _topPosition;
    }
}
