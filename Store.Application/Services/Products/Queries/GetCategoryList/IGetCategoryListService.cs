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

namespace Store.Application.Services.Products.Queries.GetCategoryList
{
    public interface IGetCategoryListService
    {
        ResultDto<List<GetCategoryResultDto>> Execute(long? ParentId);
    }
    public class GetCategoryListService : IGetCategoryListService
    {
        private readonly IDataBaseContext _context;
        public GetCategoryListService(IDataBaseContext context)
        {
            _context = context;
        }
        //با این کد در واقع پرننت آیدی رو بهش میدی م و
        //میگیم بچه های این رو برامون بیار
        //میتونیم هم نگیم یعنی بچه ای نداشته باشه 
        public ResultDto<List<GetCategoryResultDto>> Execute(long? ParentId)
        {
            if(ParentId == null)
            {
                var allCategories = _context.Categories.Select(p => new GetCategoryResultDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Parent = p.ParentCategory != null ? new
                    ParentCategoryDto
                    {
                        Id = p.ParentCategory.Id,
                        Name = p.ParentCategory.Name
                    }
                    : null,
                    HasChild = p.SubCategories.Count() > 0 ? true : false,
                }).ToList();
                return new ResultDto<List<GetCategoryResultDto>>
                {
                    Data = allCategories,
                    IsSuccess = true,
                    Message = "لیست با موفقیت برگشت داده شد."
                };
            }
            var categories = _context.Categories
            .Include(p => p.ParentCategory)
            .Include(p => p.SubCategories)
            .Where(p => p.ParentCategoryId == ParentId)
            .ToList()
            .Select(p => new GetCategoryResultDto
            {
                Id = p.Id,
                Name = p.Name,
                Parent = p.ParentCategory != null? new
                ParentCategoryDto
                {
                    Id = p.ParentCategory.Id,
                    Name = p.ParentCategory.Name
                }
                : null,
                HasChild = p.SubCategories.Count() > 0 ? true : false,
            }).ToList();

            return new ResultDto<List<GetCategoryResultDto>>
            {
                Data = categories,
                IsSuccess = true,
                Message = "لیست با موفقیت برگشت داده شد."
            };
        }
    }
    public class GetCategoryResultDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ParentCategoryDto Parent { get; set; }
        public bool HasChild { get; set; }
    }

    public class ParentCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
