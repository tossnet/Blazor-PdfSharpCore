namespace Blazor.Wasm.Pages;

using Blazor.Wasm.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PdfSharpCore.Drawing;
using PdfSharpCore.Fonts;
using PdfSharpCore.Pdf;
using PdfSharpCore.Utils;


public partial class Index
{
	[Inject] public IJSRuntime js { get; set; }
	[Inject] public HttpClient _client { get; set; }
	[Inject] public FontServices FontService { get; set; }

	private const string JAVASCRIPT_FILE = "./Pages/Index.razor.js";
	private IJSObjectReference JsModule { get; set; } = default!;


	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			if (JsModule == null)
			{
				JsModule = await js.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
			}

			byte[] font = await FontService.LoadFontData("OpenSans-Regular.ttf");
			
			if (GlobalFontSettings.FontResolver is not CustomFontResolver)
			//{
			//if (GlobalFontSettings.FontResolver  == null)
			{
				GlobalFontSettings.FontResolver = new CustomFontResolver(_client, font);
			}
			//}
		}
	}

	async Task PDF()
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

		var pdf= PdfStream.ToArray();
		await JsModule.InvokeVoidAsync("BlazorDownloadFile", "exemple.pdf", pdf);
	}
}