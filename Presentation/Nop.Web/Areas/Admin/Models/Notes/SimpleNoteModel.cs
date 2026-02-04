using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Notes;

public class SimpleNoteModel
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreatedOnUtc { get; set; }
}
