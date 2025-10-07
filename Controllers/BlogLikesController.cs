using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MLFamilyTravelBlog.Data;
using MLFamilyTravelBlog.Models;

namespace MLFamilyTravelBlog.Controllers
{
    public class BlogLikesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogLikesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BlogLikes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BlogLikes.Include(b => b.BlogPost).Include(b => b.BlogUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BlogLikes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogLike = await _context.BlogLikes
                .Include(b => b.BlogPost)
                .Include(b => b.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogLike == null)
            {
                return NotFound();
            }

            return View(blogLike);
        }

        // GET: BlogLikes/Create
        public IActionResult Create()
        {
            ViewData["BlogPostId"] = new SelectList(_context.BlogPosts, "Id", "Content");
            ViewData["BlogUserId"] = new SelectList(_context.BlogUsers, "Id", "Id");
            return View();
        }

        // POST: BlogLikes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BlogPostId,IsLiked,BlogUserId")] BlogLike blogLike)
        {
            if (ModelState.IsValid)
            {
                _context.Add(blogLike);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogPostId"] = new SelectList(_context.BlogPosts, "Id", "Content", blogLike.BlogPostId);
            ViewData["BlogUserId"] = new SelectList(_context.BlogUsers, "Id", "Id", blogLike.BlogUserId);
            return View(blogLike);
        }

        // GET: BlogLikes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogLike = await _context.BlogLikes.FindAsync(id);
            if (blogLike == null)
            {
                return NotFound();
            }
            ViewData["BlogPostId"] = new SelectList(_context.BlogPosts, "Id", "Content", blogLike.BlogPostId);
            ViewData["BlogUserId"] = new SelectList(_context.BlogUsers, "Id", "Id", blogLike.BlogUserId);
            return View(blogLike);
        }

        // POST: BlogLikes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogPostId,IsLiked,BlogUserId")] BlogLike blogLike)
        {
            if (id != blogLike.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blogLike);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogLikeExists(blogLike.Id))
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
            ViewData["BlogPostId"] = new SelectList(_context.BlogPosts, "Id", "Content", blogLike.BlogPostId);
            ViewData["BlogUserId"] = new SelectList(_context.BlogUsers, "Id", "Id", blogLike.BlogUserId);
            return View(blogLike);
        }

        // GET: BlogLikes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogLike = await _context.BlogLikes
                .Include(b => b.BlogPost)
                .Include(b => b.BlogUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogLike == null)
            {
                return NotFound();
            }

            return View(blogLike);
        }

        // POST: BlogLikes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogLike = await _context.BlogLikes.FindAsync(id);
            if (blogLike != null)
            {
                _context.BlogLikes.Remove(blogLike);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogLikeExists(int id)
        {
            return _context.BlogLikes.Any(e => e.Id == id);
        }
    }
}
