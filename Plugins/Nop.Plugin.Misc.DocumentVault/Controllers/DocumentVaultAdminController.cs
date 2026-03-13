using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.DocumentVault.Domain;
using Nop.Plugin.Misc.DocumentVault.Infrastructure;
using Nop.Plugin.Misc.DocumentVault.Models;
using Nop.Plugin.Misc.DocumentVault.Services;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Controllers;

namespace Nop.Plugin.Misc.DocumentVault.Controllers;

public class DocumentVaultAdminController : BaseAdminController
{
    private readonly IDocumentService _documentService;
    private readonly IDocumentStorageService _storageService;
    private readonly INotificationService _notificationService;
    private readonly ILocalizationService _localizationService;
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;
    private readonly IPermissionService _permissionService;

    public DocumentVaultAdminController(
        IDocumentService documentService,
        IDocumentStorageService storageService,
        INotificationService notificationService,
        ILocalizationService localizationService,
        ICustomerService customerService,
        IProductService productService,
        IOrderService orderService,
        IPermissionService permissionService)
    {
        _documentService = documentService;
        _storageService = storageService;
        _notificationService = notificationService;
        _localizationService = localizationService;

        _customerService = customerService;
        _productService = productService;
        _orderService = orderService;
        _permissionService = permissionService;
    }

    public async Task<IActionResult> List()
    {
        if (!await _permissionService.AuthorizeAsync(DocumentVaultPermissionDefaults.VIEWDOCUMENTS))
            return AccessDeniedView();

        var entities = await _documentService.GetAllAsync();

        var model = new DocumentListModel();

        foreach (var item in entities)
        {
            model.Documents.Add(new DocumentModel
            {
                Id = item.Id,
                Title = item.Title,
                FileName = item.FileName,
                EntityType = item.EntityType,
                EntityId = item.EntityId,
                FileSize = item.FileSize,
                UploadedOnUtc = item.UploadedOnUtc
            });
        }

        return View("~/Plugins/Misc.DocumentVault/Views/DocumentVault/List.cshtml", model);
    }

    public async Task<IActionResult> Upload()
    {
        if (!await _permissionService.AuthorizeAsync(DocumentVaultPermissionDefaults.CREATEDOCUMENTS))
            return AccessDeniedView();

        var model = new DocumentModel();

        model.AvailableEntityTypes = Enum.GetValues(typeof(DocumentEntityType))
        .Cast<DocumentEntityType>()
        .Select(x => new SelectListItem
        {
            Value = ((int)x).ToString(),
            Text = x.ToString()
        }).ToList();

        return View("~/Plugins/Misc.DocumentVault/Views/DocumentVault/Upload.cshtml", model);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(DocumentModel model)
    {
        if (!await _permissionService.AuthorizeAsync(DocumentVaultPermissionDefaults.VIEWDOCUMENTS))
            return AccessDeniedView();

        if (!ModelState.IsValid)
            return View("~/Plugins/Misc.DocumentVault/Views/DocumentVault/Upload.cshtml", model);

        if (model.File == null)
        {
            _notificationService.ErrorNotification("File is required");
            return View("~/Plugins/Misc.DocumentVault/Views/DocumentVault/Upload.cshtml", model);
        }

        using var stream = model.File.OpenReadStream();

        var path = DocumentPathBuilder.BuildPath(
            model.EntityType.ToString(),
            model.EntityId,
            model.File.FileName);

        var objectKey = await _storageService.UploadAsync(stream, path);

        var file = new DocumentVaultFile
        {
            Title = model.Title ?? model.File.FileName,
            FileName = model.File.FileName,
            ObjectKey = objectKey,
            EntityType = model.EntityType,
            EntityId = model.EntityId,
            FileSize = model.File.Length,
            UploadedOnUtc = DateTime.UtcNow
        };

        await _documentService.InsertAsync(file);

        _notificationService.SuccessNotification(
            await _localizationService.GetResourceAsync("Admin.Common.DataSaved")
        );

        return RedirectToAction("List");
    }

    public async Task<IActionResult> Download(int id)
    {
        var file = await _documentService.GetByIdAsync(id);

        if (file == null || string.IsNullOrEmpty(file.ObjectKey))
            return NotFound();

        var url = await _storageService.GetPresignedUrlAsync(file.ObjectKey);

        return Redirect(url);
    }

    [HttpGet]
    public async Task<IActionResult> GetEntities(DocumentEntityType entityType)
    {
        var result = new List<object>();

        switch (entityType)
        {
            case DocumentEntityType.Customer:

                var customers = await _customerService.GetAllCustomersAsync();

                result = customers
                    .Take(50)
                    .Select(x => new
                    {
                        id = x.Id,
                        name = x.Email
                    }).ToList<object>();

                break;

            case DocumentEntityType.Product:

                var products = await _productService.SearchProductsAsync();

                result = products
                    .Take(50)
                    .Select(x => new
                    {
                        id = x.Id,
                        name = x.Name
                    }).ToList<object>();

                break;

            case DocumentEntityType.Order:

                var orders = await _orderService.SearchOrdersAsync();

                result = orders
                    .Take(50)
                    .Select(x => new
                    {
                        id = x.Id,
                        name = $"Order #{x.Id}"
                    }).ToList<object>();

                break;
        }

        return Json(result);
    }
}