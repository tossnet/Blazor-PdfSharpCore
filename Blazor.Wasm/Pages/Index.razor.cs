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
    [Inject] public NavigationManager nav { get; set; }

    private const string JAVASCRIPT_FILE = "./js/javascript.js";
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
			catch (Exception e)
			{
				Console.WriteLine(e.Message.ToString());
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

		await JsModule.InvokeVoidAsync("BlazorDownloadFile", "graphics.pdf", pdf);
	}
	
	void PrintTable()
	{
		nav.NavigateTo("fetchdata");
	}

	async Task PrintUnicode()
	{ 
		byte[] pdf = Share.PDF.Unicode.UnicodeSample();
	
	    await JsModule.InvokeVoidAsync("BlazorDownloadFile", "unicode.pdf", pdf);
	}


    async Task MixMigraSharpClick()
    {
        byte[] pdf = Share.PDF.MixMigraSharp.GetRenderer();

        await JsModule.InvokeVoidAsync("BlazorDownloadFile", "mixMigraSharp.pdf", pdf);
    }

    async Task MultiPageClick()
    {
        byte[] pdf = Share.PDF.MultiPages.GetRenderer();

        await JsModule.InvokeVoidAsync("BlazorDownloadFile", "MultiPages.pdf", pdf);
    }


    async Task HelloMigraDocCoreClick()
    {
        byte[] pdf = Share.PDF.HelloMigraDocCore.GetRendered();

        await JsModule.InvokeVoidAsync("BlazorDownloadFile", "HelloMigraDocCore.pdf", pdf);
    }
}