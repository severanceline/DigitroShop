using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.HomePage.Commands.EditHomePageImage;
using Store.Application.Services.HomePage.Commands.RemoveHomePageImage;
using Store.Application.Services.HomePage.Queries.GetHomePageImages;
using Store.Application.Services.HomePages.AddHomePageImages;
using Store.Domain.Entities.HomePage;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Operator")]
    [Area("Admin")]
    public class HomePageImagesController : Controller
    {
        private readonly IAddHomePageImagesService _addHomePageImagesService;
        private readonly IGetHomePageImagesService _getHomePageImagesService;
        private readonly IRemoveHomePageImageService _removeHomePageImageService;
        private readonly IEditHomePageImage _editHomePageImage;
        public HomePageImagesController(IAddHomePageImagesService addHomePageImagesService,
            IGetHomePageImagesService getHomePageImagesService,
             IRemoveHomePageImageService removeHomePageImageService,
             IEditHomePageImage editHomePageImage)
        {
            _addHomePageImagesService = addHomePageImagesService;
            _getHomePageImagesService = getHomePageImagesService;
            _removeHomePageImageService = removeHomePageImageService;
            _editHomePageImage = editHomePageImage;
        }
         
        public IActionResult Index()
        {
            return View(_getHomePageImagesService.Execute().Data);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(IFormFile file, string link, ImageLocation imageLocation)
        {
            _addHomePageImagesService.Execute(new requestAddHomePageImagesDto
            {
                File = file,
                Link = link,
                ImageLocation = imageLocation
            });
            return View();
        }

        public IActionResult Remove(long Id)
        {
            return Json(_removeHomePageImageService.Execute(Id));
        }

        [HttpPost]
        public IActionResult Edit([FromForm]RequestEditHomePageImageDto request)
        {
            return Json(_editHomePageImage.Execute(request));
        }
    }
}
