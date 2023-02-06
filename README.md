# Blazor.PDFSharpCode

Example of use of the library PdfSharpCore with Blazor Server and Blazor Webassembly.

With Blazor Wasm, I included a .TTF font and loaded it via a service. I couldn't load this font from my CustomFontResolver class because in Wasm I am mono-thread. 
