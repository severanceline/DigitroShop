using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Finances.Queries.GetAllRequestPayForAdmin;

namespace EndPoint.Site.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Operator")]
    [Area("Admin")]
    public class PayController : Controller
    {
        private readonly IGetAllRequestPayForAdminSevice _getAllRequestPayForAdminSevice;
        public PayController(IGetAllRequestPayForAdminSevice getAllRequestPayForAdminSevice)
        {
            _getAllRequestPayForAdminSevice = getAllRequestPayForAdminSevice;
        }
        public IActionResult Index(int page = 1, int pageSize = 20)
        {
            return View(_getAllRequestPayForAdminSevice.Execute(page,pageSize).Data);
        }
    }
}