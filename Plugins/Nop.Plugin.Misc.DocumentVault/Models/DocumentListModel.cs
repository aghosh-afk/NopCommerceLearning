using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.DocumentVault.Models;

public record DocumentListModel : BaseNopEntityModel
{
    public IList<DocumentModel> Documents { get; set; } = new List<DocumentModel>();
}
