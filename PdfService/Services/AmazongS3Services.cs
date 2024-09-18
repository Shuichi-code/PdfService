using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using PdfService.Constants;
using PdfService.Interfaces;

namespace PdfService.Services
{
    public class AmazongS3Services(IAmazonS3 s3Client) : IAmazonS3Services
    {
        public async Task<string> UploadPdf(byte[] pdfBytes, string fileName)
        {
            var bucketName = Environment.GetEnvironmentVariable(AppConstants.BUCKET);

            bool bucketExists = await AmazonS3Util.DoesS3BucketExistV2Async(s3Client, bucketName);
            if (!bucketExists)
            {
                throw new Exception($"Bucket {bucketName} does not exist.");
            }

            // Adding 'mqb' subfolder to the key
            string key = $"mqb/{fileName}";

            using (var memoryStream = new MemoryStream(pdfBytes))
            {
                var request = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = key,
                    InputStream = memoryStream
                };

                await s3Client.PutObjectAsync(request);
            }

            // Use the AWS SDK to generate the pre-signed URL
            string url = s3Client.GetPreSignedURL(new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return url;
        }
    }
}