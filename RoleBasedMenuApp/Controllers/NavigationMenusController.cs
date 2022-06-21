using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoleBasedMenuApp.Data;

namespace RoleBasedMenuApp.Controllers
{
    [Authorize("Authorization")]
    public class NavigationMenusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NavigationMenusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NavigationMenus
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.NavigationMenu.Include(n => n.ParentNavigationMenu);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: NavigationMenus/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navigationMenu = await _context.NavigationMenu
                .Include(n => n.ParentNavigationMenu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navigationMenu == null)
            {
                return NotFound();
            }

            return View(navigationMenu);
        }

        // GET: NavigationMenus/Create
        public IActionResult Create()
        {
            ViewData["ParentMenuId"] = new SelectList(_context.NavigationMenu, "Id", "Id");
            return View();
        }

        // POST: NavigationMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ParentMenuId,Area,ControllerName,ActionName,IsExternal,ExternalUrl,DisplayOrder,Visible")] NavigationMenu navigationMenu)
        {
            if (ModelState.IsValid)
            {
                navigationMenu.Id = Guid.NewGuid();
                _context.Add(navigationMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentMenuId"] = new SelectList(_context.NavigationMenu, "Id", "Id", navigationMenu.ParentMenuId);
            return View(navigationMenu);
        }

        // GET: NavigationMenus/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navigationMenu = await _context.NavigationMenu.FindAsync(id);
            if (navigationMenu == null)
            {
                return NotFound();
            }
            ViewData["ParentMenuId"] = new SelectList(_context.NavigationMenu, "Id", "Id", navigationMenu.ParentMenuId);
            return View(navigationMenu);
        }

        // POST: NavigationMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,ParentMenuId,Area,ControllerName,ActionName,IsExternal,ExternalUrl,DisplayOrder,Visible")] NavigationMenu navigationMenu)
        {
            if (id != navigationMenu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(navigationMenu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NavigationMenuExists(navigationMenu.Id))
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
            ViewData["ParentMenuId"] = new SelectList(_context.NavigationMenu, "Id", "Id", navigationMenu.ParentMenuId);
            return View(navigationMenu);
        }

        // GET: NavigationMenus/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var navigationMenu = await _context.NavigationMenu
                .Include(n => n.ParentNavigationMenu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (navigationMenu == null)
            {
                return NotFound();
            }

            return View(navigationMenu);
        }

        // POST: NavigationMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var navigationMenu = await _context.NavigationMenu.FindAsync(id);
            _context.NavigationMenu.Remove(navigationMenu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NavigationMenuExists(Guid id)
        {
            return _context.NavigationMenu.Any(e => e.Id == id);
        }
    }
}
