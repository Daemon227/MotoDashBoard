
using Azure;
using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Helpers;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using X.PagedList.Extensions;
using AutoMapper;
using System.Drawing.Drawing2D;

namespace DashBoard_MotoManager.Controllers
{
    public class BrandController : Controller
    {
       
        private readonly HttpClient _httpClient;
        private readonly ILogger<BrandController> _logger;
        private readonly IMapper _mapper;
        public BrandController(ILogger<BrandController> logger, HttpClient httpClient, IMapper mapper)
        {
            _logger = logger;
            _httpClient = httpClient;
            _mapper = mapper;
        }


        public async Task<IActionResult> ListBrand(int? page)
        {
            int pageSize = 6;  // Số lượng mục mỗi trang
            int pageNumber = (page ?? 1); // Nếu page là null, gán giá trị mặc định là 1

            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7252/api/Brand/Brands");
                response.EnsureSuccessStatusCode(); 
                var data = await response.Content.ReadAsStringAsync();
                var brands = JsonConvert.DeserializeObject<List<BrandVM>>(data);
                var pageResult = brands.ToPagedList(pageNumber, pageSize);
                return View(pageResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching brands from API");
                return StatusCode(500, "Internal server error");
            }
           
        }

        public async Task<IActionResult> SeeDetail(string? brandID)
        {
            if (brandID != null)
            {
                var response = await _httpClient.GetAsync("https://localhost:7252/api/Brand/Brands/"+brandID);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<BrandVM>(data);
                return View(model);
            }
            else return NotFound();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Addbrand()
        {
            BrandVM brand = new BrandVM();
            return View(brand); 
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddBrand(BrandVM model)
        {
            if (ModelState.IsValid)
            {
                var brand = new Brand
                {
                    MaHangSanXuat = MyTool.GenarateRandomKey(),
                    TenHangSanXuat = model.TenHangSanXuat,
                    QuocGia = model.QuocGia,
                    MoTaNgan = model.MoTaNgan,
                };               
                var content = new StringContent(JsonConvert.SerializeObject(brand), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7252/api/Brand/Brands", content);
                if (response.IsSuccessStatusCode) 
                { 
                    _logger.LogInformation("Luu Brand Thanh Cong"); 
                    return RedirectToAction("ListBrand", "Brand"); 
                } 
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest) 
                {
                    
                    var errorMessage = await response.Content.ReadAsStringAsync(); 
                    ModelState.AddModelError(string.Empty, errorMessage); 
                    return View(model); 
                } 
                else 
                { 
                    _logger.LogError("Error creating brand");
                    ModelState.AddModelError(string.Empty, "Error creating brand"); 
                    return View(model); 
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> RemoveBrand(string brandId)
        {        
            if (brandId != null)
            {
                var response = await _httpClient.DeleteAsync("https://localhost:7252/api/Brand/Brands/" + brandId);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ListBrand");
                }
                else return View();
            }
            else
            {
                _logger.LogError("Ma Brand ID Null");
                return View();
            }

        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateBrand(string brandId)
        {
            var response = await _httpClient.GetAsync("https://localhost:7252/api/Brand/Brands/" + brandId);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<BrandVM>(data);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateBrand(BrandVM model, string brandId)
        {
            if (ModelState.IsValid)
            {
                var brand = _mapper.Map<BrandVM>(model);
                brand.MaHangSanXuat = brandId;
                var content = new StringContent(JsonConvert.SerializeObject(brand), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("https://localhost:7252/api/Brand/Brands/" + brandId, content);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Luu Brand Thanh Cong");
                    return RedirectToAction("ListBrand", "Brand");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return View(model);
                }
                else
                {
                    _logger.LogError("Error updating brand");
                    ModelState.AddModelError(string.Empty, "Error updating brand");
                    return View(model);
                }
            }
            else return View(model);
        }
    }
}
