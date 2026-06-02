using Azure;
using EndPoint.Site.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using Store.Application.Services.Carts;
using Store.Application.Services.Finances.Commands.AddRequestPay;
using Store.Application.Services.Finances.Queries.GetRequestPay;
using Store.Application.Services.Finances.Queries.UpdateRequstPay;
using Store.Application.Services.Orders.Commands.AddOrder;
using System.Transactions;
using Zarinpal.AspNetCore.DTOs;
using Zarinpal.AspNetCore.Extensions;
using Zarinpal.AspNetCore.Interfaces;

namespace EndPoint.Site.Controllers
{
    [Authorize("Customer")]
    public class PayController : Controller
    {
        private readonly IAddRequestPayService _addPayService;
        private readonly CookiesManeger cookiesManeger;
        private readonly ICartService _cartService;
        private readonly IZarinpalService _zarinpalService;
        private readonly IGetRequestPayService _getRequestPayService;
        private readonly IAddOrderService _addOrderService;
        IUpdateRequestPayService _updateRequestPayService;

        public PayController(IAddRequestPayService addPayService,
            ICartService cartService,
            IZarinpalService zarinpalService,
            IGetRequestPayService getRequestPayService,
            IAddOrderService addOrderService,
            IUpdateRequestPayService updateRequestPayService)
        {
            _zarinpalService = zarinpalService;
            _addPayService = addPayService;
            _cartService = cartService;
            cookiesManeger = new CookiesManeger();
            _getRequestPayService = getRequestPayService;
            _addOrderService = addOrderService;
            _updateRequestPayService = updateRequestPayService;
        }
        public async Task<IActionResult> Index()
        {
            long? userId = ClaimUtility.GetUserId(User);
            var cart = _cartService.GetMyCart(cookiesManeger.GetBrowserId(HttpContext), userId);
            if(cart.Data.SumAmount >= 10000)
            {
                var requestPay = _addPayService.Execute(cart.Data.SumAmount, userId.Value,cart.Data.CartID);
                int amount = cart.Data.SumAmount;
                var request = new ZarinpalRequestDTO(
                    amount,
                    "پرداخت فاکتور خرید از فروشگاه تستی",
                    "https://localhost:44354/Pay/VerifyPayment",
                    "severanceline@gmail.com" ,
                    "09123456789");

                //var result = await _zarinpalService.RequestAsync(request);
                //if (result == null || result.Data == null || !result.IsSuccessStatusCode || string.IsNullOrEmpty(result.Data.Authority))
                //{
                    string fakeAuthority = "A" + Guid.NewGuid().ToString("N").ToUpper() + "123";
                    return Redirect($"/Pay/VerifyPayment?Authority={fakeAuthority}&Status=OK&paymentGuid={requestPay.Data.guid}&isSimulation=true");
                //}
                //return Redirect(result.RedirectUrl);
            }
            else
            {
                return RedirectToAction("Index","Cart");
            }
        }

        public async Task<IActionResult> VerifyPayment(string Authority, string Status, Guid paymentGuid, bool isSimulation = false)
        {
            if (Status != "OK")
            {
                ViewBag.Message = "پرداخت توسط کاربر لغو شد یا ناموفق بود.";
                return View("Failed");
            }

            if (isSimulation)
            {
                string fakeRefId = new Random().Next(10000000, 99999999).ToString();
                var requestpaySimulated = _getRequestPayService.Execute(paymentGuid);
                var updatedRequestPay = _updateRequestPayService.Execute(new RequestUpdateRequestPayDto()
                {
                    Guid = paymentGuid,
                    Authority = Authority,
                    RefId = fakeRefId,
                });
                var orderSimulated = _addOrderService.Execute(requestpaySimulated.Data.Id);
                // ViewBag.RefId = fakeRefId;
                return RedirectToAction("Index", "Order");

            }
            //  از اینجا به بعد فقط برای پرداخت‌های واقعی است 
            int amount = _getRequestPayService.Execute(paymentGuid).Data.Amount;
            var verify = new ZarinpalVerifyDTO(amount, HttpContext.GetZarinpalAuthorityQuery()!);
            var result = await _zarinpalService.VerifyAsync(verify);
            
            if (result != null && result.IsSuccessStatusCode)
            {
                var requestpay = _getRequestPayService.Execute(paymentGuid);
                _addOrderService.Execute(requestpay.Data.Id);
                // ViewBag.RefId = result.Data.RefId;
                return View("Success");
            }

            ViewBag.Message = "خطا در تایید تراکنش.";
            return View("Failed");
        }
    } 
}
