using System;

namespace Nop.Web.Areas.Admin.Models.SimpleNote
{
    public class SimpleNoteSearchModel
    {
        public string SearchTitle { get; set; }
        public DateTime? SearchDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string AvailablePageSizes { get; set; } = "10,20,50";
    }
}
