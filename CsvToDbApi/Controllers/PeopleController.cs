using CsvToDbApi.Data;
using CsvToDbApi.Models;
using CsvToDbApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CsvToDbApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly CsvService _csvService;

        public PeopleController(AppDbContext db, CsvService csvService)
        {
            _db = db;
            _csvService = csvService;
        }

        // 1) Get All
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.People.ToListAsync();
            return Ok(list);
        }

        // 2) Get by Id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var person = await _db.People.FindAsync(id);
            if (person == null) return NotFound();
            return Ok(person);
        }

        // 3) Create via CSV file
        [HttpPost("import")]
        public async Task<IActionResult> ImportCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("CSV file chahiye");

            await _csvService.ImportAsync(file.OpenReadStream());
            return Ok("Imported Successfully");
        }

        // 4) Update
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Person updated)
        {
            var p = await _db.People.FindAsync(id);
            if (p == null) return NotFound();
            p.Name = updated.Name;
            p.Email = updated.Email;
            p.Age = updated.Age;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // 5) Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.People.FindAsync(id);
            if (p == null) return NotFound();
            _db.People.Remove(p);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
