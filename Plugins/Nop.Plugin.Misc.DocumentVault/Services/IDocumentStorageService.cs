using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.DocumentVault.Services;

public interface IDocumentStorageService
{
    Task<string> UploadAsync(Stream stream, string path);

    Task<string> GetPresignedUrlAsync(string objectKey);

    Task<Stream> DownloadAsync(string objectKey);

    Task DeleteAsync(string objectKey);
}
