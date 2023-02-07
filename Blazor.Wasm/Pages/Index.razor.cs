namespace Blazor.Wasm.Pages;

using Blazor.Wasm.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PdfSharpCore.Fonts;
using Share.PDF.Models;

public partial class Index
{
	[Inject] public IJSRuntime JS { get; set; }
	[Inject] public FontServices FontService { get; set; }

	private const string JAVASCRIPT_FILE = "./Pages/Index.razor.js";
	private IJSObjectReference JsModule { get; set; } = default!;


	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			JsModule ??= await JS.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);

			Fonts font = await FontService.LoadFonts();

			try
			{
				//if (GlobalFontSettings.FontResolver is not CustomFontResolver)
				//{
				//if (GlobalFontSettings.FontResolver  == null)
				//{
				GlobalFontSettings.FontResolver = new CustomFontResolver(font);
				//}
			}
			catch (Exception)
			{
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