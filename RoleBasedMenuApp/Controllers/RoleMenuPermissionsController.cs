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
    public class RoleMenuPermissionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RoleMenuPermissionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RoleMenuPermissions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RoleMenuPermission.Include(r => r.NavigationMenu);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RoleMenuPermissions/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roleMenuPermission = await _context.RoleMenuPermission
                .Include(r => r.NavigationMenu)
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (roleMenuPermission == null)
            {
                return NotFound();
            }

            return View(roleMenuPermission);
        }

        // GET: RoleMenuPermissions/Create
        public IActionResult Create()
        {
            ViewData["NavigationMenuId"] = new SelectList(_context.NavigationMenu, "Id", "Id");
            return View();
        }

        // POST: RoleMenuPermissions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleId,NavigationMenuId")] RoleMenuPermission roleMenuPermission)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roleMenuPermission);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NavigationMenuId"] = new SelectList(_context.NavigationMenu, "Id", "Id", roleMenuPermission.NavigationMenuId);
            return View(roleMenuPermission);
        }

        // GET: RoleMenuPermissions/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roleMenuPermission = await _context.RoleMenuPermission.FindAsync(id);
            if (roleMenuPermission == null)
            {
                return NotFound();
            }
            ViewData["NavigationMenuId"] = new SelectList(_context.NavigationMenu, "Id", "Id", roleMenuPermission.NavigationMenuId);
            return View(roleMenuPermission);
        }

        // POST: RoleMenuPermissions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RoleId,NavigationMenuId")] RoleMenuPermission roleMenuPermission)
        {
            if (id != roleMenuPermission.RoleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roleMenuPermission);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleMenuPermissionExists(roleMenuPermission.RoleId))
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
            ViewData["NavigationMenuId"] = new SelectList(_context.NavigationMenu, "Id", "Id", roleMenuPermission.NavigationMenuId);
            return View(roleMenuPermission);
        }

        // GET: RoleMenuPermissions/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roleMenuPermission = await _context.RoleMenuPermission
                .Include(r => r.NavigationMenu)
                .FirstOrDefaultAsync(m => m.RoleId == id);
            if (roleMenuPermission == null)
            {
                return NotFound();
            }

            return View(roleMenuPermission);
        }

        // POST: RoleMenuPermissions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var roleMenuPermission = await _context.RoleMenuPermission.FindAsync(id);
            _context.RoleMenuPermission.Remove(roleMenuPermission);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleMenuPermissionExists(string id)
        {
            return _context.RoleMenuPermission.Any(e => e.RoleId == id);
        }
    }
}
