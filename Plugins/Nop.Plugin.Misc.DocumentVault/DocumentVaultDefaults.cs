using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.DocumentVault
{
    public static class DocumentVaultDefaults
    {
        public const string SYSTEM_NAME = "Misc.DocumentVault";
        public const string BUCKET_NAME = "nop-documents";

        public static class CleanupTask
        {
            public const string NAME = "DocumentVault Cleanup Task";

            public const string TYPE=
                "Nop.Plugin.Misc.DocumentVault.Services.DocumentCleanupTask";

            public const int PERIODINSECONDS = 3600;
        }
    }
}