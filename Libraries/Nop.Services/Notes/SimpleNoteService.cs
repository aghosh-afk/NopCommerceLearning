using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Notes;
using Nop.Data;

namespace Nop.Services.Notes;

public class SimpleNoteService : ISimpleNoteService
{
    private readonly IRepository<SimpleNote> _noteRepository;

    public SimpleNoteService(IRepository<SimpleNote> noteRepository)
    {
        _noteRepository = noteRepository;
    }

    public SimpleNote GetById(int id) => _noteRepository.GetById(id);

    public IQueryable<SimpleNote> GetAll() => _noteRepository.Table;

    public void Insert(SimpleNote note)
    {
        note.CreatedOnUtc = DateTime.UtcNow;
        _noteRepository.Insert(note);
    }

    public void Update(SimpleNote note) => _noteRepository.Update(note);

    public void Delete(SimpleNote note) => _noteRepository.Delete(note);
}
