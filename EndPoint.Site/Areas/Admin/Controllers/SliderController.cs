using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.HomePage.Commands.AddNewSlider;
using Store.Application.Services.HomePage.Commands.EditSlider;
using Store.Application.Services.HomePage.Commands.RemoveSlider;
using Store.Application.Services.HomePage.Queries.GetSlider;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Operator")]
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly IAddNewSliderService _addNewSliderService;
        private readonly IGetSliderService _getSliderService;
        private readonly IRemoveSliderService _removeSliderService;
        private readonly IEditSliderService _editSliderService;

        public SliderController(IAddNewSliderService addNewSliderService,
            IGetSliderService getSliderService,
            IEditSliderService editSliderService,
            IRemoveSliderService removeSliderService)
        {
            _addNewSliderService = addNewSliderService;
            _getSliderService = getSliderService;
            _editSliderService = editSliderService;
            _removeSliderService = removeSliderService;
        }
        public IActionResult Index()
        {
            return View(_getSliderService.Execute().Data);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(IFormFile file, string link)
        {
            _addNewSliderService.Execute(file, link);
            return View();
        }

        public IActionResult Remove(long id)
        {
            return Json(_removeSliderService.Execute(id));
        }

        public IActionResult Edit(RequestEditSlider request)
        {
            return Json(_editSliderService.Execute(request));
        }
    }
}
