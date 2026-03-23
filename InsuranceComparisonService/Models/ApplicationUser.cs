using Microsoft.AspNetCore.Identity;

namespace InsuranceComparisonService.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<UserFavorite> Favorites { get; set; } = new List<UserFavorite>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
