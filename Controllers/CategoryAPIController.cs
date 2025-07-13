﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoranASP.Data;
using RestoranASP.Models;

namespace RestoranASP.Controllers
{
    [Route("api/Kategorija")]
    [ApiController]
    public class CategoryAPIController : ControllerBase
    {
        
        private readonly ApplicationDbContext _context;

        public CategoryAPIController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Kategorija>>> DohvatiSve()
        {
            var sve = await _context.Kategorije.ToListAsync();
            return Ok(sve);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Kategorija>> DohvatiJednu(int id)
        {
            var jednaKategorija = await  _context.Kategorije.FirstOrDefaultAsync(k=>k.Id == id);

            if(jednaKategorija == null)
            {
                return NotFound();
            }
            return Ok(jednaKategorija);
        }
        [HttpPost]
        public async Task<ActionResult<Kategorija>> KreirajNovu([FromBody] Kategorija input)
        {
            if (string.IsNullOrWhiteSpace(input.Naziv))
            {
                ModelState.AddModelError("Naziv", "Naziv kategorije je obavezan!");
                return BadRequest(ModelState);
            }
            var nova = new Kategorija
            {
                Naziv = input.Naziv.Trim(),

            };
            _context.Kategorije.Add(nova);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(DohvatiJednu), new { id = nova.Id }, nova);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult>Azuriraj(int id, [FromBody] Kategorija input)
        {
            if(id != input.Id)
            {
                return BadRequest("URL id se ne poklapa s CategoryId u telu!");
            }
            if (string.IsNullOrWhiteSpace(input.Naziv))
            {
                ModelState.AddModelError("Naziv", "Naziv je obavezno polje!");
                return BadRequest(ModelState);
            }
            var kategorijaIzBaze = await _context.Kategorije.FindAsync(id);
            if(kategorijaIzBaze == null)
            {
                return NotFound();
            }
            kategorijaIzBaze.Naziv = input.Naziv.Trim();

            _context.Entry(kategorijaIzBaze).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult>Obrisi(int id)
        {
            var k = await _context.Kategorije.FirstOrDefaultAsync(k=>k.Id == id);
            if (k == null)
            {
                return NotFound();
            }
            _context.Kategorije.Remove(k);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
