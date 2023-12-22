# Blazor (Server or Wasm) PDFSharpCode MigraDocCore

DEMO : https://tossnet.github.io/Blazor-PdfSharpCore/

Example of use of the [library PdfSharpCore](https://github.com/ststeiger/PdfSharpCore) with Blazor Server and Blazor Webassembly to create PDF docucments.

With Blazor Wasm, I included a .TTF font and loaded it via a service. I couldn't load this font from my CustomFontResolver class because in Wasm I am mono-thread. 

![order](https://github.com/tossnet/Blazor-PdfSharpCore/assets/3845786/5c88db77-2764-4b21-9b9f-5046c2372ce1)


![sharppdf](https://user-images.githubusercontent.com/3845786/218074655-4afd9d7b-0d93-466d-acd7-9f80c7571d7b.gif)
