using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDF_merger
{
    public class PdfImageCreator
    {
        public void CreatePdf(IReadOnlyList<ImageItem> images, string outputPath, Action<int, int> progressCallback = null)
        {
            if (images == null || images.Count == 0)
                throw new InvalidOperationException("No images to convert.");

            var document = new PdfDocument();
            document.Info.Title = "Image to PDF";

            for (int i = 0; i < images.Count; i++)
            {
                var filePath = images[i].FilePath;

                using (var sysImage = Image.FromFile(filePath))
                using (var xImage = XImage.FromFile(filePath))
                {
                    double dpiX = sysImage.HorizontalResolution <= 0 ? 96.0 : sysImage.HorizontalResolution;
                    double dpiY = sysImage.VerticalResolution <= 0 ? 96.0 : sysImage.VerticalResolution;

                    double pageWidth = sysImage.Width * 72.0 / dpiX;
                    double pageHeight = sysImage.Height * 72.0 / dpiY;

                    PdfPage page = document.AddPage();
                    page.Width = pageWidth;
                    page.Height = pageHeight;

                    using (XGraphics gfx = XGraphics.FromPdfPage(page))
                    {
                        gfx.DrawImage(xImage, 0, 0, page.Width, page.Height);
                    }
                }

                progressCallback?.Invoke(i + 1, images.Count);
            }

            document.Save(outputPath);
        }
    }
}
