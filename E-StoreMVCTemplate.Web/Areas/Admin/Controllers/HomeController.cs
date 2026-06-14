using E_StoreMVCTemplate.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreMVCTemplate.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public HomeController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _dashboardService.GetAdminDashboardAsync();
            return View(model);
        }
    }
}
