using System.Security.Cryptography.Pkcs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoranASP.Data;
using RestoranASP.Models;

namespace RestoranASP.Controllers
{
    public class NarudzbinaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NarudzbinaController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var svaJela = await _context.Jela
                .Select(j => new JeloCheckboxViewModel
                {
                    JeloId = j.Id,
                    Naziv = j.Naziv,
                    Cena = j.Cena,
                    IsChecked = false

                }).ToListAsync();

            var model = new NarudzbinaCreateViewModel
            {
                Jela = svaJela
            };
            return View(model);

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NarudzbinaCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if(!model.Jela.Any(j => j.IsChecked))
            {
                ModelState.AddModelError("", "Morate izabrati bar jedno jelo!");
                var svaJela = await _context.Jela
                .Select(j => new JeloCheckboxViewModel
                {
                    JeloId = j.Id,
                    Naziv = j.Naziv,
                    Cena = j.Cena,
                    IsChecked = false

                }).ToListAsync();

                var ponovo_model = new NarudzbinaCreateViewModel
                {
                    Jela = svaJela
                };
                return View(ponovo_model);
            }
            var nar = new Narudzbina
            {
                Datum = model.Datum,
                UserId = _userManager.GetUserId(User)
            };
            _context.Narudzbine.Add(nar);
            await _context.SaveChangesAsync();

            foreach (var j in model.Jela.Where(x => x.IsChecked))
            {
                var stavka = new StavkaNarudzbine
                {
                    NarudzbinaId = nar.Id,
                    JeloId = j.JeloId,
                    BrojPorcija = j.BrojPorcija
                };
                _context.StavkeNarudzbina.Add(stavka);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);

            if (User.IsInRole("Admin"))
            {
                var sveNarudzbine = await _context.Narudzbine
                    .Include(n => n.Jela)
                    .ThenInclude(s => s.Jelo)
                    .Include(n => n.User)
                    .ToListAsync();

                return View(sveNarudzbine);
            }

            var mojeNarudzbine = await _context.Narudzbine
                .Where(n => n.UserId == userId)
                .Include(n => n.Jela)
                .ThenInclude(s => s.Jelo)
                .ToListAsync();

            return View(mojeNarudzbine);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var narudzbina = await _context.Narudzbine
                .Include(n => n.Jela)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (narudzbina == null)
                return NotFound();

            var svaJela = await _context.Jela.ToListAsync();

            var model = new NarudzbinaCreateViewModel
            {
                NarudzbinaId = narudzbina.Id,
                Datum = narudzbina.Datum,
                Jela = svaJela.Select(j => new JeloCheckboxViewModel
                {
                    JeloId = j.Id,
                    Naziv = j.Naziv,
                    IsChecked = narudzbina.Jela.Any(s => s.JeloId == j.Id),
                    BrojPorcija = narudzbina.Jela.FirstOrDefault(s => s.JeloId == j.Id)?.BrojPorcija ?? 1
                }).ToList()
            };

            return View(model);

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NarudzbinaCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!model.Jela.Any(j => j.IsChecked))
            {
                ModelState.AddModelError("", "Morate izabrati bar jedno jelo!");
                var svaJela = await _context.Jela
                .Select(j => new JeloCheckboxViewModel
                {
                    JeloId = j.Id,
                    Naziv = j.Naziv,
                    Cena = j.Cena,
                    IsChecked = false

                }).ToListAsync();

                var ponovo_model = new NarudzbinaCreateViewModel
                {
                    Jela = svaJela
                };
                return View(ponovo_model);
            }

            var narudzbina = await _context.Narudzbine
                .Include(n => n.Jela)
                .FirstOrDefaultAsync(n => n.Id == model.NarudzbinaId);

            if (narudzbina == null)
                return NotFound();

            narudzbina.Datum = model.Datum;

            _context.StavkeNarudzbina.RemoveRange(narudzbina.Jela);

            foreach (var j in model.Jela.Where(x => x.IsChecked))
            {
                narudzbina.Jela.Add(new StavkaNarudzbine
                {
                    JeloId = j.JeloId,
                    BrojPorcija = j.BrojPorcija,
                    NarudzbinaId = narudzbina.Id
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var n = await _context.Narudzbine.Include(n => n.Jela).ThenInclude(sn => sn.Jelo).Include(n=>n.User).FirstOrDefaultAsync(n=>n.Id == id);   
            if (n == null) return NotFound();

            var narudzbina = new Narudzbina()
            {
                Id = n.Id,
                Datum = n.Datum,
                Jela = n.Jela,
                User = n.User
            };
            return View(narudzbina);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var n = await _context.Narudzbine.FindAsync(id);

            _context.Narudzbine.Remove(n);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
