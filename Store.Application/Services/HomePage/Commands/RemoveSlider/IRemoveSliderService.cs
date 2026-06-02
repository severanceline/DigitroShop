using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.HomePage.Commands.RemoveSlider
{
    public interface IRemoveSliderService
    {
        ResultDto Execute(long  id);
    }
    public class RemoveSliderService : IRemoveSliderService
    {
        private readonly IDataBaseContext _context;
        public RemoveSliderService(IDataBaseContext context)
        {
            _context = context;
        }
        public ResultDto Execute(long id)
        {
            var slider = _context.Sliders.Find(id);
            
            if(slider == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "اسلایدر با موفقیت یافت نشد"
                };
            }
            slider.IsRemoved = true;
            slider.RemovedTime = DateTime.Now;
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "اسلایدر با موفقیت حذف  شد"
            };
        }
    }
}
