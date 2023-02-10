namespace Blazor.Wasm;

using PdfSharpCore.Fonts;
using Share.PDF.Models;

public class CustomFontResolver : IFontResolver
{
	//private HttpClient _httpClient;
	private readonly Fonts _fontLoaded;

	public CustomFontResolver(Fonts FontLoaded)
	{
		//_httpClient = httpClient;
		_fontLoaded = FontLoaded;
	}

	//public string DefaultFontName => throw new NotImplementedException();

    public string DefaultFontName => "OpenSans-Regular";



    public byte[] GetFont(string faceName)
	{
		// causes an error because it blocks the UI (maybe the multi-thread in
		// .NET8 will unblock this problem) :
		//return LoadFontData("OpenSans-Regular.ttf").Result;

		return faceName switch
		{
			"OpenSans-Bold.ttf" => _fontLoaded.OpenSansBold,
			"OpenSans-BoldItalic.ttf" => _fontLoaded.OpenSansBoldItalic,
			"OpenSans-Italic.ttf" => _fontLoaded.OpenSansItalic,
			_ => _fontLoaded.OpenSans,
		};
	}

	public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
	{

		if (familyName.Equals("OpenSans-Regular", StringComparison.CurrentCultureIgnoreCase))
		{
			if (isBold && isItalic)
			{
				return new FontResolverInfo("OpenSans-BoldItalic.ttf");
			}
			else if (isBold)
			{
				return new FontResolverInfo("OpenSans-Bold.ttf");
			}
			else if (isItalic)
			{
				return new FontResolverInfo("OpenSans-Italic.ttf");
			}
			else
			{
				return new FontResolverInfo("OpenSans-Regular.ttf");
			}
		}
		return new FontResolverInfo("OpenSans-Regular.ttf"); //null;
	}

	//public async Task<byte[]> LoadFontData(string name)
	//{
	//	var sourceStream = await _httpClient.GetStreamAsync($"fonts/{name}");

	//	using (MemoryStream memoryStream = new())
	//	{
	//		sourceStream.CopyTo(memoryStream);
	//		return memoryStream.ToArray();
	//	}
	//}
}

