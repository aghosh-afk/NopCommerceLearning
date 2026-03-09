using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.DocumentVault.Models;

public record ConfigurationModel : BaseNopModel
{
    [NopResourceDisplayName("Plugins.Misc.DocumentVault.Fields.IsEnable")]
    public bool IsEnable { get; set; }

    [NopResourceDisplayName("Plugins.Misc.DocumentVault.Fields.Endpoint")]
    public string Endpoint { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.DocumentVault.Fields.AccessKey")]
    public string AccessKey { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.DocumentVault.Fields.SecretKey")]
    public string SecretKey { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.DocumentVault.Fields.BucketName")]
    public string BucketName { get; set; } = string.Empty;

    [NopResourceDisplayName("Plugins.Misc.DocumentVault.Fields.UseSSL")]
    public bool UseSSL { get; set; }
}