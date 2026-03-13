using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.DocumentVault.Domain;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.DocumentVault.Models;

public record DocumentModel : BaseNopEntityModel
{
    public string? Title { get; set; }

    public string? FileName { get; set; }

    [Required(ErrorMessage = "Entity type is required")]
    public DocumentEntityType EntityType { get; set; }

    public int EntityId { get; set; }

    public long FileSize { get; set; }

    public DateTime UploadedOnUtc { get; set; }

    public IFormFile? File { get; set; }

    public IList<SelectListItem> AvailableEntityTypes { get; set; } = new List<SelectListItem>();
}
