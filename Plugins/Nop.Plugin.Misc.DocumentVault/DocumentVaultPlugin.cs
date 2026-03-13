using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Plugin.Misc.DocumentVault.Domain;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.ScheduleTasks;

namespace Nop.Plugin.Misc.DocumentVault;

public class DocumentVaultPlugin : BasePlugin, IMiscPlugin
{
    private readonly ILocalizationService _localizationService;
    private readonly ISettingService _settingService;
    private readonly IUrlHelperFactory _urlHelperFactory;
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IScheduleTaskService _scheduleTaskService;

    public DocumentVaultPlugin(
        ILocalizationService localizationService,
        ISettingService settingService,
        IUrlHelperFactory urlHelperFactory,
        IActionContextAccessor actionContextAccessor,
        IScheduleTaskService scheduleTaskService)
    {
        _localizationService = localizationService;
        _settingService = settingService;
        _urlHelperFactory = urlHelperFactory;
        _actionContextAccessor = actionContextAccessor;
        _scheduleTaskService = scheduleTaskService;
    }

    public override async Task InstallAsync()
    {
        await _settingService.SaveSettingAsync(new DocumentVaultSettings
        {
            IsEnable = true,
            BucketName = "",
            AccessKey = "",
            SecretKey = "",
            Endpoint = "",
        });
        var task = await _scheduleTaskService.GetTaskByTypeAsync(
        DocumentVaultDefaults.CleanupTask.TYPE);

        if (task == null)
        {
            await _scheduleTaskService.InsertTaskAsync(new Nop.Core.Domain.ScheduleTasks.ScheduleTask
            {
                Name = DocumentVaultDefaults.CleanupTask.NAME,
                Seconds = DocumentVaultDefaults.CleanupTask.PERIODINSECONDS,
                Type = DocumentVaultDefaults.CleanupTask.TYPE,
                Enabled = true,
                StopOnError = false
            });
        }

        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {
            ["Plugins.Misc.DocumentVault.Fields.Provider"] = "Storage Provider",
            ["Plugins.Misc.DocumentVault.Fields.AccessKey"] = "Access Key",
            ["Plugins.Misc.DocumentVault.Fields.SecretKey"] = "Secret Key",
            ["Plugins.Misc.DocumentVault.Fields.Endpoint"] = "Endpoint",
            ["Plugins.Misc.DocumentVault.Fields.Bucket"] = "Bucket Name",
            ["Plugins.Misc.DocumentVault.Upload"] = "Upload Document",
            ["Plugins.Misc.DocumentVault.Download"] = "Download",

            ["Plugins.Misc.DocumentVault.Configure"] = "Document Vault Settings",
            ["Plugins.Misc.DocumentVault.Fields.IsEnable"] = "Enable plugin",
            ["Plugins.Misc.DocumentVault.Fields.Endpoint"] = "MinIO Endpoint",
            ["Plugins.Misc.DocumentVault.Fields.AccessKey"] = "Access Key",
            ["Plugins.Misc.DocumentVault.Fields.SecretKey"] = "Secret Key",
            ["Plugins.Misc.DocumentVault.Fields.BucketName"] = "Bucket Name",
            ["Plugins.Misc.DocumentVault.Fields.UseSSL"] = "Use SSL",

            ["Plugins.Misc.DocumentVault.Menu"] = "Document Vault",
            ["Plugins.Misc.DocumentVault.Menu.Documents"] = "Documents",
            ["Plugins.Misc.DocumentVault.Menu.Settings"] = "Settings",

            ["Plugins.Misc.DocumentVault.List.Title"] = "Document Vault",
            ["Plugins.Misc.DocumentVault.Upload"] = "Upload Document",
            ["Plugins.Misc.DocumentVault.Download"] = "Download",
            ["Plugins.Misc.DocumentVault.NoDocuments"] = "No documents found",

            ["Plugins.Misc.DocumentVault.Fields.Title"] = "Title",
            ["Plugins.Misc.DocumentVault.Fields.Entity"] = "Entity",
            ["Plugins.Misc.DocumentVault.Fields.FileName"] = "File Name",
            ["Plugins.Misc.DocumentVault.Fields.Uploaded"] = "Uploaded On",
            ["Plugins.Misc.DocumentVault.Fields.Download"] = "Download"
        });
        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await _settingService.DeleteSettingAsync<DocumentVaultSettings>();

        var task = await _scheduleTaskService.GetTaskByTypeAsync(
            DocumentVaultDefaults.CleanupTask.TYPE);

        if (task != null)
            await _scheduleTaskService.DeleteTaskAsync(task);

        await _localizationService.DeleteLocaleResourcesAsync("Plugins.Misc.DocumentVault");

        await base.UninstallAsync();
    }

    public override string GetConfigurationPageUrl()
    {
        return "/Admin/DocumentVaultSettings/Configure";
    }
}
