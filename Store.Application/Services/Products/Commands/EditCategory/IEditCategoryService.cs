using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Application.Services.Products.Queries.GetCategoryList;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Commands.EditCategory
{
    public interface IEditCategoryService
    {
        ResultDto<List<CategoriesrDto>> Execute(RequestEditCategoryDto? request);
    }
    public class EditCategoryService : IEditCategoryService
    {
        private readonly IDataBaseContext _context;
        public EditCategoryService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto<List<CategoriesrDto>> Execute(RequestEditCategoryDto? request)
        {
            //استفاده از این سرویس برای پرکردن کشوی دسته ببندی هایی که والد ندارند
            if (request == null)
            {

                List<CategoriesrDto> categorylist = _context.Categories.Where(p => p.ParentCategoryId == null).Select(p =>
                new CategoriesrDto
                {
                    Id = p.Id,
                    Name = p.Name,
                }).ToList();

                return new ResultDto<List<CategoriesrDto>>
                {
                    Data = categorylist,
                    IsSuccess = true,
                    Message = ""
                };
            }
            //کاربر برای ثبت ویرایش
            var category = _context.Categories.Include(p => p.SubCategories).Where(p => p.Id == request.Id).FirstOrDefault();
            if (request.ParentId != null)
            {
                //چون فلوغنت ولیدییشن هارو اعمال نکردم اینو اینچجا گذاشتم وگرنه ان جاش اینجا نیست 
                if (request.Id == request.ParentId)
                {
                    return new ResultDto<List<CategoriesrDto>>
                    {
                        IsSuccess = false,
                        Message = "یک دسته نمی‌تواند والد خودش باشد"
                    };
                }
                //اگر کاربری که داشت ویرایش میشد فرزند داشت و خواست والدش رو تغییر بده یعنی چیزی به غیر از 
                //نال بزاره

                if(category.SubCategories.Count() > 0)
                {
                    return new ResultDto<List<CategoriesrDto>>
                    {
                        IsSuccess = false,
                        Message = "این دسته بندی دارای زیرگروه میباشئد وایجاد دسته بندی فقط در دوسطح ممکن است."
                    };
                }
                category.ParentCategoryId = request.ParentId;
            }

            else
            {
                category.ParentCategoryId = null;
            }
            category.Name = request.Name;

            _context.SaveChanges();

            return new ResultDto<List<CategoriesrDto>>
            {
                Data = null,
                IsSuccess = true,
                Message = "دسته بندی با موفقیت ویرایش شد."
            };
        }
    }
    public class RequestEditCategoryDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
    }
    public class CategoriesrDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
