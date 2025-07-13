using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestoranASP.Data;
using RestoranASP.Models;
using RestoranASP.Models.ViewModels;

namespace RestoranASP.Controllers
{

    public class JeloController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public JeloController (ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

            try
            {
                _context.Jela.Remove(j);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return BadRequest("Jelo ne može biti obrisano, jer se nalazi u nekim narudžbinama!");
            }
        }

        
        public async Task<IActionResult> Index(int? categoryId, string searchTerm, string sortOrder, int page=1, int pageSize=10)
        {
            var JeloQuery = _context.Jela.Include(j=>j.Kategorija).AsQueryable();
            if (categoryId.HasValue)
            {
                JeloQuery = JeloQuery.Where(j=>j.KategorijaId == categoryId.Value);
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                JeloQuery = JeloQuery.Where(j => j.Naziv.Contains(searchTerm));
            }
            JeloQuery = sortOrder switch
            {
                "naziv_desc" => JeloQuery.OrderByDescending(j => j.Naziv),
                "cena" => JeloQuery.OrderByDescending(j => j.Cena),
                _ => JeloQuery.OrderBy(j=>j.Naziv)
            };

            var totalItems = await JeloQuery.CountAsync();
            var jela = await JeloQuery.Skip((page-1) * pageSize).Take(pageSize).ToListAsync();

            var kategorije = await _context.Kategorije.ToListAsync();
            var selectList = new SelectList(kategorije, "Id", "Naziv", categoryId);

            var pagedResult = new PagedResult<Jelo>
            {
                Items = jela,
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            var viewModel = new JeloFilterViewModel
            {
                PagedJela = pagedResult,
                CategorySelectList = selectList,
                SelectedCategoryId = categoryId,
                SearchTerm = searchTerm,
                SortOrder = sortOrder
            };

            ViewBag.CurretUserId = _userManager.GetUserId(User);
            return View(viewModel);


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
