using Store.Application.Services.Products.Queries.GetProductDetailForSite;
using Microsoft.AspNetCore.Hosting;
using Store.Application.Interfaces.Contexts;
using Store.Application.Interfaces.FacadPatterns;
using Store.Application.Services.Products.Commands.AddCategory;
using Store.Application.Services.Products.Commands.AddNewProduct;
using Store.Application.Services.Products.Commands.EditCategory;
using Store.Application.Services.Products.Commands.EditProduct;
using Store.Application.Services.Products.Commands.RemoveCategory;
using Store.Application.Services.Products.Commands.RemoveProduct;
using Store.Application.Services.Products.Queries.GetAllCategories;
using Store.Application.Services.Products.Queries.GetCategoryList;
using Store.Application.Services.Products.Queries.GetProductDetailForAdmin;
using Store.Application.Services.Products.Queries.GetProductForAdmin;
using Store.Application.Services.Products.Queries.GetProductForSite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.FacadPattern
{
    public class ProductFacad : IProductFacad
    {
        private readonly IDataBaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductFacad(IDataBaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private IAddCategoryService _addCategoryService;
        public IAddCategoryService AddCategoryService
        {
            get
            {
                return _addCategoryService = _addCategoryService ?? new AddCategoryService(_context);
            }
        }

        private IGetCategoryListService _getCategoryListService;
        public IGetCategoryListService GetCategoryListService
        {
            get
            {
                return _getCategoryListService = _getCategoryListService ?? new GetCategoryListService(_context);
            }
        }

        private IRemoveCategoryService _removeCategoryService;
        public IRemoveCategoryService RemoveCategoryService
        {
            get
            {
                return _removeCategoryService = _removeCategoryService ?? new RemoveCategoryService(_context);
            }
        }

        private IEditCategoryService _editCategoryService;
        public IEditCategoryService EditCategoryService
        {
            get
            {
                return _editCategoryService = _editCategoryService ?? new EditCategoryService(_context);
            }
        }

        private IAddNewProductService _addNewProductService;
        public IAddNewProductService AddNewProductService
        {
            get
            {
                return _addNewProductService = _addNewProductService ?? new AddNewProductService(_context, _webHostEnvironment);
            }
        }

        private IGetAllCategoriesService _getAllCategoriesService;
        public IGetAllCategoriesService GetAllCategoriesService
        {
            get
            {
                return _getAllCategoriesService = _getAllCategoriesService ?? new GetAllCategoriesService(_context);
            }
        }

        private IGetProductForAdminService _getProductForAdminService;
        public IGetProductForAdminService GetProductForAdminService
        {
            get
            {
                return _getProductForAdminService = _getProductForAdminService ?? new GetProductForAdminService(_context);
            }
        }

        private IGetProductDetailForAdminService _getProductDetailForAdminService;
        public IGetProductDetailForAdminService GetProductDetailForAdminService
        {
            get
            {
                return _getProductDetailForAdminService = _getProductDetailForAdminService ?? new GetProductDetailForAdminService(_context);
            }
        }

        private IRemoveProductService _removeProductService;
        public IRemoveProductService RemoveProductService
        {
            get
            {
                return _removeProductService = _removeProductService ?? new RemoveProductService(_context);
            }
        }

        private IEditProductService _editProductService;
        public IEditProductService EditProductService
        {
            get
            {
                return _editProductService = _editProductService ?? new EditProductService(_context, _webHostEnvironment);
            }
        }

        private IGetProductForSiteService _getProductForSiteService;
        public IGetProductForSiteService GetProductForSiteService
        {
            get
            {
                return _getProductForSiteService = _getProductForSiteService ?? new GetProductForSiteService(_context);
            }
        }

        private IGetProductDetailForSiteService _getProductDetailForSiteService;
        public IGetProductDetailForSiteService GetProductDetailForSiteService
        {
            get
            {
                return _getProductDetailForSiteService = _getProductDetailForSiteService ?? new GetProductDetailForSiteService(_context);
            }
        }
    }
}
