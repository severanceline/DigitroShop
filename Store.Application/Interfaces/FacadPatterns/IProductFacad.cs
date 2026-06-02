using Microsoft.AspNetCore.Hosting;
using Store.Application.Services.Products.Commands.AddCategory;
using Store.Application.Services.Products.Commands.AddNewProduct;
using Store.Application.Services.Products.Commands.EditCategory;
using Store.Application.Services.Products.Commands.EditProduct;
using Store.Application.Services.Products.Commands.RemoveCategory;
using Store.Application.Services.Products.Commands.RemoveProduct;
using Store.Application.Services.Products.Queries.GetAllCategories;
using Store.Application.Services.Products.Queries.GetCategoryList;
using Store.Application.Services.Products.Queries.GetProductDetailForAdmin;
using Store.Application.Services.Products.Queries.GetProductDetailForSite;
using Store.Application.Services.Products.Queries.GetProductForAdmin;
using Store.Application.Services.Products.Queries.GetProductForSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Interfaces.FacadPatterns
{
    public interface IProductFacad
    {
        IAddCategoryService AddCategoryService { get; }
        IGetCategoryListService GetCategoryListService { get; }
        IRemoveCategoryService RemoveCategoryService { get; }
        IEditCategoryService EditCategoryService { get; }

        IAddNewProductService AddNewProductService { get; }
        IGetAllCategoriesService GetAllCategoriesService { get; }
        /// <summary>
        /// دریافت لیست محصولات برا ادمین
        /// </summary>
        IGetProductForAdminService GetProductForAdminService { get; }
        /// <summary>
        /// دریافت جزیات محصول 
        /// </summary>
        IGetProductDetailForAdminService GetProductDetailForAdminService { get; }
        /// <summary>
        /// حذف محصول
        /// </summary>
        IRemoveProductService RemoveProductService { get; }
        IEditProductService EditProductService { get; }
        IGetProductForSiteService GetProductForSiteService { get; }
        IGetProductDetailForSiteService GetProductDetailForSiteService { get; }
    }
}
