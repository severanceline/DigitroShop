using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Application.Services.Finances.Queries.GetRequestPayForAdmin;
using Store.Application.Services.Orders.Commands.ChangeOrderState;
using Store.Application.Services.Orders.Queries.GetOrderForAdmin;
using Store.Domain.Entities.Orders;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Operator")]
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IGetOrderForAdminService _getOrderForAdminService;
        private readonly IChangeOrderState _changeOrderState;
        private readonly IGetRequestPayForAdminService _getRequestPayForAdminService;

        public OrderController(IGetOrderForAdminService getOrderForAdminService,
            IChangeOrderState changeOrderState,
            IGetRequestPayForAdminService getRequestPayForAdminService)
        {
            _getOrderForAdminService = getOrderForAdminService;
            _changeOrderState = changeOrderState;
            _getRequestPayForAdminService = getRequestPayForAdminService;
        }
        public IActionResult Index(State orderState = State.Processing, int Page = 1, int PageSize = 20)
        {
            return View(_getOrderForAdminService.Execute(orderState,Page,PageSize).Data);
        }
        public IActionResult ChangeState(State orderStateage , long OrderId)
        {
            _changeOrderState.Execute(OrderId, orderStateage);
            return RedirectToAction("Index");
        }
        public IActionResult GetRequestPay(long requestPayId)
        {
            return View(_getRequestPayForAdminService.Execute(requestPayId).Data);
        }
    }
}
