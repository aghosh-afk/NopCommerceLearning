using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using Nop.Core.Domain.Notes;
using Nop.Data.Mapping.Builders.Notes;
using static LinqToDB.Reflection.Methods.LinqToDB;

namespace Nop.Data.Migrations.Installation;

[NopUpdateMigration("2026-02-03 12:30:00", "4.90", UpdateMigrationType.Data)]
public class CreateSimpleNoteTable : Migration
{
    public override void Up()
    {
        // Fix: Call Create.Table and use SimpleNoteMap to map the entity
        Create.Table(nameof(SimpleNote))
            .WithColumn(nameof(SimpleNote.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(SimpleNote.Title)).AsString(255).NotNullable()
            .WithColumn(nameof(SimpleNote.Description)).AsString(int.MaxValue).Nullable()
            .WithColumn(nameof(SimpleNote.CreatedOnUtc)).AsDateTime().NotNullable();
    }

    public override void Down()
    {
        Delete.Table(nameof(SimpleNote));
    }
}
