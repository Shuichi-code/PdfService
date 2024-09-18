namespace PdfService.Interfaces
{
    public interface IAmazonS3Services
    {
        Task<string> UploadPdf(byte[] pdfBytes, string fileName);
    }
}
