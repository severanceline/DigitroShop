using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Common.Queries.GetMenuItem;

namespace EndPoint.Site.ViewComponents
{
    public class GetMenuForMobile: ViewComponent
    {
        private readonly IGetMenuItemService _getMenuItemService;
        public GetMenuForMobile(IGetMenuItemService getMenuItemService)
        {
            _getMenuItemService = getMenuItemService;
        }
        public IViewComponentResult Invoke()
        {
            var menuItem = _getMenuItemService.Execute();
            return View(viewName: "GetMenuForMobile", menuItem.Data);
        }
    }
}
