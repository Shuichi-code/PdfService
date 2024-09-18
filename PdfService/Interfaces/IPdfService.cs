namespace PdfService.Interfaces
{
    public interface IPdfService
    {
        Task<byte[]> CreatePdfAsync(string s3Url);
    }
}
