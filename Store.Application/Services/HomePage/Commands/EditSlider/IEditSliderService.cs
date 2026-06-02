using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Store.Application.Interfaces.Contexts;
using Store.Application.Services.Products.Commands.AddNewProduct;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.HomePage.Commands.EditSlider
{
    public interface IEditSliderService
    {
        ResultDto Execute(RequestEditSlider request);
    }
    public class EditSliderService : IEditSliderService
    {
        private readonly IDataBaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditSliderService(IDataBaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public ResultDto Execute(RequestEditSlider request)
        {
            var slider = _context.Sliders.Find(request.Id);

            if (slider == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "اسلایدر با موفقیت یافت نشد"
                };
            }

            if(request.File != null)
            {
                var resultUpload = UploadFile(request.File);
                slider.Src = resultUpload.FileNameAddress;
            }
            if(request.Link != null)
            {
                slider.Link = request.Link;
            }

            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = "اسلایدر با موفقیت ویرایش شد"
            };
        }
        private UploadDto UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string folder = $@"images\HomePages\Slider\";
                var uploadsRootFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                if (!Directory.Exists(uploadsRootFolder))
                {
                    Directory.CreateDirectory(uploadsRootFolder);
                }


                if (file == null || file.Length == 0)
                {
                    return new UploadDto()
                    {
                        Status = false,
                        FileNameAddress = "",
                    };
                }

                string fileName = DateTime.Now.Ticks.ToString() + file.FileName;
                var filePath = Path.Combine(uploadsRootFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                return new UploadDto()
                {
                    FileNameAddress = folder + fileName,
                    Status = true,
                };
            }
            return null;
        }
    }
    public class RequestEditSlider
    {
        public long Id { get; set; }
        public string? Link { get; set; }
        public IFormFile? File { get; set; }
    }
}
