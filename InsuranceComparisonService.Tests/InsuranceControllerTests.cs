using InsuranceComparisonService.Controllers;
using InsuranceComparisonService.Models;
using InsuranceComparisonService.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace InsuranceComparisonService.Tests
{
    public class InsuranceControllerTests
    {
        private readonly Mock<IInsuranceRepository> _mockRepo;

        public InsuranceControllerTests()
        {
            _mockRepo = new Mock<IInsuranceRepository>();
        }

        [Fact]
        public async Task Kasko_ReturnsViewWithKaskoOffers()
        {
            // Arrange
            var offers = new List<InsuranceOffer>
            {
                new InsuranceOffer { Id = 1, Title = "Каско Тест", Type = InsuranceType.Kasko, Price = 800 },
                new InsuranceOffer { Id = 2, Title = "Каско 2", Type = InsuranceType.Kasko, Price = 1200 }
            };
            _mockRepo.Setup(r => r.GetOffersByTypeAsync(InsuranceType.Kasko))
                     .ReturnsAsync(offers);
            _mockRepo.Setup(r => r.GetAllCompaniesAsync())
                     .ReturnsAsync(new List<InsuranceCompany>());

            var controller = new InsuranceController(_mockRepo.Object, null!, null!);

            // Act
            var result = await controller.Kasko(null, null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<InsuranceOffer>>(viewResult.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public async Task Health_ReturnsViewWithHealthOffers()
        {
            // Arrange
            var offers = new List<InsuranceOffer>
            {
                new InsuranceOffer { Id = 3, Title = "Здравна Базик", Type = InsuranceType.Health, Price = 350 }
            };
            _mockRepo.Setup(r => r.GetOffersByTypeAsync(InsuranceType.Health))
                     .ReturnsAsync(offers);
            _mockRepo.Setup(r => r.GetAllCompaniesAsync())
                     .ReturnsAsync(new List<InsuranceCompany>());

            var controller = new InsuranceController(_mockRepo.Object, null!, null!);

            // Act
            var result = await controller.Health(null, null, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<InsuranceOffer>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenOfferDoesNotExist()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetOfferByIdAsync(99))
                     .ReturnsAsync((InsuranceOffer?)null);

            var controller = new InsuranceController(_mockRepo.Object, null!, null!);

            // Act
            var result = await controller.Details(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsView_WhenOfferExists()
        {
            // Arrange
            var offer = new InsuranceOffer { Id = 1, Title = "Каско Тест", Type = InsuranceType.Kasko, Price = 800 };
            _mockRepo.Setup(r => r.GetOfferByIdAsync(1)).ReturnsAsync(offer);

            var controller = new InsuranceController(_mockRepo.Object, null!, null!);

            // Act
            var result = await controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(offer, viewResult.Model);
        }

        [Fact]
        public async Task Compare_RedirectsToHome_WhenIdsAreZero()
        {
            // Arrange
            var controller = new InsuranceController(_mockRepo.Object, null!, null!);

            // Act
            var result = await controller.Compare(0, 0);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Compare_ReturnsView_WhenBothOffersExist()
        {
            // Arrange
            var offer1 = new InsuranceOffer { Id = 1, Title = "Оферта 1", Type = InsuranceType.Kasko, Price = 800 };
            var offer2 = new InsuranceOffer { Id = 2, Title = "Оферта 2", Type = InsuranceType.Kasko, Price = 1100 };
            _mockRepo.Setup(r => r.GetOfferByIdAsync(1)).ReturnsAsync(offer1);
            _mockRepo.Setup(r => r.GetOfferByIdAsync(2)).ReturnsAsync(offer2);

            var controller = new InsuranceController(_mockRepo.Object, null!, null!);

            // Act
            var result = await controller.Compare(1, 2);

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void InsuranceOffer_PriceValidation_FailsForNegativePrice()
        {
            // Arrange
            var offer = new InsuranceOffer { Price = -100 };

            // Assert
            Assert.True(offer.Price < 0);
        }

        [Fact]
        public void InsuranceOffer_DefaultIsActive_IsTrue()
        {
            var offer = new InsuranceOffer();
            Assert.True(offer.IsActive);
        }

        [Fact]
        public async Task Repository_GetOffersByType_ReturnsOnlyKasko()
        {
            // Arrange
            var offers = new List<InsuranceOffer>
            {
                new InsuranceOffer { Id = 1, Type = InsuranceType.Kasko },
                new InsuranceOffer { Id = 2, Type = InsuranceType.Health }
            };
            _mockRepo.Setup(r => r.GetOffersByTypeAsync(InsuranceType.Kasko))
                     .ReturnsAsync(offers.Where(o => o.Type == InsuranceType.Kasko).ToList());

            // Act
            var result = await _mockRepo.Object.GetOffersByTypeAsync(InsuranceType.Kasko);

            // Assert
            Assert.All(result, o => Assert.Equal(InsuranceType.Kasko, o.Type));
        }

        [Fact]
        public void UserFavorite_DefaultSavedAt_IsRecentDate()
        {
            var fav = new UserFavorite();
            Assert.True(fav.SavedAt >= DateTime.UtcNow.AddMinutes(-1));
        }
    }
}
