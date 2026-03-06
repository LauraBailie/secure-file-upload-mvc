using System.ComponentModel.DataAnnotations;

namespace LeakAlertDemo.Models
{
    public class Plumber
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? LicenseNumber { get; set; }

        public bool Verified { get; set; }

        // THIS IS THE PROPERTY YOU NEED
        public string? DocumentPath { get; set; }
    }
}