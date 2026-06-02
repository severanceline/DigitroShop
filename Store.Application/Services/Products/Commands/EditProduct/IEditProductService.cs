using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Application.Services.Products.Commands.AddNewProduct;
using Store.Application.Services.Products.Queries.GetProductDetailForAdmin;
using Store.Common.Dto;
using Store.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Store.Application.Services.Products.Commands.EditProduct
{
    public interface IEditProductService
    {
        ResultDto Execute(RequestEditProductDto request);

    }
    public class EditProductService : IEditProductService
    {
        private readonly IDataBaseContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EditProductService(IDataBaseContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public ResultDto Execute(RequestEditProductDto request)
        {
            try
            {
                var product = _context.Products
                    .Include(p => p.Category)
                    .ThenInclude(p => p.ParentCategory)
                    .Include(p => p.ProductFeatures)
                    .Include(p => p.ProductImages)
                    .Where(p => p.Id == request.Id)
                    .FirstOrDefault();

                if (product == null)
                {
                    return new ResultDto { IsSuccess = false, Message = "محصول یافت نشد." };
                }

                if (request.CategoryId.HasValue && request.CategoryId.Value != product.CategoryId)
                {
                    product.CategoryId = request.CategoryId.Value;
                }
                product.Brand = request.Brand ?? product.Brand;
                product.Description = request.Description ?? product.Description;
                product.Name = request.Name ?? product.Name;
                product.Price = request.Price ?? product.Price;
                product.Inventory = request.Inventory ?? product.Inventory;
                product.Displayed = request.Displayed ?? product.Displayed;

                if (request.RemovedFeatures != null)
                {
                    foreach (var item in request.RemovedFeatures)
                    {
                        var feature = _context.ProductFeatures.Where(p => p.Id == item).FirstOrDefault();
                        feature.IsRemoved = true;
                        feature.RemovedTime = DateTime.Now;
                    }
                }

                if (request.UpdatedFeatures != null)
                {
                    foreach (var item in request.UpdatedFeatures)
                    {
                        var feature = _context.ProductFeatures
                            .FirstOrDefault(x => x.Id == item.Id);

                        if (feature != null)
                        {
                            feature.DisplayName = item.DisplayName;
                            feature.Value = item.Value;
                        }
                    }
                }
                if (request.AddedFeatures != null)
                {
                    foreach (var item in request.AddedFeatures)
                    {
                        var feature = new ProductFeatures
                        {
                            DisplayName = item.DisplayName,
                            Value = item.Value,
                            Product =product,
                            ProductId = product.Id
                        };

                        _context.ProductFeatures.Add(feature);
                    }
                }

                if (request.RemovedImages != null)
                {
                    foreach (var item in request.RemovedImages)
                    {
                        var image = _context.ProductImages.Where(p => p.Id == item).FirstOrDefault();
                        if (image != null)
                        {
                            image.IsRemoved = true;
                            image.RemovedTime = DateTime.Now;
                            DeleteFile(_webHostEnvironment, image.Src);
                        }
                    }
                }

                if (request.Images != null && request.Images.Any())
                {
                    List<ProductImages> productImages = new List<ProductImages>();
                    foreach (var item in request.Images)
                    {
                        var uploadedResult = UploadFile(item);
                        productImages.Add(new ProductImages
                        {
                            Product = product,
                            Src = uploadedResult.FileNameAddress,
                        });
                    }
                    _context.ProductImages.AddRange(productImages);
                }

                product.UpdateTime = DateTime.Now;

                _context.SaveChanges();

                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "محصول با موفقیت ویرایش شد",
                };
            }
            catch (Exception ex)
            {

                return new ResultDto
                {
                    IsSuccess = false,
                    Message = $"خطا: {ex.Message}"
                };
            }
        }
        private UploadDto UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string folder = $@"images\ProductImages\";
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
         private void DeleteFile(IWebHostEnvironment webHostEnvironment, string filePath)
         {
            var fullPath = Path.Combine(webHostEnvironment.WebRootPath, filePath);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
         }
    }
    public class UploadDto
    {
        public long Id { get; set; }
        public bool Status { get; set; }
        public string FileNameAddress { get; set; }
    }

    public class RequestEditProductDto
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Brand { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public int? Inventory { get; set; }
        public long? CategoryId { get; set; }
        public bool? Displayed { get; set; }
        public List<long>? RemovedImages { get; set; }
        public List<IFormFile>? Images { get; set; }

        public List<EditProduct_Features>? AddedFeatures { get; set; }
        public List<UpdateFeatureDto>? UpdatedFeatures { get; set; }
        public List<long>? RemovedFeatures { get; set; }
    }

    public class EditProduct_Features
    {
        public string DisplayName { get; set; }
        public string Value { get; set; }
    }
    public class UpdateFeatureDto
    {
        public long Id { get; set; }

        public string DisplayName { get; set; }

        public string Value { get; set; }
    }

}
