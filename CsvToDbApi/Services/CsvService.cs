using CsvHelper;
using CsvHelper.Configuration;
using CsvToDbApi.Data;
using CsvToDbApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CsvToDbApi.Services
{
    public class CsvService
    {
        private readonly AppDbContext _db;
        public CsvService(AppDbContext db) => _db = db;

        public async Task ImportAsync(Stream csvStream)
        {
            // Configuration for CSV parsing
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,   // Ignore header validation
                MissingFieldFound = null  // Ignore missing fields
            };

            // Reading the CSV stream
            using var reader = new StreamReader(csvStream);
            using var csv = new CsvReader(reader, config);

            // Mapping CSV records to DTO (without Id)
            var records = csv.GetRecords<PersonCsvDto>().ToList();

            // Mapping DTO to Person model (add Id automatically)
            var people = records.Select(r => new Person
            {
                Name = r.Name,
                Email = r.Email,
                Age = r.Age
            }).ToList();

            // Adding records to the database
            _db.People.AddRange(people);
            await _db.SaveChangesAsync();
        }
    }
}
