namespace Blazor.Wasm.Pages;

using Blazor.Wasm.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PdfSharpCore.Fonts;

public partial class Index
{
	[Inject] public IJSRuntime JS { get; set; }
	[Inject] public HttpClient Client { get; set; }
	[Inject] public FontServices FontService { get; set; }

	private const string JAVASCRIPT_FILE = "./Pages/Index.razor.js";
	private IJSObjectReference JsModule { get; set; } = default!;


	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			if (JsModule == null)
			{
				JsModule = await JS.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);
			}

			byte[] font = await FontService.LoadFontData("OpenSans-Regular.ttf");

			try
			{
				//if (GlobalFontSettings.FontResolver is not CustomFontResolver)
				//{
				//if (GlobalFontSettings.FontResolver  == null)
				//{
				GlobalFontSettings.FontResolver = new CustomFontResolver(Client, font);
				//}
			}
			catch (Exception)
			{
			}
		}
	}

	async Task PDF()
	{
		byte[] pdf = Share.PDF.Editions.HelloWord();

		await JsModule.InvokeVoidAsync("BlazorDownloadFile", "exemple.pdf", pdf);
	}
}