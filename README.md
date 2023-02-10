# Blazor.PDFSharpCode

Example of use of the [library PdfSharpCore](https://github.com/ststeiger/PdfSharpCore) with Blazor Server and Blazor Webassembly to create PDF docucments.

With Blazor Wasm, I included a .TTF font and loaded it via a service. I couldn't load this font from my CustomFontResolver class because in Wasm I am mono-thread. 

![sharppdf](https://user-images.githubusercontent.com/3845786/218074655-4afd9d7b-0d93-466d-acd7-9f80c7571d7b.gif)
