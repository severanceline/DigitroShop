using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common;
using Store.Common.Dto;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Queries.GetProductForSite
{
    public interface IGetProductForSiteService
    {
        ResultDto<ResultProductForSiteDto> Execute(Ordering Ordering,string SearchKey,int Page,int PageSize,long? CatId);
    }


    public class GetProductForSiteService : IGetProductForSiteService
    {

        private readonly IDataBaseContext _context;
        public GetProductForSiteService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<ResultProductForSiteDto> Execute(Ordering Ordering,string SearchKey, int Page, int PageSize, long? CatId)
        {
            int totalRow = 0;
            var productQuery = _context.Products.Include(p => p.ProductImages).AsQueryable();
            if (CatId != null)
            {
                //میگیم برو دسته بندی ای که پرنت کتگوریش مساوی کت آیدی رو پیدا کن و
                //آیدیش رو تو این متغیر بریز
                var subCategoryIds = _context.Categories
                .Where(c => c.ParentCategoryId == CatId) 
                .Select(c => c.Id)
                .ToList();

                productQuery = productQuery.Where(p =>
                p.CategoryId == CatId || subCategoryIds.Contains(p.CategoryId)).AsQueryable();
            }
            if (!string.IsNullOrEmpty(SearchKey))
            {
                productQuery = productQuery.Where(p => p.Name.Contains(SearchKey) || p.Brand.Contains(SearchKey)).AsQueryable();
            }
            switch (Ordering)
            {
                case Ordering.NotOrder:
                    productQuery = productQuery.OrderByDescending(p => p.Id).AsQueryable();
                    break;
                case Ordering.MostVisited:
                    productQuery = productQuery.OrderByDescending(p => p.ViewCount).AsQueryable();
                    break;
                case Ordering.Bestseling:
                    break;
                case Ordering.MostPopular:
                    break;
                case Ordering.TheNewest:
                    productQuery = productQuery.OrderByDescending(p => p.Id).AsQueryable();
                    break;
                case Ordering.Cheapest:
                    productQuery = productQuery.OrderBy(p => p.Price).AsQueryable();
                    break;
                case Ordering.TheMostexpensive:
                    productQuery = productQuery.OrderByDescending(p => p.Price).AsQueryable();
                    break;
                default:
                    productQuery = productQuery.OrderByDescending(p => p.Id).AsQueryable();
                    break;
            }
            var product = productQuery.ToPaged(Page, PageSize, out totalRow);

            Random rd = new Random();
            return new ResultDto<ResultProductForSiteDto>
            {
                Data = new ResultProductForSiteDto
                {
                    TotalRow = totalRow,
                    Products = product.Select(p => new ProductForSiteDto
                    {
                        Id = p.Id,
                        Star = rd.Next(1, 5),
                        Title = p.Name,
                        ImageSrc = p.ProductImages.FirstOrDefault().Src,
                        Price = p.Price
                    }).ToList(),
                },
                IsSuccess = true,
            };
        }
    }

    public class ResultProductForSiteDto
    {

        public List<ProductForSiteDto> Products { get; set; }
        public int TotalRow { get; set; }
    }

    public class ProductForSiteDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ImageSrc { get; set; }
        public int Star { get; set; }
        public int Price { get; set; }
    }

    public enum Ordering
    {
        NotOrder = 0,
        /// <summary>
        /// پربازدیدترین
        /// </summary>
        MostVisited = 1,
        /// <summary>
        /// پرفروشترین
        /// </summary>
        Bestseling = 2,
        /// <summary>
        /// محبوبترین
        /// </summary>
        MostPopular = 3,
        /// <summary>
        /// جدیدترین
        /// </summary>
        TheNewest = 4,
        /// <summary>
        /// ارزانترین
        /// </summary>
        Cheapest = 5,
        /// <summary>
        /// گرانترین
        /// </summary>
        TheMostexpensive = 6
    }
}
