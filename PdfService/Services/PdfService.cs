
using PdfService.Interfaces;
using Microsoft.Playwright;

namespace PdfService.Services
{
    public class PdfService : IPdfService
    {
        public async Task<byte[]> CreatePdfAsync(string s3Url)
        {
            using var playwright = await Playwright.CreateAsync();
            IBrowser browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            IPage page = await browser.NewPageAsync();
            await page.GotoAsync(s3Url);

            // Inject CSS to force exact color rendering
            await page.AddStyleTagAsync(new PageAddStyleTagOptions
            {
                Content = "body { -webkit-print-color-adjust: exact; }"
            });

            byte[] pdfBytes = await page.PdfAsync(new PagePdfOptions
            {
                Format = "A4",
                Path = "page.pdf"
            });

            await page.CloseAsync();
            await browser.CloseAsync();
            return pdfBytes;
        }
    }
}
