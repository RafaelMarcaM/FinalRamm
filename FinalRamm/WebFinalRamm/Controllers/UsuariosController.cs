﻿using System;
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
    public class UsuariosController : Controller
    {
        private readonly FinalRammContext _context;

        public UsuariosController(FinalRammContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var finalRammContext = _context.Usuarios.Include(u => u.IdEmpleadoNavigation);   
            return View(await finalRammContext.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            var empleados = _context.Empleados.Select(e => new { e.Id, nombreCompleto = $"{e.Nombre} {e.PrimerApellido} {e.SegundoApellido}" });
            ViewData["IdEmpleado"] = new SelectList(empleados, "Id", "nombreCompleto");
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            usuario.Clave = AccountController.Encrypt("DieselSur");
            if (!string.IsNullOrEmpty(usuario.Usuario1))
            {
                usuario.UsuarioRegistro = User.Identity?.Name;
                usuario.FechaRegistro = DateTime.Now;
                usuario.RegistroActivo = true;
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var empleados = _context.Empleados.Select(e => new { e.Id, nombreCompleto = $"{e.Nombre} {e.PrimerApellido} {e.SegundoApellido}" });
            ViewData["IdEmpleado"] = new SelectList(empleados, "Id", "nombreCompleto");
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            var empleados = _context.Empleados.Select(e => new { e.Id, nombreCompleto = $"{e.Nombre} {e.PrimerApellido} {e.SegundoApellido}" });
            ViewData["IdEmpleado"] = new SelectList(empleados, "Id", "nombreCompleto");
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdEmpleado,Usuario1,Clave,UsuarioRegistro,RegistroActivo,FechaRegistro")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    usuario.UsuarioRegistro = User.Identity?.Name;
                    usuario.FechaRegistro = DateTime.Now;
                    usuario.RegistroActivo = true;
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
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
            var empleados = _context.Empleados.Select(e => new { e.Id, nombreCompleto = $"{e.Nombre} {e.PrimerApellido} {e.SegundoApellido}" });
            ViewData["IdEmpleado"] = new SelectList(empleados, "Id", "nombreCompleto");
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'FinalRammContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                //_context.Usuarios.Remove(usuario);
                usuario.RegistroActivo = false;
                _context.Update(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return (_context.Usuarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
