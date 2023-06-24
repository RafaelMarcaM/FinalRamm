using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFinalRamm.Models;

namespace WebFinalRamm.Controllers
{
    [Authorize]
    public class CompraDetallesController : Controller
    {
        private readonly FinalRammContext _context;

        public CompraDetallesController(FinalRammContext context)
        {
            _context = context;
        }

        // GET: CompraDetalles
        public async Task<IActionResult> Index()
        {
            var finalRammContext = _context.CompraDetalles.Include(c => c.IdCompraNavigation).Include(c => c.IdProductoNavigation);
            return View(await finalRammContext.ToListAsync());
        }

        // GET: CompraDetalles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CompraDetalles == null)
            {
                return NotFound();
            }

            var compraDetalle = await _context.CompraDetalles
                .Include(c => c.IdCompraNavigation)
                .Include(c => c.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compraDetalle == null)
            {
                return NotFound();
            }

            return View(compraDetalle);
        }

        // GET: CompraDetalles/Create
        public IActionResult Create()
        {
            ViewData["IdCompra"] = new SelectList(_context.Compras, "Id", "Id");
            ViewData["IdProducto"] = new SelectList(_context.Productos, "Id", "Id");
            return View();
        }

        // POST: CompraDetalles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCompra,IdProducto,Cantidad,PrecioUnitario,Total")] CompraDetalle compraDetalle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(compraDetalle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "Id", "Id", compraDetalle.IdCompra);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "Id", "Id", compraDetalle.IdProducto);
            return View(compraDetalle);
        }

        // GET: CompraDetalles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CompraDetalles == null)
            {
                return NotFound();
            }

            var compraDetalle = await _context.CompraDetalles.FindAsync(id);
            if (compraDetalle == null)
            {
                return NotFound();
            }
            ViewData["IdCompra"] = new SelectList(_context.Compras, "Id", "Id", compraDetalle.IdCompra);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "Id", "Id", compraDetalle.IdProducto);
            return View(compraDetalle);
        }

        // POST: CompraDetalles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCompra,IdProducto,Cantidad,PrecioUnitario,Total")] CompraDetalle compraDetalle)
        {
            if (id != compraDetalle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compraDetalle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraDetalleExists(compraDetalle.Id))
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
            ViewData["IdCompra"] = new SelectList(_context.Compras, "Id", "Id", compraDetalle.IdCompra);
            ViewData["IdProducto"] = new SelectList(_context.Productos, "Id", "Id", compraDetalle.IdProducto);
            return View(compraDetalle);
        }

        // GET: CompraDetalles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CompraDetalles == null)
            {
                return NotFound();
            }

            var compraDetalle = await _context.CompraDetalles
                .Include(c => c.IdCompraNavigation)
                .Include(c => c.IdProductoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compraDetalle == null)
            {
                return NotFound();
            }

            return View(compraDetalle);
        }

        // POST: CompraDetalles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CompraDetalles == null)
            {
                return Problem("Entity set 'FinalRammContext.CompraDetalles'  is null.");
            }
            var compraDetalle = await _context.CompraDetalles.FindAsync(id);
            if (compraDetalle != null)
            {
                _context.CompraDetalles.Remove(compraDetalle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraDetalleExists(int id)
        {
          return (_context.CompraDetalles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
