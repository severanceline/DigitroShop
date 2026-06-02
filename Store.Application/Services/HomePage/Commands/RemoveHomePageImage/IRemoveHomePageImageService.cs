using Microsoft.VisualBasic;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.HomePage.Commands.RemoveHomePageImage
{
    public interface IRemoveHomePageImageService
    {
        ResultDto Execute(long Id);
    }
    public class RemoveHomePageImageService : IRemoveHomePageImageService
    {
        private readonly IDataBaseContext _context;
        public RemoveHomePageImageService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(long Id)
        {
            var homePageImage = _context.HomePageImages.Find(Id);

            if(homePageImage == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "عکس یافت نشد"
                };

            }

            homePageImage.IsRemoved = true;
            homePageImage.RemovedTime = DateTime.Now;

            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "عکس با موفقیت حذف شد"
            };
        }
    }
}
