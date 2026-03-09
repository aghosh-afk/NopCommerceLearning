using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Nop.Services.Events;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.DocumentVault.Infrastructure;

public class DocumentVaultAdminMenuConsumer : IConsumer<AdminMenuCreatedEvent>
{
    private readonly IActionContextAccessor _actionContextAccessor;
    private readonly IUrlHelperFactory _urlHelperFactory;

    public DocumentVaultAdminMenuConsumer(
        IActionContextAccessor actionContextAccessor,
        IUrlHelperFactory urlHelperFactory)
    {
        _actionContextAccessor = actionContextAccessor;
        _urlHelperFactory = urlHelperFactory;
    }

    public async Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
    {
        var root = eventMessage.RootMenuItem;

        var actionContext = _actionContextAccessor.ActionContext;

        if (actionContext == null)
            return;

        var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);

        var documentVaultMenu = new AdminMenuItem
        {
            SystemName = "DocumentVault",
            Title = "Document Vault",
            IconClass = "far fa-folder",
            Visible = true
        };

        documentVaultMenu.ChildNodes.Add(new AdminMenuItem
        {
            SystemName = "DocumentVault.Documents",
            Title = "Documents",
            Url = urlHelper.Action("List", "DocumentVaultAdmin", new { area = "Admin" }),
            IconClass = "far fa-file"
        });

        documentVaultMenu.ChildNodes.Add(new AdminMenuItem
        {
            SystemName = "DocumentVault.Upload",
            Title = "Upload",
            Url = urlHelper.Action("Upload", "DocumentVaultAdmin", new { area = "Admin" }),
            IconClass = "fas fa-upload"
        });

        documentVaultMenu.ChildNodes.Add(new AdminMenuItem
        {
            SystemName = "DocumentVault.Settings",
            Title = "Settings",
            Url = urlHelper.Action("Configure", "DocumentVaultSettings", new { area = "Admin" }),
            IconClass = "fas fa-cog"
        });

        root.ChildNodes.Add(documentVaultMenu);

        await Task.CompletedTask;
    }
}