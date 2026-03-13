using System;
using System.Linq;
using FluentMigrator;
using global::Nop.Core.Domain.Customers;
using global::Nop.Core.Domain.Security;
using global::Nop.Data;
using global::Nop.Data.Migrations;
using Nop.Plugin.Misc.DocumentVault.Infrastructure;

namespace Nop.Plugin.Misc.DocumentVault.Data;

[NopMigration("2026/03/09 00:00:00", "DocumentVault Permission Migration", MigrationProcessType.Installation)]
public class DocumentVaultPermissionMigration : Migration
{
    private readonly INopDataProvider _dataProvider;

    public DocumentVaultPermissionMigration(INopDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    private void InsertPermission(CustomerRole role, PermissionRecord permission)
    {
        if (role == null || permission == null)
            return;

        _dataProvider.InsertEntity(new PermissionRecordCustomerRoleMapping
        {
            CustomerRoleId = role.Id,
            PermissionRecordId = permission.Id
        });
    }

    public override void Up()
    {
        var adminRole = _dataProvider
            .GetTable<CustomerRole>()
            .FirstOrDefault(x => x.SystemName == NopCustomerDefaults.AdministratorsRoleName);

        var permissionTable = _dataProvider.GetTable<PermissionRecord>();

        CreatePermission(
            "Admin area. Manage Document Vault",
            DocumentVaultPermissionDefaults.MANAGEDOCUMENTS,
            adminRole,
            permissionTable
        );

        CreatePermission(
            "Admin area. View Document Vault",
            DocumentVaultPermissionDefaults.VIEWDOCUMENTS,
            adminRole,
            permissionTable
        );

        CreatePermission(
            "Admin area. Create Document Vault",
            DocumentVaultPermissionDefaults.CREATEDOCUMENTS,
            adminRole,
            permissionTable
        );

        CreatePermission(
            "Admin area. Edit Document Vault",
            DocumentVaultPermissionDefaults.EDITDOCUMENTS,
            adminRole,
            permissionTable
        );

        CreatePermission(
            "Admin area. Delete Document Vault",
            DocumentVaultPermissionDefaults.DELETEDOCUMENTS,
            adminRole,
            permissionTable
        );
    }

    private void CreatePermission(string name, string systemName, CustomerRole adminRole, IQueryable<PermissionRecord> table)
    {
        if (table.Any(p => p.SystemName == systemName))
            return;

        var permission = new PermissionRecord
        {
            Name = name,
            SystemName = systemName,
            Category = "DocumentVault"
        };

        _dataProvider.InsertEntity(permission);

        InsertPermission(adminRole, permission);
    }

    public override void Down()
    {
        var permissionSystemNames = new[]
        {
        DocumentVaultPermissionDefaults.MANAGEDOCUMENTS,
        DocumentVaultPermissionDefaults.VIEWDOCUMENTS,
        DocumentVaultPermissionDefaults.CREATEDOCUMENTS,
        DocumentVaultPermissionDefaults.EDITDOCUMENTS,
        DocumentVaultPermissionDefaults.DELETEDOCUMENTS
    };

        var permissionTable = _dataProvider.GetTable<PermissionRecord>();

        foreach (var systemName in permissionSystemNames)
        {
            var permission = permissionTable.FirstOrDefault(p => p.SystemName == systemName);

            if (permission == null)
                continue;

            // Delete role mappings first
            var mappings = _dataProvider.GetTable<PermissionRecordCustomerRoleMapping>()
                .Where(x => x.PermissionRecordId == permission.Id)
                .ToList();

            foreach (var mapping in mappings)
                _dataProvider.DeleteEntity(mapping);

            // Delete permission
            _dataProvider.DeleteEntity(permission);
        }
    }
}