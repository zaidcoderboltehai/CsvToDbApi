using System.ComponentModel.DataAnnotations;

namespace CsvToDbApi.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; } // Don't map it from CSV
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }

}
