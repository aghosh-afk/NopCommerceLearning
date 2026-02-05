using System.Collections.Generic;


namespace Nop.Web.Areas.Admin.Models.SimpleNote
{
    public class SimpleNoteListModel
    {
        public SimpleNoteSearchModel SearchModel { get; set; }
        public IList<SimpleNoteModel> Notes { get; set; }

    }
}
