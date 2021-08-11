using System.Text;
using UglyToad.PdfPig;

namespace ConsoleApps
{
    public class PDFReader
    {
        public string LoadData()
        {
            StringBuilder str = new StringBuilder();
            using (PdfDocument document = PdfDocument.Open(@"C:\Users\DATIA-SOFT.DEV02\Desktop\Events.pdf"))
            {
                var pages = document.GetPages();
                foreach (var page in pages)
                {
                    str.Append(page.Text);
                }
            }
            return str.ToString();
        }
    }
}
