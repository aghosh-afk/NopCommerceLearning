using System.Linq;
using Nop.Core.Domain.Notes;
using Nop.Data;

namespace Nop.Services.Notes
{
    public class SimpleNoteService : ISimpleNoteService
    {
        private readonly IRepository<SimpleNote> _noteRepo;

        public SimpleNoteService(IRepository<SimpleNote> noteRepo)
        {
            _noteRepo = noteRepo;
        }

        public IQueryable<SimpleNote> GetAll() => _noteRepo.Table;

        public SimpleNote GetById(int id) => _noteRepo.GetById(id);

        public void Insert(SimpleNote note) => _noteRepo.Insert(note);

        public void Update(SimpleNote note) => _noteRepo.Update(note);

        public void Delete(SimpleNote note) => _noteRepo.Delete(note);
    }
}
