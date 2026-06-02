using Microsoft.Build.Execution;
using Microsoft.EntityFrameworkCore;
using Store.Application.Interfaces.Contexts;
using Store.Common.Dto;
using Store.Domain.Entities.Carts;
using Store.Domain.Entities.Products;
using Store.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Services.Carts
{
    public interface ICartService
    {
        ResultDto AddToCart(long ProductId, Guid BrowserId, long? UserId);
        ResultDto RemoveFromCart(long ProductId, Guid BrowserId, long? UserId);
        ResultDto<CartDto> GetMyCart(Guid BrowserId, long? UserId);

        ResultDto Add(long CartItemId);
        ResultDto LowOff(long CartItemId);
    }

    public class CartService : ICartService
    {
        private readonly IDataBaseContext _context;
        public CartService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Add(long CartItemId)
        {
            var cartItem = _context.CartItems.Find(CartItemId);
            cartItem.Count++;
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
            };
        }

        public ResultDto AddToCart(long ProductId, Guid BrowserId, long? UserId)
        {
            var query = _context.Carts.AsQueryable();
            //به این دلیل اینجا چک میکنم چون ممکنه که کاربر بدون لاگین
            //تو سایت باشه و بعد لاگین کنه و سبد خریدش دارای اون یروزرآیدی و یوزر آیدی یکسانه 
            //اگه با همون مرورگر لاگاوت کرد  دوباره به سبد خریدش اضافه کرد 
            //به سبد خرید واقعیش ارسال میشه با این که لاگین نیست
            //ولی چون لاگوت کرده و کوکی مربوط به برور آیدیش پاک شده با بروزرآیدی میره یه سبد خرید دیگه میسازه
            if (UserId != null)
            {
                query = query.Where(p => p.UserId == UserId && p.Finished == false);
            }
            else
            {
                query = query.Where(p => p.BrowserId == BrowserId && p.Finished == false);
            }

            var cart = query.OrderByDescending(p => p.Id).FirstOrDefault();

            if (cart == null)
            {
                Cart newCart = new Cart()
                {
                    Finished = false
                };

                if (UserId != null) { newCart.UserId = UserId; }
                else { newCart.BrowserId = BrowserId; }

                _context.Carts.Add(newCart);
                _context.SaveChanges();
                cart = newCart;
            }

            var product = _context.Products.Find(ProductId);

            var cartItem = _context.CartItems.Where(p => p.ProductId == ProductId && p.CartId == cart.Id).FirstOrDefault();
            if (cartItem != null)
            {
                cartItem.Count++;
            }
            else
            {
                CartItem newCartItem = new CartItem()
                {
                    Cart = cart,
                    Count = 1,
                    Price = product.Price,
                    Product = product,

                };
                _context.CartItems.Add(newCartItem);
                
            }
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
                Message = $"محصول  {product.Name} با موفقیت به سبد خرید شما اضافه شد ",
            };
        }

        public ResultDto<CartDto> GetMyCart(Guid BrowserId, long? UserId)
        {
            try
            {
                var query = _context.Carts
                    .Include(p => p.CartItems)
                        .ThenInclude(p => p.Product)
                            .ThenInclude(p => p.ProductImages)
                    .AsQueryable();


                if (UserId != null)
                {
                    var userCart = _context.Carts
                        .Include(c => c.CartItems)
                        .FirstOrDefault(c => c.UserId == UserId && c.Finished == false);

                    var browserCart = query.FirstOrDefault(p => p.BrowserId == BrowserId && p.Finished == false);
                    if (browserCart != null)
                    {
                        if (userCart == null)
                        {
                            browserCart.UserId = UserId;
                        }
                        else
                        {
                            foreach (var browserItem in browserCart.CartItems.ToList())
                            {
                                var existingItemInUserCart = userCart.CartItems
                                    .FirstOrDefault(p => p.ProductId == browserItem.ProductId);

                                if (existingItemInUserCart != null)
                                {
                                    // اگر بود، تعدادش را اضافه می‌کنیم
                                    existingItemInUserCart.Count += browserItem.Count;
                                    browserItem.IsRemoved = true;
                                    browserItem.RemovedTime = DateTime.Now;
                                    //_context.CartItems.Remove(browserItem);
                                }
                                else
                                {
                                    browserItem.CartId = userCart.Id;
                                }
                            }

                            browserCart.IsRemoved = true;
                            browserCart.RemovedTime = DateTime.Now;
                            //_context.Carts.Remove(browserCart);
                        }
                        _context.SaveChanges();
                    }
                    query = query.Where(p => p.UserId == UserId && p.Finished == false);
                }
                else
                {
                    query = query.Where(p => p.BrowserId == BrowserId && p.Finished == false);
                }

                var cart = query.OrderByDescending(p => p.Id).FirstOrDefault();

                if (cart == null)
                {
                    return new ResultDto<CartDto>()
                    {
                        Data = new CartDto()
                        {
                            CartItems = new List<CartItemDto>()
                        },
                        IsSuccess = false,
                    };
                }

                return new ResultDto<CartDto>()
                {
                    Data = new CartDto()
                    {
                        ProductCount = cart.CartItems.Count(),
                        SumAmount = cart.CartItems.Sum(p => p.Price * p.Count),
                        CartID = cart.Id,
                        CartItems = cart.CartItems.Select(p => new CartItemDto
                        {
                            Count = p.Count,
                            Price = p.Price,
                            ProductId = p.Product.Id,
                            ProductName = p.Product.Name,
                            Id = p.Id,
                            SumPrice = p.Count * p.Price,
                            Images = p.Product?.ProductImages?.FirstOrDefault()?.Src ?? "",        
                        }).ToList(),
                    },
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public ResultDto LowOff(long CartItemId)
        {
            var cartItem = _context.CartItems.Find(CartItemId);

            if (cartItem.Count <= 1)
            {
                return new ResultDto()
                {
                    IsSuccess = false,
                };
            }
            cartItem.Count--;
            _context.SaveChanges();
            return new ResultDto()
            {
                IsSuccess = true,
            };
        }

        public ResultDto RemoveFromCart(long ProductId, Guid BrowserId, long? UserId)
        {
            var query = _context.CartItems.Include(p => p.Cart).AsQueryable();

            if(UserId != null)
            {
                query = query.Where(p => p.Cart.UserId == UserId && p.ProductId == ProductId ).AsQueryable();
            }
            else
            {
                query = query.Where(p => p.Cart.BrowserId == BrowserId && p.ProductId == ProductId).AsQueryable();
            }
                
            var cartitem = query.FirstOrDefault();
            if (cartitem != null)
            {
                cartitem.IsRemoved = true;
                cartitem.RemovedTime = DateTime.Now;
                _context.SaveChanges();
                return new ResultDto
                {
                    IsSuccess = true,
                    Message = "محصول از سبد خرید شما حذف شد"
                };

            }
            else
            {
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "محصول یافت نشد"
                };
            }
        }

    }

    public class CartDto
    {
        public int ProductCount { get; set; }
        public int SumAmount { get; set; }
        public List<CartItemDto> CartItems { get; set; }
        public long CartID { get; set; }
    }
    public class CartItemDto
    {
        public long Id { get; set; }
        public string ProductName { get; set; }
        public long ProductId { get; set; }
        public string Images { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public int SumPrice { get; set; }
    }
}