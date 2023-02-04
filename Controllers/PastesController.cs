using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PastebinClone.Data;
using PastebinClone.Models;

namespace PastebinClone.Controllers
{
    public class PastesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PastesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pastes
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Paste.ToListAsync());
        }

        // GET: Pastes/ShowSearchForm
        [Authorize]
        public async Task<IActionResult> ShowSearchForm()
        {
            return View();
        }

        // GET: Pastes/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String SearchedAuthor)
        {
            return View("Index", await _context.Paste.Where( j => j.Author.Contains(SearchedAuthor)).ToListAsync());
        }

        // GET: Pastes/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Paste == null)
            {
                return NotFound();
            }

            var paste = await _context.Paste
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paste == null)
            {
                return NotFound();
            }

            return View(paste);
        }

        // GET: Pastes/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // PASTE: Pastes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Author,Description,PublishDate")] Paste paste)
        {
            if (ModelState.IsValid)
            {
                _context.Add(paste);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(paste);
        }

        // GET: Pastes/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Paste == null)
            {
                return NotFound();
            }

            var paste = await _context.Paste.FindAsync(id);
            if (paste == null)
            {
                return NotFound();
            }
            return View(paste);
        }

        // PASTE: Pastes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Author,Description,PublishDate")] Paste paste)
        {
            if (id != paste.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(paste);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PasteExists(paste.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(paste);
        }

        // GET: Pastes/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Paste == null)
            {
                return NotFound();
            }

            var paste = await _context.Paste
                .FirstOrDefaultAsync(m => m.Id == id);
            if (paste == null)
            {
                return NotFound();
            }

            return View(paste);
        }

        // PASTE: Pastes/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Paste == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Paste'  is null.");
            }
            var paste = await _context.Paste.FindAsync(id);
            if (paste != null)
            {
                _context.Paste.Remove(paste);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PasteExists(int id)
        {
          return _context.Paste.Any(e => e.Id == id);
        }
    }
}
