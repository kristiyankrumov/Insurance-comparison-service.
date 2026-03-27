using System.ComponentModel.DataAnnotations;

namespace InsuranceComparisonService.Models
{
    public enum PolicyStatus { Active, Expired, Cancelled }
    public enum PaymentType { OneTime, Monthly }

    public class InsurancePolicy
    {
        public int Id { get; set; }

        public int OfferId { get; set; }
        public InsuranceOffer? Offer { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public int? VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public PolicyStatus Status { get; set; } = PolicyStatus.Active;
        public PaymentType PaymentType { get; set; } = PaymentType.OneTime;

        [Range(1, 200000)]
        public decimal FinalPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string PolicyNumber { get; set; } = string.Empty;

        /// <summary>True if EndDate has passed.</summary>
        public bool IsExpired => EndDate < DateTime.UtcNow;

        /// <summary>Days remaining until expiry. Negative if already expired.</summary>
        public int DaysRemaining => (EndDate.Date - DateTime.UtcNow.Date).Days;

        /// <summary>Human-readable status including expiry info.</summary>
        public string StatusDisplay => Status switch
        {
            PolicyStatus.Cancelled => "Cancelled",
            PolicyStatus.Expired   => "Expired",
            _ => IsExpired ? "Expired" : $"Active ({DaysRemaining}d left)"
        };
    }
}
