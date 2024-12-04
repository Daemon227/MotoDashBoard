using AutoMapper;
using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Helpers;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using X.PagedList.Extensions;

namespace DashBoard_MotoManager.Controllers
{
    public class MotoTypeController : Controller
    {
      
        private readonly HttpClient _httpClient;
        private readonly ILogger<MotoController> _logger;
        private readonly IMapper _mapper;
        public MotoTypeController(HttpClient httpClient,ILogger<MotoController> logger, IMapper mapper)
        {    
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> ListType(int? page)
        {
            int pageSize = 6;  // Số lượng mục mỗi trang
            int pageNumber = (page ?? 1); // Nếu page là null, gán giá trị mặc định là 1

            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7252/api/Type/Types");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var types = JsonConvert.DeserializeObject<List<MotoTypeVM>>(data);
                var pageResult = types.ToPagedList(pageNumber, pageSize);
                return View(pageResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching types from API");
                return StatusCode(500, "Internal server error");
            }

        }

        public async Task<IActionResult> SeeDetail(string? typeID)
        {
            if (typeID != null)
            {
                var response = await _httpClient.GetAsync("https://localhost:7252/api/Type/Types/" + typeID);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<MotoTypeVM>(data);
                return View(model);
            }
            else return NotFound();
        }

        //[Authorize]
        [HttpGet]
        public IActionResult AddType()
        {
            MotoTypeVM model = new MotoTypeVM();
            return View(model);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> AddType(MotoTypeVM model)
        {
            if (ModelState.IsValid)
            {
                var type = new MotoType
                {
                   MaLoai = MyTool.GenarateRandomKey(),
                   TenLoai = model.TenLoai,
                   DoiTuongSuDung = model.DoiTuongSuDung,
                   MoTaNgan = model.MoTaNgan,
                };
                var content = new StringContent(JsonConvert.SerializeObject(type), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7252/api/Type/Types", content);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Luu loai Thanh Cong");
                    return RedirectToAction("ListType", "MotoType");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return View(model);
                }
                else
                {
                    _logger.LogError("Error creating type");
                    ModelState.AddModelError(string.Empty, "Error creating type");
                    return View(model);
                }
            }
            return View(model);
        }

        //[Authorize]
        public async Task<IActionResult> RemoveType(string typeId)
        {
            if (typeId != null)
            {
                var response = await _httpClient.DeleteAsync("https://localhost:7252/api/Type/Types/" + typeId);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ListType");
                }
                else return View();
            }
            else
            {
                _logger.LogError("Ma Type ID Null");
                return View();
            }

        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateType(string typeId)
        {
            var response = await _httpClient.GetAsync("https://localhost:7252/api/Type/Types/" + typeId);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<MotoTypeVM>(data);
            return View(model);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateType(MotoTypeVM model, string typeId)
        {
            if (ModelState.IsValid)
            {
                var type = _mapper.Map<MotoTypeVM>(model);
                type.MaLoai = typeId;
                var content = new StringContent(JsonConvert.SerializeObject(type), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("https://localhost:7252/api/Type/Types/" + typeId, content);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Luu Type Thanh Cong");
                    return RedirectToAction("ListType", "MotoType");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return View(model);
                }
                else
                {
                    _logger.LogError("Error updating type");
                    ModelState.AddModelError(string.Empty, "Error updating type");
                    return View(model);
                }
            }
            else return View(model);
        }
    }
}
