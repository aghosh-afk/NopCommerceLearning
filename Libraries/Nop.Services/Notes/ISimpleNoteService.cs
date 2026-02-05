using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Notes;

namespace Nop.Services.Notes;

public interface ISimpleNoteService
{
    IQueryable<SimpleNote> GetAll();
    SimpleNote GetById(int id);
    void Insert(SimpleNote note);
    void Update(SimpleNote note);
    void Delete(SimpleNote note);
}
