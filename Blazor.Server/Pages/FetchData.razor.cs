namespace Blazor.Server.Pages;

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using PdfSharpCore.Utils;
using PdfSharpCore.Fonts;
using Microsoft.AspNetCore.Components;
using CommonModels;

public partial class FetchData
{
	[Inject] public IJSRuntime JS { get; set; }

	private const string JAVASCRIPT_FILE = "./js/javascript.js";
	private IJSObjectReference JsModule { get; set; } = default!;
	private WeatherForecast[]? forecasts;

    protected override async Task OnInitializedAsync()
    {
        forecasts = await ForecastService.GetForecastAsync(DateOnly.FromDateTime(DateTime.Now));
	}
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

	async Task PDFTable()
	{
		byte[] pdf = Share.PDF.Editions.PDFTable(forecasts);

		await JsModule.InvokeVoidAsync("BlazorDownloadFile", "table.pdf", pdf);
	}
}