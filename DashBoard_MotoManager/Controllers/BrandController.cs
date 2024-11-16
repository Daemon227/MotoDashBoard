using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Helpers;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace DashBoard_MotoManager.Controllers
{
    public class BrandController : Controller
    {
        private readonly MotoWebsiteContext _db;
        private readonly ILogger<MotoController> _logger;
        public BrandController(MotoWebsiteContext context, ILogger<MotoController> logger)
        {
            _db = context;
            _logger = logger;
        }


        public IActionResult ListBrand(int? page)
        {
            int pageSize = 6;  // Số lượng mục mỗi trang
            int pageNumber = (page ?? 1); // Nếu page là null, gán giá trị mặc định là 1

            var brands = _db.Brands.AsQueryable();


            // Phân trang dữ liệu
            var result = brands.Select(b => new Brand
            {
                MaHangSanXuat = b.MaHangSanXuat,
                TenHangSanXuat = b.TenHangSanXuat,
                QuocGia = b.QuocGia,
                MoTaNgan = b.MoTaNgan,
            }).ToPagedList(pageNumber, pageSize); // Dùng ToPagedList để phân trang

            return View(result);
        }

        public IActionResult SeeDetail(string? brandID)
        {
            if (brandID != null)
            {
                var brand = _db.Brands.FirstOrDefault(b => b.MaHangSanXuat == brandID);
                var model = new BrandVM
                {
                    MaHangSanXuat = brand.MaHangSanXuat,
                    TenHangSanXuat = brand.TenHangSanXuat,
                    QuocGia = brand.QuocGia,
                    MoTaNgan = brand.MoTaNgan,
                };
                return View(model);
            }
            else return NotFound();
        }
        

        [HttpGet]
        public IActionResult Addbrand()
        {
            BrandVM brand = new BrandVM();
            return View(brand); 
        }

        [HttpPost]
        public IActionResult AddBrand(BrandVM model)
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
                var brandCheck = _db.Brands.FirstOrDefault(b => b.TenHangSanXuat.Equals(brand.TenHangSanXuat));
                if (brandCheck != null) 
                {
                    return View(model);
                }
                else 
                {  
                    _db.Brands.Add(brand);
                    try
                    {
                        var result = _db.SaveChanges();
                        if (result > 0)
                        {
                            _logger.LogError("Luu Brand Thanh Cong");
                            return RedirectToAction("ListBrand", "Brand");
                        }
                        else
                        {
                            _logger.LogError("Không lưu được brand, không có hàng nào bị ảnh hưởng.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi khi lưu brand vào cơ sở dữ liệu.");
                        throw; // Ném lại lỗi để dễ dàng kiểm tra khi debug
                    }
                }
            }
            return View(model);
        }

        public IActionResult RemoveBrand(string brandId)
        {
            var brand = _db.Brands.FirstOrDefault(b => b.MaHangSanXuat == brandId);
            if (brand == null)
            {
                return NotFound();
            }

            _db.Brands.Remove(brand);
            try
            {
                var result = _db.SaveChanges();
                if (result > 0)
                {
                    _logger.LogError("Xóa Brand Thanh Cong");
                    return RedirectToAction("ListBrand", "Brand");
                }
                else
                {
                    _logger.LogError("Không Xóa được brand, không có hàng nào bị ảnh hưởng.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi Xóa brand vào cơ sở dữ liệu.");
                throw; // Ném lại lỗi để dễ dàng kiểm tra khi debug
            }
            return RedirectToAction("ListBrand");
            
        }

        [HttpGet]
        public IActionResult UpdateBrand(string brandId)
        {
            var brand = _db.Brands.FirstOrDefault(b=>b.MaHangSanXuat==brandId);
            
            if (brand == null) 
            { 
                return NotFound();
            }
            else
            {
                var model = new BrandVM
                {
                    MaHangSanXuat = brandId,
                    TenHangSanXuat = brand.TenHangSanXuat,
                    QuocGia = brand.QuocGia,
                    MoTaNgan = brand.MoTaNgan,
                };
                return View(model);
            }
        }
        [HttpPost]
        public IActionResult UpdateBrand(BrandVM model, string brandId)
        {
            if (ModelState.IsValid)
            {
                var brand = _db.Brands.FirstOrDefault(b => b.MaHangSanXuat == brandId);
                if (brand != null)
                {
                    brand.TenHangSanXuat = model.TenHangSanXuat;
                    brand.QuocGia = model.QuocGia;
                    brand.MoTaNgan= model.MoTaNgan;

                    try
                    {
                        var result = _db.SaveChanges();
                        if (result > 0)
                        {
                            _logger.LogError("Sửa Brand Thanh Cong");
                            return RedirectToAction("ListBrand", "Brand");
                        }
                        else
                        {
                            _logger.LogError("Không Sửa được brand, không có hàng nào bị ảnh hưởng.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi khi Sửa brand vào cơ sở dữ liệu.");
                        throw; // Ném lại lỗi để dễ dàng kiểm tra khi debug
                    }
                    return RedirectToAction("ListBrand");
                }
                else
                {
                    return NotFound();
                }
            }
            else return View(model);
        }
    }
}
