using Minio;
using Minio.DataModel.Args;
using Nop.Plugin.Misc.DocumentVault.Domain;
using Nop.Services.Configuration;

namespace Nop.Plugin.Misc.DocumentVault.Services;

public class MinioDocumentStorageService : IDocumentStorageService
{
    private readonly IMinioClient? _client;
    private readonly DocumentVaultSettings _settings;

    public MinioDocumentStorageService(ISettingService settingService)
    {
        _settings = settingService.LoadSetting<DocumentVaultSettings>();

        if (string.IsNullOrWhiteSpace(_settings.Endpoint) ||
            string.IsNullOrWhiteSpace(_settings.AccessKey) ||
            string.IsNullOrWhiteSpace(_settings.SecretKey) ||
            string.IsNullOrWhiteSpace(_settings.BucketName))
            return;

        _client = new MinioClient()
            .WithEndpoint(_settings.Endpoint)
            .WithCredentials(_settings.AccessKey, _settings.SecretKey)
            .Build();
    }

    private void ValidateClient()
    {
        if (_client == null)
            throw new InvalidOperationException("MinIO is not configured. Configure Document Vault settings.");
    }

    public async Task<string> UploadAsync(Stream stream, string fileName)
    {
        ValidateClient();

        var objectName = $"{Guid.NewGuid()}_{fileName}";

        var size = stream.CanSeek ? stream.Length : -1;

        var bucketExists = await _client!.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_settings.BucketName));

        if (!bucketExists)
        {
            await _client.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_settings.BucketName));
        }

        await _client.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(size));

        return objectName;
    }

    public async Task<string> GetPresignedUrlAsync(string objectKey)
    {
        ValidateClient();

        return await _client!.PresignedGetObjectAsync(
            new PresignedGetObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(objectKey)
                .WithExpiry(3600));
    }

    public async Task<Stream> DownloadAsync(string objectKey)
    {
        ValidateClient();

        var memory = new MemoryStream();

        var args = new GetObjectArgs()
            .WithBucket(_settings.BucketName)
            .WithObject(objectKey)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(memory);
            });

        await _client!.GetObjectAsync(args);

        memory.Position = 0;

        return memory;
    }

    public async Task DeleteAsync(string objectKey)
    {
        ValidateClient();

        await _client!.RemoveObjectAsync(
            new RemoveObjectArgs()
                .WithBucket(_settings.BucketName)
                .WithObject(objectKey));
    }
}