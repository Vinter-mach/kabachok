using Amazon.S3;
using Amazon.S3.Model;

namespace Cs_backend.Infrastructure;

public class CloudFileGetter
{
    public static (string accessKey, string secretKey, string backet, string endpoint) GetEnvironmentVariables()
    {
        if (Environment.GetEnvironmentVariable("ACCESS_KEY") == null)
        {
            DotNetEnv.Env.Load();
        }

        return (Environment.GetEnvironmentVariable("ACCESS_KEY"),
            Environment.GetEnvironmentVariable("SECRET_KEY"),
            Environment.GetEnvironmentVariable("BACKET_NAME"),
            Environment.GetEnvironmentVariable("ENDPOINT"))!;
    }

    public static async Task<List<string>> GetTemporaryLinks(string endpoint, string bucketName, string accessKey,
        string secretKey, string prefix, DateTime timeout)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = endpoint,
            ForcePathStyle = true
        };

        var client = new AmazonS3Client(accessKey, secretKey, config);

        var request = new ListObjectsV2Request
        {
            BucketName = bucketName,
            Prefix = prefix
        };

        var matchingKeys = new List<string>();
        ListObjectsV2Response response;

        do
        {
            response = await client.ListObjectsV2Async(request);

            matchingKeys.AddRange(response.S3Objects.Select(s3Object => s3Object.Key));

            request.ContinuationToken = response.NextContinuationToken;
        } while (response.IsTruncated);

        var urls = new List<string>();
        foreach (var key in matchingKeys)
        {
            var urlRequest = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = key,
                Expires = timeout,
                Verb = HttpVerb.GET
            };

            urls.Add(await client.GetPreSignedURLAsync(urlRequest));
        }

        return urls;
    }
}