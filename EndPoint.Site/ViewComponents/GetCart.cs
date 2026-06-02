using EndPoint.Site.Utilities;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Carts;

namespace EndPoint.Site.ViewComponents
{
    public class GetCart : ViewComponent
    {
        private readonly ICartService _cartService;
        private readonly CookiesManeger cookiesManeger;
        public GetCart(ICartService cartService)
        {
            _cartService = cartService;
            cookiesManeger = new CookiesManeger();
        }

        public IViewComponentResult Invoke()
        {
            var userId = ClaimUtility.GetUserId(HttpContext.User);

            var resultGetLst = _cartService.GetMyCart(cookiesManeger.GetBrowserId(HttpContext),userId);
            return View(viewName: "GetCart", resultGetLst.Data);
        }
    }
}
