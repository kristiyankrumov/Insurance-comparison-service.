using InsuranceComparisonService.Models;
using InsuranceComparisonService.Models.ViewModels;
using InsuranceComparisonService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace InsuranceComparisonService.Controllers
{
    public class HomeController : Controller
    {
        private readonly IInsuranceRepository _repo;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IInsuranceRepository repo, ILogger<HomeController> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var offers = await _repo.GetAllOffersAsync();

            var cards = offers
                .Where(o => o.IsActive)
                .OrderBy(o => o.FinalPrice)
                .Take(6)
                .Select(o => new OfferCardViewModel
                {
                    Id          = o.Id,
                    Title       = o.Title,
                    CompanyName = o.Company?.Name ?? "",
                    Price       = o.FinalPrice,
                    Description = o.Description,
                    TypeName = o.Type switch
                    {
                        InsuranceType.Kasko    => "Kasko",
                        InsuranceType.Health   => "Health",
                        InsuranceType.Civil    => "Civil",
                        InsuranceType.Property => "Property",
                        _                      => o.Type.ToString()
                    },
                    TypeBadge = o.Type switch
                    {
                        InsuranceType.Kasko    => "bg-primary",
                        InsuranceType.Health   => "bg-danger",
                        InsuranceType.Civil    => "bg-warning text-dark",
                        InsuranceType.Property => "bg-success",
                        _                      => "bg-secondary"
                    }
                }).ToList();

            _logger.LogInformation("Home page loaded {Count} active offers", cards.Count);
            return View(cards);
        }

        [Route("Home/Error/{statusCode?}")]
        public IActionResult Error(int? statusCode)
        {
            if (statusCode == 404) return View("Error404");
            return View("Error500");
        }
    }
}
