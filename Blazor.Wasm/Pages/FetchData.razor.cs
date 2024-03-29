namespace Blazor.Wasm.Pages;

using Blazor.Wasm.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using PdfSharpCore.Fonts;
using CommonModels;
using Share.PDF.Models;
using System.IO;
using System;


public partial class FetchData
{
    [Inject] public IJSRuntime JS { get; set; }
    [Inject] public FontServices FontService { get; set; }

    private const string JAVASCRIPT_FILE = "./js/javascript.js";
    private IJSObjectReference JsModule { get; set; } = default!;
    private WeatherForecast[]? forecasts;

    protected override async Task OnInitializedAsync()
    {
        forecasts = await Http.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");

    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            JsModule ??= await JS.InvokeAsync<IJSObjectReference>("import", JAVASCRIPT_FILE);


            Fonts font = await FontService.LoadFonts();

            try
            {
                GlobalFontSettings.FontResolver = new CustomFontResolver(font);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message.ToString());
            }
        }

    }

    async Task PDFTable()
    {
        var imageFile = await GetImage("images/BackwardDiagonal.png");
        byte[] pdf = Share.PDF.Tables.PDFTable(forecasts, imageFile);
        await JsModule.InvokeVoidAsync("BlazorDownloadFile", "table.pdf", pdf);
    }

    async Task PDFAdvancedTable()
    { 
        byte[] pdf = Share.PDF.Tables.PDFAdvancedTable();
        await JsModule.InvokeVoidAsync("BlazorDownloadFile", "advancedtable.pdf", pdf);
    }

    async Task<byte[]> GetImage(string imageSource)
    {
        using var response = await Http.GetAsync(imageSource);
        response.EnsureSuccessStatusCode();
        Stream ms = await response.Content.ReadAsStreamAsync();
        byte[] byteArray;
        using (MemoryStream memoryStream = new())
        {
            ms.CopyTo(memoryStream);

            byteArray = memoryStream.ToArray();
        }
        return byteArray; 
    }
}