using FluentMigrator;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.DocumentVault.Domain;

namespace Nop.Plugin.Misc.DocumentVault.Data;

[NopMigration("2026/03/09 10:00:00", "DocumentVault table", MigrationProcessType.Installation)]
public class DocumentVaultMigration : Migration
{
    public override void Up()
    {
        if (!Schema.Table("DocumentVaultFile").Exists())
        {
            Create.Table("DocumentVaultFile")
            .WithColumn(nameof(DocumentVaultFile.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(DocumentVaultFile.Title)).AsString(500).Nullable()
            .WithColumn(nameof(DocumentVaultFile.FileName)).AsString(500).NotNullable()
            .WithColumn(nameof(DocumentVaultFile.ObjectKey)).AsString(500).NotNullable()
            .WithColumn(nameof(DocumentVaultFile.EntityType)).AsInt32().NotNullable()
            .WithColumn(nameof(DocumentVaultFile.EntityId)).AsInt32().NotNullable()
            .WithColumn(nameof(DocumentVaultFile.FileSize)).AsInt64().NotNullable()
            .WithColumn(nameof(DocumentVaultFile.UploadedOnUtc)).AsDateTime().NotNullable();
        }
    }

    public override void Down()
    {
        Delete.Table(nameof(DocumentVaultFile));
    }
}
