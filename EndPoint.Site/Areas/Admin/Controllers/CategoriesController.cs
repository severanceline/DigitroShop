using Azure.Core;
using EndPoint.Site.Areas.Admin.Models.ViewModels.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.FacadPatterns;
using Store.Application.Services.Products.Commands.AddCategory;
using Store.Application.Services.Products.Commands.EditCategory;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Authorize(Policy = "Admin")]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IProductFacad _productFacad;
        public CategoriesController(IProductFacad productFacad)
        {
            _productFacad = productFacad;
        }

        public IActionResult Index(long? parentId)
        {
            return View(_productFacad.GetCategoryListService.Execute(parentId).Data);
        }

        [HttpGet]
        public IActionResult CreatCategory(long? parentId)
        {
            ViewBag.parentId = parentId;
            return View();
        }

        [HttpPost] 
        public IActionResult CreatCategory(string Name,long? ParentId)
        {
            var result = _productFacad.AddCategoryService.Execute(Name,ParentId);
            return Json(result);
        }

        [HttpPost] 
        public IActionResult RemoveCategory(long CategoryId)
        {   
            return Json(_productFacad.RemoveCategoryService.Execute(CategoryId));
        }

        [HttpGet] 
        public IActionResult EditCategory(long CategoryId,string Name)
        {
            ViewBag.CategoryId = CategoryId;
            ViewBag.Name = Name;

            var categories = _productFacad.EditCategoryService.Execute(null).Data;

            return View(categories);
        }
        [HttpPost] 
        public IActionResult EditCategory(EditRequestViewModel request)
        {
            RequestEditCategoryDto request1 = new RequestEditCategoryDto
            {
                Id = request.Id,
                Name = request.Name,
                ParentId = request.ParentId
            };
            return Json(_productFacad.EditCategoryService.Execute(request1));
        }
    }
}
