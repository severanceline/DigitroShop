using EndPoint.Site.Models;
using EndPoint.Site.Models.ViewModels.HomePage;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.FacadPatterns;
using Store.Application.Services.HomePage.Queries.GetHomePageImages;
using Store.Application.Services.HomePage.Queries.GetSlider;
using System.Diagnostics;
using Store.Application.Services.Products.Queries.GetProductForSite;


namespace EndPoint.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGetSliderService _getSliderService;
        private readonly IGetHomePageImagesService _getpageImagesService;
        private readonly IProductFacad _productFacad;

        public HomeController(ILogger<HomeController> logger, IGetSliderService getSliderService,
            IGetHomePageImagesService getpageImagesService,
            IProductFacad productFacad)
        {
            _logger = logger;
            _getSliderService = getSliderService;
            _getpageImagesService = getpageImagesService;
            _productFacad = productFacad;
        }

        public IActionResult Index()
        {
            HomePageViewModel homePage = new HomePageViewModel()
            {
                Sliders = _getSliderService.Execute().Data,
                PageImages = _getpageImagesService.Execute().Data,
                Phone = _productFacad.GetProductForSiteService.Execute(Ordering.TheNewest,null,1,6,10033).Data.Products,
                Laptop = _productFacad.GetProductForSiteService.Execute(Ordering.TheNewest,null,1,6,10034).Data.Products,
            };
            return View(homePage);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
