@model IEnumerable<InsuranceComparisonService.Models.InsuranceOffer>
@{
    ViewData["Title"] = "Начало";
}

<div class="py-5 bg-primary text-white rounded mb-5 text-center">
    <h1 class="display-5 fw-bold"><i class="bi bi-shield-check"></i> Сравни застраховките си</h1>
    <p class="lead">Намери най-добрата оферта за Каско и Здравна застраховка</p>
    <a asp-controller="Insurance" asp-action="Kasko" class="btn btn-light btn-lg me-2">
        <i class="bi bi-car-front"></i> Каско
    </a>
    <a asp-controller="Insurance" asp-action="Health" class="btn btn-outline-light btn-lg">
        <i class="bi bi-heart-pulse"></i> Здравна
    </a>
</div>

<div class="row text-center mb-5">
    <div class="col-md-4">
        <div class="p-3">
            <i class="bi bi-search display-4 text-primary"></i>
            <h5 class="mt-2 fw-bold">Търси</h5>
            <p class="text-muted">Разгледай всички налични застрахователни оферти от водещи компании</p>
        </div>
    </div>
    <div class="col-md-4">
        <div class="p-3">
            <i class="bi bi-bar-chart display-4 text-primary"></i>
            <h5 class="mt-2 fw-bold">Сравни</h5>
            <p class="text-muted">Сравни две оферти едновременно по всички параметри</p>
        </div>
    </div>
    <div class="col-md-4">
        <div class="p-3">
            <i class="bi bi-heart display-4 text-primary"></i>
            <h5 class="mt-2 fw-bold">Запази</h5>
            <p class="text-muted">Запази любимите си оферти за по-лесен достъп</p>
        </div>
    </div>
</div>

<h4 class="mb-3 fw-bold">Последни оферти</h4>
<div class="row">
    @foreach (var offer in Model.Take(4))
    {
        <div class="col-md-3 mb-3">
            <div class="card h-100 shadow-sm">
                <div class="card-header bg-primary text-white">
                    @(offer.Type == InsuranceComparisonService.Models.InsuranceType.Kasko ? "Каско" : "Здравна")
                </div>
                <div class="card-body">
                    <h6 class="card-title fw-bold">@offer.Title</h6>
                    <p class="text-muted small">@offer.Company?.Name</p>
                    <p class="text-primary fw-bold fs-5">@offer.Price лв./год.</p>
                    <p class="card-text small">@offer.Description</p>
                </div>
                <div class="card-footer">
                    <a asp-controller="Insurance" asp-action="Details" asp-route-id="@offer.Id"
                       class="btn btn-sm btn-outline-primary w-100">Детайли</a>
                </div>
            </div>
        </div>
    }
</div>