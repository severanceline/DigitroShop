using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Application.Interfaces.FacadPatterns;
using Store.Application.Services.Products.Commands.AddNewProduct;
using Store.Application.Services.Products.Commands.EditProduct;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Operator")]
    [Area("Admin")]
    public class ProductsController : Controller
    {

        private readonly IProductFacad _productFacad;
        public ProductsController(IProductFacad productFacad)
        {
            _productFacad = productFacad;
        }

        public IActionResult Index(int Page = 1, int PageSize = 20)
        {
            return View(_productFacad.GetProductForAdminService.Execute(Page, PageSize).Data);
        }

        public IActionResult Detail(long Id)
        {
            return View(_productFacad.GetProductDetailForAdminService.Execute(Id).Data);
        }

        [HttpGet]
        public IActionResult AddNewProduct()
        {
            ViewBag.Categories = new SelectList(_productFacad.GetAllCategoriesService.Execute().Data,"Id","Name");
            return View();
        }

        [HttpPost]
        public IActionResult AddNewProduct(RequestAddNewProductDto request)
        {
            //List<IFormFile> images = new List<IFormFile>();
            //for(int i = 0; i < Request.Form.Files.Count; i++)
            //{
            //    var file = Request.Form.Files[i];
            //    images.Add(file);
            //}
            //request.Features = Features;
            //request.Images = images;
            return Json(_productFacad.AddNewProductService.Execute(request));
        }

        [HttpPost]
        public IActionResult RemoveProduct(long Id)
        {
            return Json(_productFacad.RemoveProductService.Execute(Id));
        }

        [HttpGet]
        public IActionResult EditProduct(long Id)
        {
            var product = _productFacad.GetProductDetailForAdminService.Execute(Id).Data;

            ViewBag.Categories = new SelectList(
                _productFacad.GetAllCategoriesService.Execute().Data,
                "Id",
                "Name",
                product.CategoryId
            );

            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct([FromForm] RequestEditProductDto request)
        {
            return Json(_productFacad.EditProductService.Execute(request));
        }
    }
}
