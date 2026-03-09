using global::Nop.Plugin.Misc.DocumentVault.Domain;

namespace Nop.Plugin.Misc.DocumentVault.Services;

public interface IDocumentService
{
    Task InsertAsync(DocumentVaultFile file);

    Task<DocumentVaultFile?> GetByIdAsync(int id);

    Task<IList<DocumentVaultFile>> GetByEntityAsync(DocumentEntityType entityType, int entityId);

    Task<IList<DocumentVaultFile>> GetAllAsync();

    Task DeleteAsync(DocumentVaultFile file);
}