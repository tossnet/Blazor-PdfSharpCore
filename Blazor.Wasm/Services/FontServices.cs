namespace Blazor.Wasm.Services;

public sealed class FontServices
{
	private readonly HttpClient _httpClient;
	public FontServices(HttpClient httpClient)
	{
		this._httpClient = httpClient;
	}

	public async Task<byte[]> LoadFontData(string name)
	{
		var sourceStream = await _httpClient.GetStreamAsync($"fonts/{name}");

		using (MemoryStream memoryStream = new())
		{
			sourceStream.CopyTo(memoryStream);
			return memoryStream.ToArray();
		}
	}
}
