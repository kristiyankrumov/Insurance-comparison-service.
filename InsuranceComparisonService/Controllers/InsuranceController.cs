using InsuranceComparisonService.Data;
using InsuranceComparisonService.Models;
using InsuranceComparisonService.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsuranceComparisonService.Controllers
{
    public class InsuranceController : Controller
    {
        private readonly IInsuranceRepository _repo;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InsuranceController(IInsuranceRepository repo, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Kasko(decimal? minPrice, decimal? maxPrice, int? companyId)
        {
            var offers = await _repo.GetOffersByTypeAsync(InsuranceType.Kasko);
            if (minPrice.HasValue) offers = offers.Where(o => o.Price >= minPrice.Value);
            if (maxPrice.HasValue) offers = offers.Where(o => o.Price <= maxPrice.Value);
            if (companyId.HasValue) offers = offers.Where(o => o.CompanyId == companyId.Value);
            ViewBag.Companies = await _repo.GetAllCompaniesAsync();
            return View(offers);
        }

        public async Task<IActionResult> Health(decimal? minPrice, decimal? maxPrice, int? companyId)
        {
            var offers = await _repo.GetOffersByTypeAsync(InsuranceType.Health);
            if (minPrice.HasValue) offers = offers.Where(o => o.Price >= minPrice.Value);
            if (maxPrice.HasValue) offers = offers.Where(o => o.Price <= maxPrice.Value);
            if (companyId.HasValue) offers = offers.Where(o => o.CompanyId == companyId.Value);
            ViewBag.Companies = await _repo.GetAllCompaniesAsync();
            return View(offers);
        }

        public async Task<IActionResult> Civil(decimal? minPrice, decimal? maxPrice, int? companyId)
        {
            var offers = await _repo.GetOffersByTypeAsync(InsuranceType.Civil);
            if (minPrice.HasValue) offers = offers.Where(o => o.Price >= minPrice.Value);
            if (maxPrice.HasValue) offers = offers.Where(o => o.Price <= maxPrice.Value);
            if (companyId.HasValue) offers = offers.Where(o => o.CompanyId == companyId.Value);
            ViewBag.Companies = await _repo.GetAllCompaniesAsync();
            return View(offers);
        }

        public async Task<IActionResult> Property(decimal? minPrice, decimal? maxPrice, int? companyId)
        {
            var offers = await _repo.GetOffersByTypeAsync(InsuranceType.Property);
            if (minPrice.HasValue) offers = offers.Where(o => o.Price >= minPrice.Value);
            if (maxPrice.HasValue) offers = offers.Where(o => o.Price <= maxPrice.Value);
            if (companyId.HasValue) offers = offers.Where(o => o.CompanyId == companyId.Value);
            ViewBag.Companies = await _repo.GetAllCompaniesAsync();
            return View(offers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var offer = await _repo.GetOfferByIdAsync(id);
            if (offer == null) return NotFound();
            return View(offer);
        }

        public async Task<IActionResult> Compare(int id1, int id2)
        {
            if (id1 == 0 || id2 == 0)
                return RedirectToAction("Index", "Home");

            var offer1 = await _repo.GetOfferByIdAsync(id1);
            var offer2 = await _repo.GetOfferByIdAsync(id2);

            if (offer1 == null || offer2 == null)
                return NotFound();

            return View((offer1, offer2));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(int offerId, string comment, int rating)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var existing = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == user.Id && r.OfferId == offerId);

            if (existing == null)
            {
                _context.Reviews.Add(new Review
                {
                    OfferId = offerId,
                    UserId = user.Id,
                    Comment = comment,
                    Rating = rating
                });
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Details", new { id = offerId });
        }
    }
}
// Review system added 
