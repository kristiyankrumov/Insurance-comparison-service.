using System.ComponentModel.DataAnnotations;

namespace InsuranceComparisonService.Models
{
    public enum FuelType { Petrol, Diesel, Electric, Hybrid }
    public enum VehicleCategory { Car, Motorcycle, Truck, Bus }

    public class Vehicle
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Markata e zadaljitelna")]
        [StringLength(100)]
        public string Make { get; set; } = string.Empty;

        [Required(ErrorMessage = "Modelat e zadaljitelen")]
        [StringLength(100)]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Godinata e zadaljitelna")]
        [Range(1950, 2030, ErrorMessage = "Nevalidna godina")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Registratsionniyat nomer e zadaljitelen")]
        [StringLength(10)]
        public string PlateNumber { get; set; } = string.Empty;

        [Range(50, 2000)]
        public int HorsePower { get; set; }

        public FuelType FuelType { get; set; }
        public VehicleCategory Category { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public ICollection<InsurancePolicy> Policies { get; set; } = new List<InsurancePolicy>();

        /// <summary>Returns Make + Model + Year as a display string.</summary>
        public string FullName => $"{Make} {Model} ({Year})";

        /// <summary>Returns vehicle age in years.</summary>
        public int AgeInYears => DateTime.Now.Year - Year;
    }
}
