using EndPoint.Site.Utilities;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Finances.Queries.GetRequestPayForUser;
using Store.Application.Services.Orders.Queries.GetOrder;
using Store.Domain.Entities.Orders;

namespace EndPoint.Site.Controllers
{
    public class OrderController : Controller
    {
        IGetOrderUserService _getOrderUserService;
        IGetRequestPayForUser _getRequestPayForUser;
        public OrderController(IGetOrderUserService getOrderUserService
            , IGetRequestPayForUser getRequestPayForUser)
        {
            _getOrderUserService = getOrderUserService;
            _getRequestPayForUser = getRequestPayForUser;
        }
        public IActionResult Index()
        {
            long UserId = ClaimUtility.GetUserId(User).Value;
            
            return View(_getOrderUserService.Execute(UserId).Data);
        }

        public IActionResult GetRequestPay(long requestPayId)
        {
            return View(_getRequestPayForUser.Execute(requestPayId).Data);
        }
    }
}
