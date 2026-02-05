using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Notes;
using Nop.Services.Notes;

using Nop.Web.Areas.Admin.Models.SimpleNote;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area("Admin")]
    public class SimpleNoteController : BaseAdminController
    {
        private readonly ISimpleNoteService _noteService;

        public SimpleNoteController(ISimpleNoteService noteService)
        {
            _noteService = noteService;
        }

        // GET: /Admin/SimpleNote/
        public IActionResult Index()
        {
            var searchModel = new SimpleNoteSearchModel
            {
                AvailablePageSizes = "10,20,50" // string, not List<int>
            };
            return View(searchModel);
        }


        [HttpPost]
        public IActionResult List(SimpleNoteSearchModel searchModel)
        {
            var query = _noteService.GetAll();

            // Filters
            if (!string.IsNullOrWhiteSpace(searchModel.SearchTitle))
                query = query.Where(n => n.Title.Contains(searchModel.SearchTitle));

            if (searchModel.SearchDate.HasValue)
            {
                var date = searchModel.SearchDate.Value.Date;
                query = query.Where(n => n.CreatedOnUtc >= date && n.CreatedOnUtc < date.AddDays(1));
            }

            var totalCount = query.Count();

            // Paging
            int pageIndex = searchModel.Page > 0 ? searchModel.Page - 1 : 0;
            int pageSize = searchModel.PageSize > 0 ? searchModel.PageSize : 10;

            var notes = query
                .OrderByDescending(n => n.CreatedOnUtc)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(n => new SimpleNoteModel
                {
                    Id = n.Id,
                    Title = n.Title,
                    Description = n.Description,
                    CreatedOnUtc = n.CreatedOnUtc
                }).ToList();

            return Json(new
            {
                Notes = notes,
                TotalCount = totalCount,
                PageSize = pageSize
            });
        }


        public IActionResult Create()
        {
            return View("Create"); // was "CreateEdit"
        }

        [HttpPost]
        public IActionResult Create(SimpleNoteModel model)
        {
            if (!ModelState.IsValid)
                return View("Create"); // was "CreateEdit"

            _noteService.Insert(new SimpleNote
            {
                Title = model.Title,
                Description = model.Description,
                CreatedOnUtc = DateTime.UtcNow
            });

            return RedirectToAction("Index");
        }

        // Edit
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

            return View("Edit", model); // was "CreateEdit"
        }

        [HttpPost]
        public IActionResult Edit(SimpleNoteModel model)
        {
            if (!ModelState.IsValid)
                return View("Edit", model); // was "CreateEdit"

            var note = _noteService.GetById(model.Id);
            if (note == null)
                return NotFound();

            note.Title = model.Title;
            note.Description = model.Description;

            _noteService.Update(note);

            return RedirectToAction("Index");
        }


        [HttpPost]
        [Area("Admin")]
        public IActionResult Delete(int id)
        {
            var note = _noteService.GetById(id);
            if (note == null)
                return Json(new { success = false, message = "Note not found" });

            _noteService.Delete(note);
            return Json(new { success = true });
        }


    }
}
