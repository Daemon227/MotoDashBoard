using AutoMapper;
using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Helpers;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http.Headers;
using System.Text;

namespace DashBoard_MotoManager.Controllers
{
    public class ColorController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ColorController> _logger;
        private readonly IMapper _mapper;

        public ColorController(ILogger<ColorController> logger, HttpClient httpClient, IMapper mapper)
        {
            _logger = logger;
            _httpClient = httpClient;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> UpdateColor(string? colorID)
        {
            if (colorID != null)
            {
                var response = await _httpClient.GetAsync("https://localhost:7252/api/Color/Colors/" + colorID);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var model = JsonConvert.DeserializeObject<VersionColorVM>(data);
                return View(model);
            }
            else return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateColor(VersionColorVM model)
        {
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddColor(string versionID)
        {
            VersionColorVM color = new VersionColorVM
            {
                MaVersion = versionID,
            };
            return View(color);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddColor(VersionColorVM model)
        {
            if (ModelState.IsValid)
            {
                if (model.versionImageIF!=null)
                {
                    var formDataContent = new MultipartFormDataContent();
                    foreach(var item in model.versionImageIF)
                    {
                        var streamContent = new StreamContent(item.OpenReadStream());
                        streamContent.Headers.ContentType = new MediaTypeHeaderValue(item.ContentType);
                        formDataContent.Add(streamContent, "files", item.FileName);
                    }
                    var response = await _httpClient.PostAsync("https://localhost:7252/api/Upload/VersionImages", formDataContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var fileNames = JsonConvert.DeserializeObject<List<string>>(responseData);
                        var color = new VersionColorVM
                        {
                            MaVersionColor = MyTool.GenarateRandomKey(),
                            TenMau = model.TenMau,
                            MaVersion = model.MaVersion,
                            VersionImageVM = new List<VersionImageVM>(),
                        };
                        foreach (var fileName in fileNames)
                        {
                            var image = new VersionImageVM();
                            image.MaVersionColor = color.MaVersionColor;            
                            image.ImageUrl = (string)fileName;

                            color.VersionImageVM.Add(image);
                        }
                        // Lưu model vào cơ sở dữ liệu hoặc xử lý thêm }
                        var content = new StringContent(JsonConvert.SerializeObject(color), Encoding.UTF8, "application/json");
                        var response1 = await _httpClient.PostAsync("https://localhost:7252/api/Color/Colors", content);
                        if (response1.IsSuccessStatusCode)
                        {
                            _logger.LogInformation("Luu Color Thanh Cong");
                            return RedirectToAction("SeeDetail", "Version", new { versionID = model.MaVersion });
                        }
                        else if (response1.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            var errorMessage = await response1.Content.ReadAsStringAsync();
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
                    else
                    {
                        _logger.LogError("Error creating version");
                        ModelState.AddModelError(string.Empty, "Error creating version");

                        return View(model);
                    }
                }
                else { return View(model); }
                
            }
            else
            {
                return View(model);
            }
        }

        [Authorize]
        public async Task<IActionResult> RemoveColor(string colorID, string versionID)
        {
            if (colorID != null)
            {
                _logger.LogError("color ID: "+ colorID +" versionID" +versionID);
                var response = await _httpClient.DeleteAsync("https://localhost:7252/api/Color/Colors/" + colorID);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("SeeDetail", "Version", new { versionID = versionID });
                }
                else
                {
                    _logger.LogError("Response is false");
                    return View();
                }
            }

            else
            {
                _logger.LogError("Ma Brand ID Null");
                return View();
            }

        }

        
    }
}
