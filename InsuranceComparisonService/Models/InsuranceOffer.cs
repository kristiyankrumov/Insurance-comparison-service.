using System.ComponentModel.DataAnnotations;

namespace InsuranceComparisonService.Models
{
    public enum InsuranceType
    {
        Kasko,
        Health,
        Civil,
        Property
    }

    public class InsuranceOffer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naimenovanieto e zadaljitelno")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Opisanieto e zadaljitelno")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vidat zastrahkovka e zadaljitelen")]
        public InsuranceType Type { get; set; }

        [Required(ErrorMessage = "Tsenata e zadaljitelna")]
        [Range(1, 100000)]
        public decimal Price { get; set; }

        [Range(0, 50)]
        public int DiscountPercent { get; set; } = 0;

        public decimal FinalPrice =>
            DiscountPercent > 0
                ? Math.Round(Price * (1 - DiscountPercent / 100m), 2)
                : Price;

        [Required]
        public string Coverage { get; set; } = string.Empty;

        public string Conditions { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CompanyId { get; set; }
        public InsuranceCompany? Company { get; set; }

        public ICollection<UserFavorite> Favorites { get; set; } = new List<UserFavorite>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
