
using PdfService.Interfaces;
using Microsoft.Playwright;

namespace PdfService.Services
{
    public class PdfService : IPdfService
    {
        public async Task<byte[]> CreatePdfAsync(string html)
        {
            using var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            });

            var page = await browser.NewPageAsync();
            await page.GotoAsync(html);

            var pdfBytes = await page.PdfAsync(new PagePdfOptions
            {
                Format = "A4"
            });

            await page.CloseAsync();
            return pdfBytes;
        }
    }
}
