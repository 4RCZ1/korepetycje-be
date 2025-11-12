using Amazon.S3;
using Amazon.S3.Model;
using Services.Interfaces;

namespace FileStorage;

public class S3Client : IFileStorageClient
{
    public S3Client(string bucketName)
    {
        _bucketName = bucketName;
    }

    public string GetDownloadUrl(string filePath)
    {
        return _s3.GetPreSignedURL(new GetPreSignedUrlRequest
        {
            Verb = HttpVerb.GET,
            BucketName = _bucketName,
            Key = filePath,
            Expires = GetUrlExpirationTime(),
        });
    }

    public string GetUploadUrl(string filePath)
    {
        return _s3.GetPreSignedURL(new GetPreSignedUrlRequest
        {
            Verb = HttpVerb.PUT,
            BucketName = _bucketName,
            Key = filePath,
            Expires = GetUrlExpirationTime(),
        });
    }

    private static DateTime GetUrlExpirationTime()
    {
        return DateTime.UtcNow.AddMinutes(3);
    }

    private readonly string _bucketName;
    private readonly AmazonS3Client _s3 = new();
}
