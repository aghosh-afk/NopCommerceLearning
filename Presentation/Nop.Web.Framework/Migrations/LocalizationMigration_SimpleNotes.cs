using FluentMigrator;
using Nop.Core.Infrastructure;
using Nop.Data;
using Nop.Data.Migrations;
using Nop.Services.Localization;
using System.Collections.Generic;

namespace Nop.Web.Framework.Migrations
{
    [NopUpdateMigration("2026-02-05 00:00:00", "4.90", UpdateMigrationType.Localization)]
    public class LocalizationMigration_SimpleNotes : MigrationBase
    {
        public override void Up()
        {
            if (!DataSettingsManager.IsDatabaseInstalled())
                return;

            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

            localizationService.AddOrUpdateLocaleResourceAsync(
                new Dictionary<string, string>
                {
                    // ======= MENU & PAGE TITLES =======
                    { "Admin.SimpleNote", "Simple Notes" },
                    { "Admin.SimpleNote.List", "Simple Notes" },

                    // ======= GRID COLUMNS =======
                    { "Admin.SimpleNote.Title", "Title" },
                    { "Admin.SimpleNote.Description", "Description" },
                    { "Admin.SimpleNote.CreatedOn", "Created On" },

                    // ======= FILTERS (MATCH YOUR INDEX.CSHTML) =======
                    { "Admin.SimpleNote.SearchTitle", "Search by Title" },

                    // ======= FORM FIELD LABELS =======
                    { "Admin.SimpleNote.Fields.Title", "Note title" },
                    { "Admin.SimpleNote.Fields.Description", "Note description" },

                    // ======= ACTION BUTTONS (YOU ASKED TO ADD THESE) =======
                    { "Admin.SimpleNote.Create", "Create Note" },
                    { "Admin.SimpleNote.Edit", "Edit Note" },
                    { "admin.Delete", "Delete Note" },

                    // ======= COMMON ADMIN BUTTONS (USED IN YOUR PAGE) =======
                    { "Admin.Search", "Search" },
                    { "Admin.Clear", "Clear" },
                    { "Admin.Actions", "Actions" },
                    { "Admin.NoRecordsFound", "No records found" },
                    { "Admin.AreYouSure", "Are you sure?" },
                    { "admin.Deleted", "Deleted successfully" },

                    // ======= PAGINATION BUTTONS =======
                    { "Admin.Paging.Prev", "Prev" },
                    { "Admin.Paging.Next", "Next" },

                    // ======= SUCCESS MESSAGES =======
                    { "Admin.SimpleNote.Added", "Note added successfully." },
                    { "Admin.SimpleNote.Updated", "Note updated successfully." },
                    { "Admin.SimpleNote.Deleted", "Note deleted successfully." }
                }
            ).Wait();   // REQUIRED in nopCommerce migrations
        }

        public override void Down()
        {
            // Leave empty as per nopCommerce practice
        }
    }
}
