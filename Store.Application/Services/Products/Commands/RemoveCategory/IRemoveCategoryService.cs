using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Commands.RemoveCategory
{
    public interface IRemoveCategoryService
    {
        ResultDto Execute(long CategoryId);
    }
    public class RemoveCategoryService : IRemoveCategoryService
    {
        private readonly IDataBaseContext _context;
        public RemoveCategoryService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(long CategoryId)
        {
            var category = _context.Categories.Include(p => p.SubCategories).FirstOrDefault(p => p.Id == CategoryId);
            if(category.SubCategories.Count() == 0)
            {
                category.IsRemoved = true;
                _context.SaveChanges();
                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "دسته بندی با موفقیت حذف شد"
                };
            }
            else
            {
                category.IsRemoved = true;
                foreach (var subCategory in category.SubCategories)
                {
                    subCategory.IsRemoved = true;
                }
                _context.SaveChanges();

                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "دسته بندی و تمام زیر گروه های آن با موفقیت حذف شد"
                };
            }
        }
    }
}
