using Store.Application.Services.HomePage.Queries.GetHomePageImages;
using Store.Application.Services.HomePage.Queries.GetSlider;
using Store.Application.Services.Products.Queries.GetProductForSite;

namespace EndPoint.Site.Models.ViewModels.HomePage
{
    public class HomePageViewModel
    {
        public List<SliderDto> Sliders { get; set; }
        public List<HomePageImagesDto> PageImages { get; set; }
        public List<ProductForSiteDto> Phone { get; set; }
        public List<ProductForSiteDto> Laptop { get; set; }
    }
}
