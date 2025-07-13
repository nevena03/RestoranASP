using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoranASP.Data;
using RestoranASP.Models;

namespace RestoranASP.Controllers
{
    public class KategorijaController : Controller
    {
       private readonly ApplicationDbContext _context;

        public KategorijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Upravljaj()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Kategorija model)
        {
            if (ModelState.IsValid)
            {
                var k = new Kategorija()
                {
                    Naziv = model.Naziv
                };

                _context.Kategorije.Add(k);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var k = await _context.Kategorije.FindAsync(id);
            if (k == null) return NotFound();

            var kategorija = new Kategorija
            {
                Id = k.Id,
                Naziv = k.Naziv
            };
            return View(kategorija);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Kategorija model)
        {
            if(id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var kategorija = await _context.Kategorije.FindAsync(id);
                if (kategorija == null) return NotFound();

                kategorija.Naziv = model.Naziv;

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var k = await _context.Kategorije.FindAsync(id);

            try
            {
                _context.Kategorije.Remove(k);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return BadRequest("U kategoriji se nalaze jela! Zabranjeno brisanje!");
            }
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var kategorije = await _context.Kategorije.Include(k => k.Jela).ToListAsync();
            return View(kategorije);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var k = await _context.Kategorije.Include(k=>k.Jela).FirstOrDefaultAsync(k=>k.Id == id);
            if (k == null) return NotFound();

            var kategorija = new Kategorija()
            {
                Id = k.Id,
                Naziv = k.Naziv,
                Jela = k.Jela
            };
            return View(kategorija);
        }
    }
}
