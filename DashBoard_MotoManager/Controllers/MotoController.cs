using AutoMapper;
using Azure;
using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Helpers;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json;
using System.Diagnostics.Contracts;
using System.Drawing.Drawing2D;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Web;
using X.PagedList.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DashBoard_MotoManager.Controllers
{
    public class MotoController : Controller
    {
        private readonly MotoWebsiteContext _db;
        private readonly HttpClient _httpClient;
        private readonly ILogger<MotoController> _logger;
        private readonly IMapper _mapper;
        public MotoController(MotoWebsiteContext context,HttpClient httpClient, ILogger<MotoController> logger, IMapper mapper)
        {
            _db = context;
            _httpClient = httpClient;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> ListMoto(int? page, string? maLoai)
        {
            int pageSize = 6;  // Số lượng mục mỗi trang
            int pageNumber = (page ?? 1); // Nếu page là null, gán giá trị mặc định là 1

            try
            {
                var response = await _httpClient.GetAsync("https://localhost:7252/api/Moto/Motos");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var motos = JsonConvert.DeserializeObject<List<MotoVM>>(data);
                var pageResult = motos.ToPagedList(pageNumber, pageSize);
                return View(pageResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching brands from API");
                return StatusCode(500, "Internal server error");
            }
        }

        public async Task<IActionResult> SeeDetail(string? maXe)
        {
            if (maXe != null)
            {
                //lay ra xe
                var response = await _httpClient.GetAsync("https://localhost:7252/api/Moto/Motos/" + maXe);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();    
                var moto = JsonConvert.DeserializeObject<MotoVM>(data);
                //lay hinh anh
                
                var response1 = await _httpClient.GetAsync("https://localhost:7252/api/Library/Libraries/" + moto.MaLibrary);
                response1.EnsureSuccessStatusCode();
                var librarydata = await response1.Content.ReadAsStringAsync();
				//_logger.LogError($"Library response data: {librarydata}");
				var library = JsonConvert.DeserializeObject<LibraryVM>(librarydata);   
                moto.MaLibraryNavigation = library;
                return View(moto);
            }
            else return NotFound();         
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> AddMoto()
        {
            // lay brand
			var response = await _httpClient.GetAsync("https://localhost:7252/api/Brand/Brands");
			response.EnsureSuccessStatusCode();
			var data = await response.Content.ReadAsStringAsync();
			var brands = JsonConvert.DeserializeObject<List<BrandVM>>(data);

			//lay Type
			var response1 = await _httpClient.GetAsync("https://localhost:7252/api/Type/Types");
			response1.EnsureSuccessStatusCode();
			var data1 = await response1.Content.ReadAsStringAsync();
			var types = JsonConvert.DeserializeObject<List<MotoTypeVM>>(data1);

			var model = new AddMotoVM
            {
                Brands = brands,
                MotoTypes = types,
            };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddMoto(AddMotoVM model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    _logger.LogError("Model xe truyen ve bi null");
                    return View();
                }
                else
                {                 
                    var moto = _mapper.Map<MotoBike>(model.MotoBike);
                    moto.MaXe = MyTool.GenarateRandomKey();
                    moto.MaLibrary = MyTool.GenarateRandomKey();

					// xu ly anh mota:
					if (model.AnhMoTaIF != null)
					{
						// truyen file sang backend.
						var formDataContent = new MultipartFormDataContent();
						var streamContent = new StreamContent(model.AnhMoTaIF.OpenReadStream());
						streamContent.Headers.ContentType = new MediaTypeHeaderValue(model.AnhMoTaIF.ContentType);
						formDataContent.Add(streamContent, "files", model.AnhMoTaIF.FileName);
						var responseLibrary = await _httpClient.PostAsync("https://localhost:7252/api/Upload/MotoImages", formDataContent);
						// neu thanh cong thi lay file string
						if (responseLibrary.IsSuccessStatusCode)
						{
							var responseData = await responseLibrary.Content.ReadAsStringAsync();
							var fileNames = JsonConvert.DeserializeObject<List<string>>(responseData);

							foreach (var fileName in fileNames)
							{
								moto.AnhMoTaUrl = fileName;
							}
						}
						else
						{
                            moto.AnhMoTaUrl = "anh";
							_logger.LogError("Error creating Moto Image");
							ModelState.AddModelError(string.Empty, "Error creating Moto Image");
						}
					}
					#region LIBRARY
					//them library
					var motoLibray = new LibraryVM
                    {
                        MaLibrary = moto.MaLibrary,
                        LibraryImageVM = new List<LibraryImageVM>()
                    };
					// Them anh vao thu vien
					if (model.LibraryImageIF != null)
					{
						// truyen file sang backend.
						var formDataContent = new MultipartFormDataContent();
						foreach (var item in model.LibraryImageIF)
						{
							var streamContent = new StreamContent(item.OpenReadStream());
							streamContent.Headers.ContentType = new MediaTypeHeaderValue(item.ContentType);
							formDataContent.Add(streamContent, "files", item.FileName);
						}
						var responseLibrary = await _httpClient.PostAsync("https://localhost:7252/api/Upload/LibraryImages", formDataContent);
						// neu thanh cong thi lay file string
						if (responseLibrary.IsSuccessStatusCode)
						{
							var responseData = await responseLibrary.Content.ReadAsStringAsync();
							var fileNames = JsonConvert.DeserializeObject<List<string>>(responseData);

							foreach (var fileName in fileNames)
							{
								// set ten va them vao MotoLibrary
								var image = new LibraryImageVM();
								image.MaLibrary = motoLibray.MaLibrary;
                                image.ImageUrl = fileName;
								motoLibray.LibraryImageVM.Add(image);
							}
						}
						else
						{
							_logger.LogError("Error creating library Image");
							ModelState.AddModelError(string.Empty, "Error creating library Image");
						}
					}

					var libraryCreate = new StringContent(JsonConvert.SerializeObject(motoLibray), Encoding.UTF8, "application/json");
					var libraryCreateResult = await _httpClient.PostAsync("https://localhost:7252/api/Library/Libraries", libraryCreate);

                    // kiem tra trang thai tao library
                    if (libraryCreateResult.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Create Library Thanh Cong___"+motoLibray.MaLibrary);

                    }
                    else
                    {
						_logger.LogInformation("Loi Create Library Khong Thanh Cong");
						ModelState.AddModelError(string.Empty, "Error creating Library");
					}
					#endregion

					var content = new StringContent(JsonConvert.SerializeObject(moto), Encoding.UTF8, "application/json");
					var response = await _httpClient.PostAsync("https://localhost:7252/api/Moto/Motos", content);
					if (response.IsSuccessStatusCode)
					{
						_logger.LogInformation("Luu Xe Thanh Cong");
						return RedirectToAction("ListMoto", "Moto");
					}
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var errorMessage = await response.Content.ReadAsStringAsync();
                        ModelState.AddModelError(string.Empty, errorMessage);
                        return View(model);
                    }
                    else
					{
						_logger.LogError("Error creating Moto");
						var errorMessage = await response.Content.ReadAsStringAsync();
						ModelState.AddModelError(string.Empty, "Error creating Moto ___ "+errorMessage);

						var responseBrand = await _httpClient.GetAsync("https://localhost:7252/api/Brand/Brands");
						responseBrand.EnsureSuccessStatusCode();
						var data = await responseBrand.Content.ReadAsStringAsync();
						var brands = JsonConvert.DeserializeObject<List<BrandVM>>(data);

						//lay Type
						var response1 = await _httpClient.GetAsync("https://localhost:7252/api/Type/Types");
						response1.EnsureSuccessStatusCode();
						var data1 = await response1.Content.ReadAsStringAsync();
						var types = JsonConvert.DeserializeObject<List<MotoTypeVM>>(data1);

						model.Brands = brands;
						model.MotoTypes = types;
						return View(model);
					}
                }
            }
            else
            {
				// lay brand
				var response = await _httpClient.GetAsync("https://localhost:7252/api/Brand/Brands");
				response.EnsureSuccessStatusCode();
				var data = await response.Content.ReadAsStringAsync();
				var brands = JsonConvert.DeserializeObject<List<BrandVM>>(data);

				//lay Type
				var response1 = await _httpClient.GetAsync("https://localhost:7252/api/Type/Types");
				response1.EnsureSuccessStatusCode();
				var data1 = await response1.Content.ReadAsStringAsync();
				var types = JsonConvert.DeserializeObject<List<MotoTypeVM>>(data1);

                model.Brands = brands;
                model.MotoTypes = types;
                return View(model);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> RemoveMoto(string? maXe)
        {
            if (maXe == null)
            {
                return RedirectToAction("NotFound","Home");
            }
            else
            {
				var response = await _httpClient.GetAsync("https://localhost:7252/api/Moto/Motos/" + maXe);
				response.EnsureSuccessStatusCode();
				var data = await response.Content.ReadAsStringAsync();
				var moto = JsonConvert.DeserializeObject<MotoVM>(data);
				return View(moto);
			}
			//string id = "00";
	
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmRemoveMoto(string maXe)
        {
			if (maXe != null)
			{
				var response = await _httpClient.DeleteAsync("https://localhost:7252/api/Moto/Motos/" + maXe);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("ListMoto","Moto");
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
        public async Task<IActionResult> EditMoto(string maXe)
        {
			if (maXe != null)
			{
				//lay ra xe
				var responseMoto = await _httpClient.GetAsync("https://localhost:7252/api/Moto/Motos/" + maXe);
				responseMoto.EnsureSuccessStatusCode();
				var dataMoto = await responseMoto.Content.ReadAsStringAsync();
				var moto = JsonConvert.DeserializeObject<MotoVM>(dataMoto);

				//lay hinh anh
				var responseLibrary = await _httpClient.GetAsync("https://localhost:7252/api/Library/Libraries/" + moto.MaLibrary);
				responseLibrary.EnsureSuccessStatusCode();
				var librarydata = await responseLibrary.Content.ReadAsStringAsync();				
				var library = JsonConvert.DeserializeObject<LibraryVM>(librarydata);		
				moto.MaLibraryNavigation = library;

				// lay brand
				var response = await _httpClient.GetAsync("https://localhost:7252/api/Brand/Brands");
				response.EnsureSuccessStatusCode();
				var data = await response.Content.ReadAsStringAsync();
				var brands = JsonConvert.DeserializeObject<List<BrandVM>>(data);

				//lay Type
				var response1 = await _httpClient.GetAsync("https://localhost:7252/api/Type/Types");
				response1.EnsureSuccessStatusCode();
				var data1 = await response1.Content.ReadAsStringAsync();
				var types = JsonConvert.DeserializeObject<List<MotoTypeVM>>(data1);

				var model = new EditMotoVM
				{
                    MotoBike = moto,
					Brands = brands,
					MotoTypes = types,
				};
				return View(model);
			}
			else return NotFound();
			
			
		}

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditMoto(EditMotoVM model, string maXe)
        {
            if (model != null && maXe!= null)
            {
                var response = await _httpClient.GetAsync("https://localhost:7252/api/Moto/Motos/" + maXe);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var oldMoto = JsonConvert.DeserializeObject<MotoBike>(data);
				var newMoto = _mapper.Map<MotoBike>(model.MotoBike);
				newMoto.MaXe = oldMoto.MaXe;
                newMoto.MaLibrary = oldMoto.MaLibrary;

                if (model.AnhMoTaIF == null || model.AnhMoTaIF.Length == 0)
				{
					newMoto.AnhMoTaUrl = oldMoto.AnhMoTaUrl;
				}
                else
                {  
                    // truyen file sang backend.
                    var formDataContent = new MultipartFormDataContent();
                    var streamContent = new StreamContent(model.AnhMoTaIF.OpenReadStream());
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(model.AnhMoTaIF.ContentType);
                    formDataContent.Add(streamContent, "files", model.AnhMoTaIF.FileName);
                    var responseLibrary = await _httpClient.PostAsync("https://localhost:7252/api/Upload/MotoImages", formDataContent);
                    // neu thanh cong thi lay file string
                    if (responseLibrary.IsSuccessStatusCode)
                    {
                        var responseData = await responseLibrary.Content.ReadAsStringAsync();
                        var fileNames = JsonConvert.DeserializeObject<List<string>>(responseData);

                        foreach (var fileName in fileNames)
                        {
                            newMoto.AnhMoTaUrl = fileName;
                        }
                    }
                }
				if (model.LibraryImageIF != null && model.LibraryImageIF.Count()>0)
				{
                    _logger.LogError("ma library: " + newMoto.MaLibrary);
                    var responseLibraryDelete = await _httpClient.DeleteAsync("https://localhost:7252/api/Library/ResetLibraries/" + oldMoto.MaLibrary);
					if (responseLibraryDelete.IsSuccessStatusCode)
					{
                        #region LIBRARY 
                        // Them anh vao thu vien
                        var motoLibrary = new LibraryVM
                        {
                            MaLibrary = newMoto.MaLibrary,
                            LibraryImageVM = new List<LibraryImageVM>()
                        };
                        // truyen file sang backend.
                        var formDataContent = new MultipartFormDataContent();
                        foreach (var item in model.LibraryImageIF)
                        {
                            var streamContent = new StreamContent(item.OpenReadStream());
                            streamContent.Headers.ContentType = new MediaTypeHeaderValue(item.ContentType);
                            formDataContent.Add(streamContent, "files", item.FileName);
                        }
                        
                        var responseLibrary = await _httpClient.PostAsync("https://localhost:7252/api/Upload/LibraryImages", formDataContent);
                        // neu thanh cong thi lay file string
                        if (responseLibrary.IsSuccessStatusCode)
                        {

                            var responseData = await responseLibrary.Content.ReadAsStringAsync();
                            var fileNames = JsonConvert.DeserializeObject<List<string>>(responseData);

                            foreach (var fileName in fileNames)
                            {
                                // set ten va them vao MotoLibrary
                                var image = new LibraryImageVM();
                                image.MaLibrary = newMoto.MaLibrary;
                                image.ImageUrl = fileName;
                                motoLibrary.LibraryImageVM.Add(image);
                            }
                        }
                        else
                        {
                            _logger.LogError("Error creating library Image");
                            ModelState.AddModelError(string.Empty, "Error creating library Image");
                        }

                        var libraryCreate = new StringContent(JsonConvert.SerializeObject(motoLibrary), Encoding.UTF8, "application/json");
                        var libraryCreateResult = await _httpClient.PutAsync("https://localhost:7252/api/Library/Libraries/" + newMoto.MaLibrary, libraryCreate);

                        // kiem tra trang thai tao library
                        if (libraryCreateResult.IsSuccessStatusCode)
                        {
                            _logger.LogWarning("Create Library Thanh Cong___" + motoLibrary.MaLibrary);

                        }
                        else
                        {
                            _logger.LogError("Loi Create Library Khong Thanh Cong");
                            ModelState.AddModelError(string.Empty, "Error creating Library");
                        }
                        #endregion
                    }
                    else
                    {
                        _logger.LogError("Khong xoa duoc library");
                    }
                }

                var content = new StringContent(JsonConvert.SerializeObject(newMoto), Encoding.UTF8, "application/json");
                var responseMoto = await _httpClient.PutAsync("https://localhost:7252/api/Moto/Motos/"+newMoto.MaXe, content);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Luu Xe Thanh Cong");
                    return RedirectToAction("ListMoto", "Moto");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, errorMessage);
                    return View(model);
                }
                else
                {
                    _logger.LogError("Error creating Moto");
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, "Error creating Moto ___ " + errorMessage);

                    //lay ra xe
                    var responseMoto1 = await _httpClient.GetAsync("https://localhost:7252/api/Moto/Motos/" + maXe);
                    responseMoto1.EnsureSuccessStatusCode();
                    var dataMoto = await responseMoto1.Content.ReadAsStringAsync();
                    var moto = JsonConvert.DeserializeObject<MotoVM>(dataMoto);

                    //lay hinh anh
                    var responseLibrary = await _httpClient.GetAsync("https://localhost:7252/api/Library/Libraries/" + moto.MaLibrary);
                    responseLibrary.EnsureSuccessStatusCode();
                    var librarydata = await responseLibrary.Content.ReadAsStringAsync();
                    var library = JsonConvert.DeserializeObject<LibraryVM>(librarydata);
                    moto.MaLibraryNavigation = library;

                    // lay brand
                    var responseBrand = await _httpClient.GetAsync("https://localhost:7252/api/Brand/Brands");
                    responseBrand.EnsureSuccessStatusCode();
                    var dataBrand = await responseBrand.Content.ReadAsStringAsync();
                    var brands = JsonConvert.DeserializeObject<List<BrandVM>>(dataBrand);

                    //lay Type
                    var response1 = await _httpClient.GetAsync("https://localhost:7252/api/Type/Types");
                    response1.EnsureSuccessStatusCode();
                    var data1 = await response1.Content.ReadAsStringAsync();
                    var types = JsonConvert.DeserializeObject<List<MotoTypeVM>>(data1);

                    var model1 = new EditMotoVM
                    {
                        MotoBike = moto,
                        Brands = brands,
                        MotoTypes = types,
                    };
                    return View(model1);
                }              
            }
            else
            {
                //lay ra xe
                var responseMoto1 = await _httpClient.GetAsync("https://localhost:7252/api/Moto/Motos/" + maXe);
                responseMoto1.EnsureSuccessStatusCode();
                var dataMoto = await responseMoto1.Content.ReadAsStringAsync();
                var moto = JsonConvert.DeserializeObject<MotoVM>(dataMoto);

                //lay hinh anh
                var responseLibrary = await _httpClient.GetAsync("https://localhost:7252/api/Library/Libraries/" + moto.MaLibrary);
                responseLibrary.EnsureSuccessStatusCode();
                var librarydata = await responseLibrary.Content.ReadAsStringAsync();
                var library = JsonConvert.DeserializeObject<LibraryVM>(librarydata);
                moto.MaLibraryNavigation = library;

                // lay brand
                var responseBrand = await _httpClient.GetAsync("https://localhost:7252/api/Brand/Brands");
                responseBrand.EnsureSuccessStatusCode();
                var dataBrand = await responseBrand.Content.ReadAsStringAsync();
                var brands = JsonConvert.DeserializeObject<List<BrandVM>>(dataBrand);

                //lay Type
                var response1 = await _httpClient.GetAsync("https://localhost:7252/api/Type/Types");
                response1.EnsureSuccessStatusCode();
                var data1 = await response1.Content.ReadAsStringAsync();
                var types = JsonConvert.DeserializeObject<List<MotoTypeVM>>(data1);

                var model1 = new EditMotoVM
                {
                    MotoBike = moto,
                    Brands = brands,
                    MotoTypes = types,
                };
                return View(model1);
            }

        }
    }
}