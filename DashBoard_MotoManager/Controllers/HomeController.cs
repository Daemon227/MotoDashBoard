using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DashBoard_MotoManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MotoWebsiteContext _db;
        public HomeController(ILogger<HomeController> logger, MotoWebsiteContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var brands = _db.Brands.ToList();
            var types = _db.MotoTypes.ToList();
            var model = new ChartVM();
            foreach (var brand in brands)
            {
                var motos = _db.MotoBikes.Where(m => m.MaHangSanXuat == brand.MaHangSanXuat).ToList();
                var brandCountVM = new BrandCountVM
                {
                    brandName = brand.TenHangSanXuat,
                    count = motos.Count,
                };
                model.brandChart.Add(brandCountVM);
            }
            foreach (var type in types)
            {
                var motos = _db.MotoBikes.Where(m => m.MaLoai == type.MaLoai).ToList();
                var typeCountVM = new TypeCountVM
                {
                    typeName = type.TenLoai,
                    count = motos.Count,
                };
                model.typeChart.Add(typeCountVM);
            }
            model.motoCount = _db.MotoBikes.Count();
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}
