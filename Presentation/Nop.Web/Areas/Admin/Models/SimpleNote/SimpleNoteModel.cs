using System;

namespace Nop.Web.Areas.Admin.Models.SimpleNote
{
    public class SimpleNoteModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
}
