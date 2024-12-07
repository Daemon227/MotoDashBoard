using AutoMapper;
using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Helpers;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text;
using X.PagedList.Extensions;

namespace DashBoard_MotoManager.Controllers
{
    public class VersionController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VersionController> _logger;
        private readonly IMapper _mapper;
        public VersionController(ILogger<VersionController> logger, HttpClient httpClient, IMapper mapper)
        {
            _logger = logger;
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<IActionResult> ListVersion(string motoID, int? page)
        {
            int pageSize = 6;  // Số lượng mục mỗi trang
            int pageNumber = (page ?? 1); // Nếu page là null, gán giá trị mặc định là 1

            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7252/api/Version/"+motoID+"/Versions");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var versions = JsonConvert.DeserializeObject<List<MotoVersionVM>>(data);
                var pageResult = versions.ToPagedList(pageNumber, pageSize);
                return View(pageResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching version from API");
                return StatusCode(500, "Internal server error");
            }

        }

        public async Task<IActionResult> SeeDetail(string? versionID)
        {
            if (versionID != null)
            {
                var response = await _httpClient.GetAsync("https://localhost:7252/api/Version/Versions/" + versionID);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<MotoVersionVM>(data);
                return View(model);
            }
            else return NotFound();
        }


        //[Authorize]
        [HttpGet]
        public IActionResult AddVersion(string motoID)
        {
            MotoVersionVM version = new MotoVersionVM
            {
                MaXe = motoID,
            };
            return View(version);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> AddVersion(MotoVersionVM model)
        {
            if (ModelState.IsValid)
            {
                var version = new MotoVersionVM
                {
                    MaVersion = MyTool.GenarateRandomKey(),
                    MaXe = model.MaXe,
                    TenVersion = model.TenVersion,
                    GiaBanVersion = model.GiaBanVersion,
                };
                _logger.LogError("Mã Xe" + version.MaXe);
                var content = new StringContent(JsonConvert.SerializeObject(version), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("https://localhost:7252/api/Version/Versions", content);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Luu Version Thanh Cong");
                    return RedirectToAction("ListVersion", "Version", new {motoID = version.MaXe});
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return View(model);
                }
                else
                {
                    _logger.LogError("Error creating version");
                    ModelState.AddModelError(string.Empty, "Error creating version");

                    return View(model);
                }
            }

            return View(model);
        }

        //[Authorize]
        public async Task<IActionResult> RemoveVersion(string versionId, string motoID)
        {
            if (versionId != null)
            {
                var response = await _httpClient.DeleteAsync("https://localhost:7252/api/Version/Versions/" + versionId);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ListVersion", "Version", new { motoID = motoID });
                    //return View();
                }
                else
                {
                    _logger.LogError("Xoa không nổi");
                    return View();
                } 
            }
            else
            {
                _logger.LogError("Version ID is Null");
                return View();
            }

        }

        //[Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateVersion(string versionID)
        {
            var response = await _httpClient.GetAsync("https://localhost:7252/api/Version/Versions/" + versionID);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var model = JsonConvert.DeserializeObject<MotoVersionVM>(data);
            return View(model);
        }

        //[Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateVersion(MotoVersionVM model, string versionID)
        {
            if (ModelState.IsValid)
            {
                //lay lai version goc
                var response1 = await _httpClient.GetAsync("https://localhost:7252/api/Version/Versions/" + versionID);
                response1.EnsureSuccessStatusCode();
                var data = await response1.Content.ReadAsStringAsync();
                var version = JsonConvert.DeserializeObject<MotoVersionVM>(data);

                // set lai gia tri
                version.TenVersion = model.TenVersion;
                version.GiaBanVersion = model.GiaBanVersion;
                var content = new StringContent(JsonConvert.SerializeObject(version), Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync("https://localhost:7252/api/Version/Versions/" + versionID, content);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Luu Version Thanh Cong");
                    return RedirectToAction("ListVersion", "Version", new { motoID = version.MaXe });
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return View(model);
                }
                else
                {
                    _logger.LogError("Error updating version");
                    ModelState.AddModelError(string.Empty, "Error updating version");
                    return View(model);
                }
            }
            else return View(model);
        }
    }
}

