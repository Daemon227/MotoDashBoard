using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Helpers;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace DashBoard_MotoManager.Controllers
{
    public class MotoTypeController : Controller
    {
        private readonly MotoWebsiteContext _db;
        private readonly ILogger<MotoController> _logger;
        public MotoTypeController(MotoWebsiteContext context, ILogger<MotoController> logger)
        {
            _db = context;
            _logger = logger;
        }
        public IActionResult ListType(int? page)
        {
            int pageSize = 6;  // Số lượng mục mỗi trang
            int pageNumber = (page ?? 1); // Nếu page là null, gán giá trị mặc định là 1

            var types = _db.MotoTypes.AsQueryable();


            // Phân trang dữ liệu
            var result = types.Select(b => new MotoType
            {
                MaLoai = b.MaLoai,
                TenLoai = b.TenLoai,
            }).ToPagedList(pageNumber, pageSize); // Dùng ToPagedList để phân trang

            return View(result);
        }

        [HttpGet]
        public IActionResult AddType()
        {
            MotoTypeVM model = new MotoTypeVM();
            return View(model);
        }

        [HttpPost]
        public IActionResult AddType(MotoTypeVM model)
        {
            if (ModelState.IsValid)
            {
                var type = new MotoType
                {
                    MaLoai = MyTool.GenarateRandomKey(),
                    TenLoai = model.TenLoai,
                };
                var typeCheck = _db.MotoTypes.FirstOrDefault(t=>t.MaLoai.Equals(type.MaLoai));
                if (typeCheck != null)
                {
                    return View(model);
                }
                else
                {
                    _db.MotoTypes.Add(type);
                    try
                    {
                        var result = _db.SaveChanges();
                        if (result > 0)
                        {
                            _logger.LogError("Luu Loai Xe Thanh Cong");
                            return RedirectToAction("ListType", "MotoType");
                        }
                        else
                        {
                            _logger.LogError("Không lưu được type, không có hàng nào bị ảnh hưởng.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi khi lưu type vào cơ sở dữ liệu.");
                        throw; // Ném lại lỗi để dễ dàng kiểm tra khi debug
                    }
                }
            }
            return View(model);
        }

        public IActionResult RemoveType(string typeId)
        {
            var type = _db.MotoTypes.FirstOrDefault(t => t.MaLoai == typeId);
            if (type == null)
            {
                return NotFound();
            }

            _db.MotoTypes.Remove(type);
            try
            {
                var result = _db.SaveChanges();
                if (result > 0)
                {
                    _logger.LogError("Xóa Loai Xe Thanh Cong");
                    return RedirectToAction("ListType", "MotoType");
                }
                else
                {
                    _logger.LogError("Không xóa được type, không có hàng nào bị ảnh hưởng.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa type vào cơ sở dữ liệu.");
                throw; // Ném lại lỗi để dễ dàng kiểm tra khi debug
            }
            return RedirectToAction("ListType");

        }

        [HttpGet]
        public IActionResult UpdateType(string typeId)
        {
            var type = _db.MotoTypes.FirstOrDefault(t => t.MaLoai == typeId);

            if (type == null)
            {
                return NotFound();
            }
            else
            {
                var model = new MotoTypeVM
                {
                    MaLoai = typeId,
                    TenLoai = type.TenLoai,
                };
                return View(model);
            }
        }
        [HttpPost]
        public IActionResult UpdateType(MotoTypeVM model, string typeId)
        {
            if (ModelState.IsValid)
            {
                var type = _db.MotoTypes.FirstOrDefault(t => t.MaLoai == typeId);
                if (type != null)
                {
                    type.TenLoai = model.TenLoai;
                    try
                    {
                        var result = _db.SaveChanges();
                        if (result > 0)
                        {
                            _logger.LogError("Xóa Loai Xe Thanh Cong");
                            return RedirectToAction("ListType", "MotoType");
                        }
                        else
                        {
                            _logger.LogError("Không xóa được type, không có hàng nào bị ảnh hưởng.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi khi xóa type vào cơ sở dữ liệu.");
                        throw; // Ném lại lỗi để dễ dàng kiểm tra khi debug
                    }
                    return RedirectToAction("ListType");
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
