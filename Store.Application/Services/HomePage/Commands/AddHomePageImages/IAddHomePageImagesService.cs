using Store.Application.Interfaces.Contexts;
using Store.Application.Services.Products.Commands.AddNewProduct;
using Store.Common.Dto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Domain.Entities.HomePage;

namespace Store.Application.Services.HomePages.AddHomePageImages
{
    public interface IAddHomePageImagesService
    {
        ResultDto Execute(requestAddHomePageImagesDto request);
    }


    public class AddHomePageImagesService : IAddHomePageImagesService
    {
        private readonly IDataBaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddHomePageImagesService(IDataBaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public ResultDto Execute(requestAddHomePageImagesDto request)
        {

            var resultUpload = UploadFile(request.File);

            HomePageImages homePageImages = new HomePageImages()
            {
                Link = request.Link,
                Src = resultUpload.FileNameAddress,
                ImageLocation = request.ImageLocation,
            };
            _context.HomePageImages.Add(homePageImages);
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
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

    public class requestAddHomePageImagesDto
    {
        public IFormFile File { get; set; }
        public string Link { get; set; }
        public ImageLocation ImageLocation{ get; set; }
    }
}
