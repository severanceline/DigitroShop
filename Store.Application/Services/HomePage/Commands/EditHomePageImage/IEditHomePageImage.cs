using Azure.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Store.Application.Interfaces.Contexts;
using Store.Application.Services.Products.Commands.AddNewProduct;
using Store.Common.Dto;
using Store.Domain.Entities.HomePage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.HomePage.Commands.EditHomePageImage
{
    public interface IEditHomePageImage
    {
        ResultDto Execute(RequestEditHomePageImageDto request);
    }
    public class EditHomePageImage : IEditHomePageImage
    {
        private readonly IDataBaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EditHomePageImage(IDataBaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public ResultDto Execute(RequestEditHomePageImageDto request)
        {
            var image = _context.HomePageImages.Find(request.Id);

            if (image == null)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                    Message = "عکس با موفقیت یافت نشد."
                };
            }
            if (request.File != null)
            {
                var resultUpload = UploadFile(request.File);
                image.Src = resultUpload.FileNameAddress;
            }
            if (request.Link != null)
            {
                image.Link = request.Link;
            }
            if (request.ImageLocation != null)
            {
                image.ImageLocation = request.ImageLocation.Value;
            }

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "عکس با موفقیت ویرایش شد."
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

    public class RequestEditHomePageImageDto
    {
        public long Id { get; set; }
        public IFormFile? File { get; set; }
        public ImageLocation? ImageLocation { get; set; }
        public string? Link { get; set; }
    }
}
