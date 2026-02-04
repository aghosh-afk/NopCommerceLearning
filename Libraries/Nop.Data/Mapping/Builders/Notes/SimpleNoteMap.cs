using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Notes;

namespace Nop.Data.Mapping.Builders.Notes;

public class SimpleNoteMap : NopEntityBuilder<SimpleNote>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(SimpleNote.Id))
            .AsInt32().PrimaryKey().Identity()

            .WithColumn(nameof(SimpleNote.Title))
            .AsString(400).NotNullable()

            .WithColumn(nameof(SimpleNote.Description))
            .AsString(int.MaxValue).Nullable()

            .WithColumn(nameof(SimpleNote.CreatedOnUtc))
            .AsDateTime2().NotNullable();
    }
}
