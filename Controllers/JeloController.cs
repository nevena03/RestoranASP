using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestoranASP.Data;
using RestoranASP.Models;

namespace RestoranASP.Controllers
{
    public class JeloController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JeloController (ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var kategorije = _context.Kategorije.ToList();
            ViewBag.Kategorije = new SelectList(kategorije, "Id", "Naziv");
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Jelo model)
        {
            if (ModelState.IsValid)
            {
                var j = new Jelo()
                {
                    Naziv = model.Naziv,
                    Cena = model.Cena,
                    KategorijaId = model.KategorijaId,
                    Kategorija = model.Kategorija,
                    Slika = model.Slika

                };

                _context.Jela.Add(j);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            ViewBag.Kategorije = new SelectList(_context.Kategorije, "Id", "Naziv", model.KategorijaId);
            return View();

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var j = await _context.Jela.FindAsync(id);
            if (j == null) return NotFound();

            var jelo = new Jelo
            {
                Id = j.Id,
                Naziv = j.Naziv,
                Cena = j.Cena,
                Slika = j.Slika,
                Kategorija = j.Kategorija,
                KategorijaId = j.KategorijaId
            };
            ViewBag.Kategorije = new SelectList(_context.Kategorije, "Id", "Naziv", j.KategorijaId);
            return View(jelo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Jelo model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var jelo = await _context.Jela.FindAsync(id);
                if (jelo == null) return NotFound();

                jelo.Naziv = model.Naziv;
                jelo.Cena = model.Cena;
                jelo.Slika = model.Slika;
                jelo.Kategorija = model.Kategorija;
                jelo.KategorijaId = model.KategorijaId;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Kategorije = new SelectList(_context.Kategorije, "Id", "Naziv", model.KategorijaId);
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var j = await _context.Jela.FindAsync(id);

            _context.Jela.Remove(j);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        
        public async Task<IActionResult> Index()
        {
            var jelo = await _context.Jela.ToListAsync();
            return View(jelo);
        }

        public async Task<IActionResult> Details(int id)
        {
            var j = await _context.Jela.Include(j => j.Kategorija).FirstOrDefaultAsync(r => r.Id == id);
            if (j == null) return NotFound();

            var jelo = new Jelo()
            {
                Id = j.Id,
                Naziv = j.Naziv,
                Cena = j.Cena,
                Slika = j.Slika,
                KategorijaId = j.KategorijaId,
                Kategorija = j.Kategorija
            };
            return View(jelo);
        }
    }
}
