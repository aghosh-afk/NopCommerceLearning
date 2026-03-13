using LinqToDB;
using Nop.Data;
using Nop.Plugin.Misc.DocumentVault.Data;
using Nop.Plugin.Misc.DocumentVault.Domain;

namespace Nop.Plugin.Misc.DocumentVault.Services;

public class DocumentService : IDocumentService
{
    private readonly IRepository<DocumentVaultFile> _repository;

    public DocumentService(IRepository<DocumentVaultFile> repository)
    {
        _repository = repository;
    }

    public async Task InsertAsync(DocumentVaultFile file)
    {
        await _repository.InsertAsync(file);
    }

    public async Task<DocumentVaultFile?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IList<DocumentVaultFile>> GetByEntityAsync(DocumentEntityType entityType, int entityId)
    {
        return await _repository.Table
            .Where(x => x.EntityType == entityType && x.EntityId == entityId)
            .ToListAsync();
    }

    public async Task<IList<DocumentVaultFile>> GetAllAsync()
    {
        return await _repository.Table
            .OrderByDescending(x => x.UploadedOnUtc)
            .ToListAsync();
    }

    public async Task DeleteAsync(DocumentVaultFile file)
    {
        await _repository.DeleteAsync(file);
    }
}