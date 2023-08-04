namespace Share.PDF;

using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

using PdfSharpCore.Drawing.Layout;
using System.Text;

public static class Unicode
{
    private static PdfDocument? document;

    public static byte[] UnicodeSample()
    {
        //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        //var test = Encoding.GetEncoding("Windows-1252");
        //var test2 = Encoding.GetEncoding(1252);


        // Create Document with info
        document = new PdfDocument();
        Common.DocumentInfo(document, "Unicode Sample");

        // Set font encoding to unicode
        XPdfFontOptions options = new(PdfFontEncoding.Unicode);

        XFont font = new("Times New Roman", 12, XFontStyle.Regular, options);

        // Draw text in different languages
        for (int i = 0; i < texts.Length; i++)
        {
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            // HACK
            gfx.MUH = PdfFontEncoding.Unicode;
            XTextFormatter tf = new(gfx);
            tf.Alignment = XParagraphAlignment.Left;

            tf.DrawString(texts[i], font, XBrushes.Black,
            new XRect(100, 100, page.Width - 200, 600), XStringFormats.TopLeft);
        }


        PdfPage page2 = document.AddPage();
        XGraphics gfx2 = XGraphics.FromPdfPage(page2);
        XTextFormatter tf2 = new(gfx2);

        string allCharacters = GetAllCharactersInFont("Times New Roman");

        const int charactersPerLine = 50;
        string[] lines = SplitStringIntoLines(allCharacters, charactersPerLine);

        const int lineHeight = 15;
        XRect rect = new(40, 40, page2.Width - 80, page2.Height - 80);

        foreach (string line in lines)
        {
            gfx2.DrawString(line, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            rect = new XRect(rect.Left, rect.Top + lineHeight, rect.Width, rect.Height);
        }

        MemoryStream PdfStream = new();
        document.Save(PdfStream);

        return PdfStream.ToArray();
    }

    static readonly string[] texts = new string[]
   {
    // International version of the text in English.
    "English\n" +
    "PdfSharpCore ✅ is a .NET library for creating and processing PDF documents 'on the fly'. " +
    "The library is completely written in C# and based exclusively on safe, managed code. " +
    "PdfSharpCore offers two powerful abstraction levels to create and process PDF documents.\n" +
    "For drawing text, graphics, and images there is a set of classes which are modeled similar to the classes " +
    "of the name space System.Drawing of the .NET framework. With these classes it is not only possible to create " +
    "the content of PDF pages in an easy way, but they can also be used to draw in a window or on a printer.\n" +
    "Additionally PdfSharpCore completely models the structure elements PDF is based on. With them existing PDF documents " +
    "can be modified, merged, or split with ease.\n" +
    "The source code of PdfSharpCore is Open Source under the MIT license (http://en.wikipedia.org/wiki/MIT_License). " +
    "Therefore it is possible to use PdfSharpCore without limitations in non open source or commercial projects/products.",
    
    // German version.
    "German (deutsch)\n" +
    "PdfSharpCore ist eine .NET-Bibliothek zum Erzeugen und Verarbeiten von PDF-Dokumenten 'On the Fly'. " +
    "Die Bibliothek ist vollständig in C# geschrieben und basiert ausschließlich auf sicherem, verwaltetem Code. " +
    "PdfSharpCore bietet zwei leistungsstarke Abstraktionsebenen zur Erstellung und Verarbeitung von PDF-Dokumenten.\n" +
    "Zum Zeichnen von Text, Grafik und Bildern gibt es einen Satz von Klassen, die sehr stark an die Klassen " +
    "des Namensraums System.Drawing des .NET Frameworks angelehnt sind. Mit diesen Klassen ist es nicht " +
    "nur auf einfache Weise möglich, den Inhalt von PDF-Seiten zu gestalten, sondern sie können auch zum " +
    "Zeichnen in einem Fenster oder auf einem Drucker verwendet werden.\n" +
    "Zusätzlich modelliert PdfSharpCore vollständig die Stukturelemente, auf denen PDF basiert. Dadurch können existierende " +
    "PDF-Dokumente mit Leichtigkeit zerlegt, ergänzt oder umgebaut werden.\n" +
    "Der Quellcode von PdfSharpCore ist Open-Source unter der MIT-Lizenz (http://de.wikipedia.org/wiki/MIT-Lizenz). " +
    "Damit kann PdfSharpCore auch uneingeschränkt in Nicht-Open-Source- oder kommerziellen Projekten/Produkten eingesetzt werden.",
    
    // Greek version.
    // The text was translated by Babel Fish. We here in Germany have no idea what it means.
    "Greek (Translated with Babel Fish)\n" +
    "Το PdfSharpCore είναι βιβλιοθήκη δικτύου α. για τη δημιουργία και την επεξεργασία των εγγράφων PDF 'σχετικά με τη μύγα'. " +
    "Η βιβλιοθήκη γράφεται εντελώς γ # και βασίζεται αποκλειστικά εκτός από, διοικούμενος κώδικας. " +
    "Το PdfSharpCore προσφέρει δύο ισχυρά επίπεδα αφαίρεσης για να δημιουργήσει και να επεξεργαστεί τα έγγραφα PDF. " +
    "Για το κείμενο, τη γραφική παράσταση, και τις εικόνες σχεδίων υπάρχει ένα σύνολο κατηγοριών που διαμορφώνονται " +
    "παρόμοιος με τις κατηγορίες του διαστημικού σχεδίου συστημάτων ονόματος του. πλαισίου δικτύου. " +
    "Με αυτές τις κατηγορίες που είναι όχι μόνο δυνατό να δημιουργηθεί το περιεχόμενο των σελίδων PDF με έναν εύκολο " +
    "τρόπο, αλλά αυτοί μπορεί επίσης να χρησιμοποιηθεί για να επισύρει την προσοχή σε ένα παράθυρο ή σε έναν εκτυπωτή. " +
    "Επιπλέον PdfSharpCore διαμορφώνει εντελώς τα στοιχεία PDF δομών είναι βασισμένο. Με τους τα υπάρχοντα έγγραφα PDF " +
    "μπορούν να τροποποιηθούν, συγχωνευμένος, ή να χωρίσουν με την ευκολία. Ο κώδικας πηγής PdfSharpCore είναι ανοικτή πηγή " +
    "με άδεια MIT (http://en.wikipedia.org/wiki/MIT_License). Επομένως είναι δυνατό να χρησιμοποιηθεί PdfSharpCore χωρίς " +
    "προβλήματα στη μη ανοικτή πηγή ή τα εμπορικά προγράμματα/τα προϊόντα.",
    
    // Russian version (by courtesy of Alexey Kuznetsov).
    "Russian\n" +
    "PdfSharpCore это .NET библиотека для создания и обработки PDF документов 'налету'. " +
    "Библиотека полностью написана на языке C# и базируется исключительно на безопасном, управляемом коде. " +
    "PdfSharpCore использует два мощных абстрактных уровня для создания и обработки PDF документов.\n" +
    "Для рисования текста, графики, и изображений в ней используется набор классов, которые разработаны аналогично с" +
    "пакетом System.Drawing, библиотеки .NET framework. С помощью этих классов возможно не только создавать" +
    "содержимое PDF страниц очень легко, но они так же позволяют рисовать напрямую в окне приложения или на принтере.\n" +
    "Дополнительно PdfSharpCore имеет полноценные модели структурированных базовых элементов PDF. Они позволяют работать с существующим PDF документами " +
    "для изменения их содержимого, склеивания документов, или разделения на части.\n" +
    "Исходный код PdfSharpCore библиотеки это Open Source распространяемый под лицензией MIT (http://ru.wikipedia.org/wiki/MIT_License). " +
    "Теоретически она позволяет использовать PdfSharpCore без ограничений в не open source проектах или коммерческих проектах/продуктах.",
    
    // French version (by courtesy of Olivier Dalet).
    "French (Français)\n" +
    "PdfSharpCore est une librairie .NET permettant de créer et de traiter des documents PDF 'à la volée'. " +
    "La librairie est entièrement écrite en C# et exclusivement basée sur du code sûr et géré. " +
    "PdfSharpCore fournit deux puissants niveaux d'abstraction pour la création et le traitement des documents PDF.\n" +
    "Un jeu de classes, modélisées afin de ressembler aux classes du namespace System.Drawing du framework .NET, " +
    "permet de dessiner du texte, des graphiques et des images. Non seulement ces classes permettent la création du " +
    "contenu des pages PDF de manière aisée, mais elles peuvent aussi être utilisées pour dessiner dans une fenêtre ou pour l'imprimante.\n" +
    "De plus, PdfSharpCore modélise complètement les éléments structurels de PDF. Ainsi, des documents PDF existants peuvent être " +
    "facilement modifiés, fusionnés ou éclatés.\n" +
    "Le code source de PdfSharpCore est Open Source sous licence MIT (http://fr.wikipedia.org/wiki/Licence_MIT). " +
    "Il est donc possible d'utiliser PdfSharpCore sans limitation aucune dans des projets ou produits non Open Source ou commerciaux.",
    
    // Dutch version (by giCalle)
    "Dutch\n" +
    "PdfSharpCore is een .NET bibliotheek om PDF documenten te creëren en te verwerken. " +
    "De bibliotheek is volledig geschreven in C# en gebruikt uitsluitend veilige, 'managed code'. " +
    "PdfSharpCore biedt twee krachtige abstractie niveaus aan om PDF documenten te maken en te verwerken.\n" +
    "Om tekst, beelden en foto's weer te geven zĳn er een reeks klassen beschikbaar, gemodelleerd naar de klassen " +
    "uit de 'System.Drawing' naamruimte van het .NET framework. Met behulp van deze klassen is het niet enkel mogelĳk " +
    "om de inhoud van PDF pagina's aan te maken op een eenvoudige manier, maar ze kunnen ook gebruikt worden om dingen " +
    "weer te geven in een venster of naar een printer. Daarbovenop implementeert PdfSharpCore de volledige elementen structuur " +
    "waarop PDF is gebaseerd. Hiermee kunnen bestaande PDF documenten eenvoudig aangepast, samengevoegd of opgesplitst worden.\n" +
    "De broncode van PdfSharpCore is opensource onder een MIT licentie (http://nl.wikipedia.org/wiki/MIT-licentie). " +
    "Daarom is het mogelĳk om PdfSharpCore te gebruiken zonder beperkingen in niet open source of commerciële projecten/producten.",
    
    // Danish version (by courtesy of Mikael Lyngvig).
    "Danish (Dansk)\n" +
    "PdfSharpCore er et .NET bibliotek til at dynamisk lave og behandle PDF dokumenter. " +
    "Biblioteket er skrevet rent i C# og indeholder kun sikker, managed kode. " +
    "PdfSharpCore tilbyder to stærke abstraktionsniveauer til at lave og behandle PDF dokumenter. " +
    "Til at tegne tekst, grafik og billeder findes der et sæt klasser som er modelleret ligesom klasserne i navnerummet " +
    "System.Drawing i .NET biblioteket. Med disse klasser er det ikke kun muligt at udforme indholdet af PDF siderne på en " +
    "nem måde – de kan også bruges til at tegne i et vindue eller på en printer. " +
    "Derudover modellerer PdfSharpCore fuldstændigt strukturelementerne som PDF er baseret på. " +
    "Med dem kan eksisterende PDF dokumenter nemt modificeres, sammenknyttes og adskilles. " +
    "Kildekoden til PdfSharpCore er Open Source under MIT licensen (http://da.wikipedia.org/wiki/MIT-Licensen). " +
    "Derfor er det muligt at bruge PdfSharpCore uden begrænsninger i både lukkede og kommercielle projekter og produkter.",
    
    // Portuguese version (by courtesy of Luís Rodrigues).
    "Portuguese (Português)\n" +
    "PdfSharpCore é uma biblioteca .NET para a criação e processamento de documentos PDF 'on the fly'." +
    "A biblioteca é completamente escrita em C# e baseada exclusivamente em código gerenciado e seguro. " +
    "O PdfSharpCore oferece dois níveis de abstração poderosa para criar e processar documentos PDF.\n" +
    "Para desenhar texto, gráficos e imagens, há um conjunto de classes que são modeladas de forma semelhante às classes " +
    "do espaço de nomes System.Drawing do framework .NET. Com essas classes não só é possível criar " +
    "o conteúdo das páginas PDF de uma maneira fácil, mas podem também ser usadas para desenhar numa janela ou numa impressora.\n" +
    "Adicionalmente, o PdfSharpCore modela completamente a estrutura dos elementos em que o PDF é baseado. Com eles, documentos PDF existentes " +
    "podem ser modificados, unidos, ou divididos com facilidade.\n" +
    "O código fonte do PdfSharpCore é Open Source sob a licença MIT (http://en.wikipedia.org/wiki/MIT_License). " +
    "Por isso, é possível usar o PdfSharpCore sem limitações em projetos/produtos não open source ou comerciais.",
    
    // Polish version (by courtesy of Krzysztof Jędryka)
    "Polish (polski)\n" +
    "PdfSharpCore jest  biblioteką .NET umożliwiającą tworzenie i przetwarzanie dokumentów PDF 'w locie'. " +
    "Biblioteka ta została stworzona w całości w języku C# i jest oparta wyłącznie na bezpiecznym i zarządzanym kodzie. " +
    "PdfSharpCore oferuje dwa rozbudowane poziomy abstrakcji do tworzenia i przetwarzania dokumentów PDF.\n" +
    "Do rysowania tekstu, grafiki i obrazów stworzono zbiór klas projektowanych na wzór klas przestrzeni nazw System.Drawing" +
    "platformy .NET. Z pomocą tych klas można tworzyć w wygodny sposób nie tylko zawartość stron dokumentu PDF, ale można również" +
    "rysować w oknie programu lub generować wydruki.\n" +
    "Ponadto PdfSharpCore w pełni odwzorowuje strukturę elementów na których opiera się format pliku PDF." +
    "Używając tych elementów, dokumenty PDF można modyfikować, łączyć lub dzielić z łatwością.\n" +
    "Kod źródłowy PdfSharpCore jest dostępny na licencji Open Source MIT (http://pl.wikipedia.org/wiki/Licencja_MIT). " +
    "Zatem można korzystać z PdfSharpCore bez żadnych ograniczeń w projektach niedostępnych dla społeczności Open Source lub komercyjnych.",

    // 
    "Chiness (Deepl)\n" +
    "PdfSharpCore 是一个 .NET 库，用于 '即时 '创建和处理 PDF" +
    "文档的.NET 库。该库完全由 C# 编写，并" +
    "完全基于安全的托管代码。PdfSharpCore 提供了两个" +
    "强大的抽象级别来创建和处理 PDF 文档。" +
    "对于绘制文本、图形和图像，有一组类" +
    "这些类的模型与.NET框架的名称空间System.Drawing的类相似。" +
    "类。使用这些类不仅可以" +
    "创建 PDF 页面的内容，而且还可" +
    "在窗口或打印机上绘图。" +
    "此外，PdfSharpCore 还能完全模拟 PDF 所基于的结构元素。" +
    "PDF 的基础。有了它们，现有的 PDF 文档可以轻松修改、合并或拆分、" +
    "合并或分割。" +
    "PdfSharpCore 的源代码在 MIT" +
    "许可证（http://en.wikipedia.org/wiki/MIT_License）下开放源代码。因此" +
    "因此，PdfSharpCore可以在非开源或商业项目/产品中无限制地使用。" +
    "或商业项目/产品。\n" +
    "通过www.DeepL.com/Translator（免费版）翻译"
    };


    private static string GetAllCharactersInFont(string fontName)
    {
        StringBuilder allCharacters = new();
        foreach (char c in Enumerable.Range(char.MinValue, char.MaxValue))
        {
            //if (XFontMetrics.GetGlyphIndex(fontName, c) > 0)
            if (c!=0 && c!=9 && c!=10 && c!=13 && c!=32 && c!=127)
                allCharacters.Append(c);
        }
        return allCharacters.ToString();
    }

    private static string[] SplitStringIntoLines(string input, int charactersPerLine)
    {
        int length = input.Length;
        int numLines = (length + charactersPerLine - 1) / charactersPerLine;
        string[] lines = new string[numLines];
        for (int i = 0; i < numLines; i++)
        {
            int startIndex = i * charactersPerLine;
            int remainingLength = length - startIndex;
            int lineLength = Math.Min(remainingLength, charactersPerLine);
            lines[i] = input.Substring(startIndex, lineLength);
        }
        return lines;
    }
}
