namespace Blazor.Wasm;

using PdfSharpCore.Fonts;


public class CustomFontResolver : IFontResolver
{
	private HttpClient _httpClient;
	private byte[] _fontLoaded;

	public CustomFontResolver(HttpClient httpClient,
						byte[] FontLoaded		)
	{
		_httpClient = httpClient;
		_fontLoaded = FontLoaded;
	}

	public string DefaultFontName => throw new NotImplementedException();

	public byte[] GetFont(string faceName)
	{
		return _fontLoaded;
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
		return null;

		//if (isBold)
		//{
		//	if (isItalic)
		//		return new FontResolverInfo("Calibri#bi");
		//	return new FontResolverInfo("Calibri#b");
		//}
		//if (isItalic)
		//	return new FontResolverInfo("Calibri#i");
		//return new FontResolverInfo("Calibri#");
	}
}

