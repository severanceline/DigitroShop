using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Products.Commands.RemoveProduct
{
    public interface IRemoveProductService
    {
        ResultDto Execute(long Id);
    }
    public class RemoveProductService : IRemoveProductService
    {
        private readonly IDataBaseContext _context;
        public RemoveProductService(IDataBaseContext contex)
        {
            _context = contex;
        }
        public ResultDto Execute(long Id)
        {
            var product = _context.Products.Include(p => p.ProductFeatures)
                .Include(p => p.ProductImages).Where(p => p.Id == Id)
                .FirstOrDefault();
            if(product == null)
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "محصول یافت نشد."
                };
            }
            if (product.ProductFeatures != null)
            {
                foreach(var item in product.ProductFeatures)
                {
                    item.IsRemoved = true;
                    item.RemovedTime = DateTime.Now;
                }
            }
            if (product.ProductImages != null)
            {
                foreach(var item in product.ProductImages)
                {
                    item.IsRemoved = true;
                    item.RemovedTime = DateTime.Now;
                }
            }
            product.IsRemoved = true;
            product.RemovedTime = DateTime.Now;
            _context.SaveChanges();
            return new ResultDto
            {
                IsSuccess = true,
                Message = "محصول با موفقیت حذف شد."
            };
        }
    }
}
