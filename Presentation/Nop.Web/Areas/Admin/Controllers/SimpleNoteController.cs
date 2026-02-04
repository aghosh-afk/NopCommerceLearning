using Microsoft.AspNetCore.Mvc;
using Nop.Services.Notes;
using Nop.Web.Areas.Admin.Models.Notes;

namespace Nop.Web.Areas.Admin.Controllers;

public class SimpleNoteController : BaseAdminController
{
    private readonly ISimpleNoteService _noteService;

    public SimpleNoteController(ISimpleNoteService noteService)
    {
        _noteService = noteService;
    }

    public IActionResult Index()
    {
        var model = _noteService.GetAll()
            .Select(n => new SimpleNoteModel
            {
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                CreatedOnUtc = n.CreatedOnUtc
            }).ToList();

        return View(model);
    }

    public IActionResult Create() => View(new SimpleNoteModel());

    [HttpPost]
    public IActionResult Create(SimpleNoteModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        _noteService.Insert(new Core.Domain.Notes.SimpleNote
        {
            Title = model.Title,
            Description = model.Description
        });

        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var note = _noteService.GetById(id);
        if (note == null)
            return NotFound();

        var model = new SimpleNoteModel
        {
            Id = note.Id,
            Title = note.Title,
            Description = note.Description,
            CreatedOnUtc = note.CreatedOnUtc
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Edit(SimpleNoteModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var note = _noteService.GetById(model.Id);
        if (note == null)
            return NotFound();

        note.Title = model.Title;
        note.Description = model.Description;

        _noteService.Update(note);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var note = _noteService.GetById(id);
        if (note == null)
            return NotFound();

        _noteService.Delete(note);
        return RedirectToAction("Index");
    }
}
