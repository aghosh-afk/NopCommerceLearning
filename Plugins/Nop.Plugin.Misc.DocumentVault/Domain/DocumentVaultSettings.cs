using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.DocumentVault.Domain;

public class DocumentVaultSettings : ISettings
{
    public bool IsEnable { get; set; }

    public string? Endpoint { get; set; }

    public string? AccessKey { get; set; }

    public string? SecretKey { get; set; }

    public string? BucketName { get; set; }

    public bool UseSSL { get; set; }
}