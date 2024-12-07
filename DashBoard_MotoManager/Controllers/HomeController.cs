using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DashBoard_MotoManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        public HomeController(ILogger<HomeController> logger,HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("https://localhost:7252/api/Brand/Brands");
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var brands = JsonConvert.DeserializeObject<List<BrandVM>>(data);
            _logger.LogError("S? brands = "+brands.Count());

            var response1 = await _httpClient.GetAsync("https://localhost:7252/api/Type/Types");
            response1.EnsureSuccessStatusCode();
            var data1 = await response1.Content.ReadAsStringAsync();
            var types = JsonConvert.DeserializeObject<List<MotoTypeVM>>(data1);
            _logger.LogError("S? types = " + types.Count());


            var response2 = await _httpClient.GetAsync("https://localhost:7252/api/Moto/Motos");
            response2.EnsureSuccessStatusCode();
            var data2 = await response2.Content.ReadAsStringAsync();
            var motos = JsonConvert.DeserializeObject<List<MotoVM>>(data2);
            _logger.LogError("S? môtss = " + motos.Count());

            var model = new ChartVM();
            foreach (var brand in brands)
            {   
                var brandCountVM = new BrandCountVM
                {
                    brandName = brand.TenHangSanXuat,
                    count = motos.Where(m => m.MaHangSanXuat == brand.MaHangSanXuat).ToList().Count()
                };
                model.brandChart.Add(brandCountVM);
            }
            foreach (var type in types)
            {
                var typeCountVM = new TypeCountVM
                {
                    typeName = type.TenLoai,
                    count = motos.Where(m => m.MaLoai == type.MaLoai).ToList().Count()
                };
                model.typeChart.Add(typeCountVM);
            }


            model.motoCount = motos.Count();
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
