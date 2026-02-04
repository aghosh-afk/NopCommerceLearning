using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Notes;

namespace Nop.Services.Notes;

public interface ISimpleNoteService
{
    SimpleNote GetById(int id);
    IQueryable<SimpleNote> GetAll();
    void Insert(SimpleNote note);
    void Update(SimpleNote note);
    void Delete(SimpleNote note);
}
