using Amazon.S3;
using Amazon.S3.Transfer;
using PdfService.Constants;
using PdfService.Interfaces;

namespace PdfService.Services
{
    public class AmazongS3Services(IAmazonS3 s3Client) : IAmazonS3Services
    {
        public async Task<string> UploadPdf(byte[] pdfBytes, string fileName)
        {
            var transferUtility = new TransferUtility(s3Client);
            var bucket = Environment.GetEnvironmentVariable(AppConstants.BUCKET);

            using (var memoryStream = new MemoryStream(pdfBytes))
            {
                await transferUtility.UploadAsync(memoryStream, bucket, fileName);
            }
            return $@"https://s3-ap-southeast-2.amazonaws.com/{bucket}/{fileName}";
        }
    }
}