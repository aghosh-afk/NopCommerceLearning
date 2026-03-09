using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.DocumentVault.Domain;
using Nop.Plugin.Misc.DocumentVault.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Controllers;

namespace Nop.Plugin.Misc.DocumentVault.Controllers;

public class DocumentVaultSettingsController : BaseAdminController
{
    private readonly ISettingService _settingService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;

    public DocumentVaultSettingsController(
        ISettingService settingService,
        ILocalizationService localizationService,
        INotificationService notificationService)
    {
        _settingService = settingService;
        _localizationService = localizationService;
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Configure()
    {
        var settings = _settingService.LoadSetting<DocumentVaultSettings>();

        var model = new ConfigurationModel
        {
            IsEnable = settings.IsEnable,
            Endpoint = settings.Endpoint ?? " ",
            AccessKey = settings.AccessKey ?? " ",
            SecretKey = settings.SecretKey ?? " ",
            BucketName = settings.BucketName ?? " ",
            UseSSL = settings.UseSSL
        };

        return View("~/Plugins/Misc.DocumentVault/Views/DocumentVault/Configure.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        var settings = _settingService.LoadSetting<DocumentVaultSettings>();

        settings.IsEnable = model.IsEnable;
        settings.Endpoint = model.Endpoint;
        settings.AccessKey = model.AccessKey;
        settings.SecretKey = model.SecretKey;
        settings.BucketName = model.BucketName;
        settings.UseSSL = model.UseSSL;

        _settingService.SaveSetting(settings);

        _notificationService.SuccessNotification(
            await _localizationService.GetResourceAsync("Admin.Plugins.Saved")
        );

        return RedirectToAction("Configure");
    }
}