using System;
using System;
using LinqToDB.Mapping;
using Nop.Core;

namespace Nop.Plugin.Misc.DocumentVault.Domain;

public class DocumentVaultFile : BaseEntity
{
    public string? Title { get; set; }

    public string? FileName { get; set; }

    public string? ObjectKey { get; set; }

    public int EntityId { get; set; }

    [Column("EntityType")]
    public DocumentEntityType EntityType { get; set; }

    public long FileSize { get; set; }

    public DateTime UploadedOnUtc { get; set; }
}
