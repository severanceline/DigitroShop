using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Commands.AddCategory
{
    public interface IAddCategoryService
    {
        ResultDto Execute(string Name,long? ParentId);
    }


    public class AddCategoryService : IAddCategoryService
    {
        private readonly IDataBaseContext _context;
        public AddCategoryService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(string Name, long? ParentId)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "لطفا نام دسته بندی را وارد کنید. "
                };
            }

            Category cat1 = new Category
            {
                Name = Name,
                ParentCategory = GetParrent(ParentId),
            };
            _context.Categories.Add(cat1);
            _context.SaveChanges();

            return new ResultDto
            {
                IsSuccess = true,
                Message = "دسته بندی با موفقیت افزوده شد "
            };
        }
        private Category GetParrent(long? parentId)
        {
            return _context.Categories.Find(parentId);
        }
    }   
}
