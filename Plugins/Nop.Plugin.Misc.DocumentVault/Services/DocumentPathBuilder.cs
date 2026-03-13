using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.DocumentVault.Services;

public static class DocumentPathBuilder
{
    public static string BuildPath(string entityType, int entityId, string fileName)
    {
        var safeFileName = fileName.Replace(" ", "_");

        return $"{entityType}/{entityId}/{Guid.NewGuid()}_{safeFileName}";
    }
}
