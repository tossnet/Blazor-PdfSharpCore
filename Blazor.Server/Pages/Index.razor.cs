namespace Blazor.Server.Pages;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PdfSharpCore.Fonts;
using PdfSharpCore.Utils;

public partial class Index
{
	[Inject] public IJSRuntime JS { get; set; }

	private const string JAVASCRIPT_FILE = "./Pages/Index.razor.js";
	private IJSObjectReference JsModule { get; set; } = default!;


	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			JsModule ??= await JS.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);

			if (PdfSharpCore.Fonts.GlobalFontSettings.FontResolver is not FontResolver)
			{
				GlobalFontSettings.FontResolver = new FontResolver();
			}
		}
	}

	async Task HelloWord()
	{
		byte[] pdf = Share.PDF.Editions.HelloWord();

		await JsModule.InvokeVoidAsync("BlazorDownloadFile", "sample.pdf", pdf);
	}

	async Task DrawGraphics()
	{
		byte[] pdf = Share.PDF.Editions.DrawGraphics();

		await JsModule.InvokeVoidAsync("BlazorDownloadFile", "sample.pdf", pdf);
	}
}