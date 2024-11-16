using AutoMapper;
using DashBoard_MotoManager.Datas;
using DashBoard_MotoManager.Helpers;
using DashBoard_MotoManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.Contracts;
using System.Runtime.ConstrainedExecution;
using X.PagedList.Extensions;

namespace DashBoard_MotoManager.Controllers
{
    public class MotoController : Controller
    {
        private readonly MotoWebsiteContext _db;
        private readonly ILogger<MotoController> _logger;
        private readonly IMapper _mapper;
        public MotoController(MotoWebsiteContext context, ILogger<MotoController> logger, IMapper mapper)
        {
            _db = context;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult ListMoto(int? page, string? maLoai)
        {
            int pageSize = 6;  // Số lượng mục mỗi trang
            int pageNumber = (page ?? 1); // Nếu page là null, gán giá trị mặc định là 1

            var motos = _db.MotoBikes.AsQueryable();

            if (!string.IsNullOrEmpty(maLoai))
            {
                motos = motos.Where(m => m.MaLoai.Equals(maLoai));
            }

            // Phân trang dữ liệu
            var result = motos.Select(p => new MotoListVM
            {
                MaXe = p.MaXe,
                TenXe = p.TenXe,
                GiaBanMoTa = p.GiaBanMoTa,
                AnhMoTaUrl = p.AnhMoTaUrl,
            }).ToPagedList(pageNumber, pageSize); // Dùng ToPagedList để phân trang

            return View(result);
        }

        public async Task<IActionResult> SeeDetail(string? maXe)
        {
            if (maXe == null)
            {
                return BadRequest("ID mismatch.");
            }
            var moto = await _db.MotoBikes.Include(p => p.MotoVersions)
                    .ThenInclude(v => v.VersionColors)
                        .ThenInclude(i => i.VersionImages)
                    .Include(l => l.MaLibraryNavigation)
                        .ThenInclude(i2 => i2.LibraryImages)
                    .Include(b => b.MaHangSanXuatNavigation)
                    .Include(t => t.MaLoaiNavigation)
                        .FirstOrDefaultAsync(p => p.MaXe == maXe);

            if (moto == null)
            {
                return NotFound("Moto not found.");
            }
            else
            {
                var motoVM = _mapper.Map<MotoDetailVM>(moto);
                return View(motoVM);
            }
        }

        /*public IActionResult SeeDetail(string? maXe)
        {
            if (!string.IsNullOrEmpty(maXe))
            {

            }
            //string id = "00";
            var moto = _db.MotoBikes.Include(p => p.MotoVersions)
                    .ThenInclude(v => v.VersionColors)
                        .ThenInclude(i => i.VersionImages)
                    .Include(l => l.MaLibraryNavigation)
                        .ThenInclude(i2 => i2.LibraryImages)
                    .Include(b => b.MaHangSanXuatNavigation)
                    .Include(t => t.MaLoaiNavigation)
                        .FirstOrDefault(p => p.MaXe == maXe);

            var result = new MotoDetailVM
            {
                MaXe = moto.MaXe,
                TenXe = moto.TenXe ?? "",
                MaHangSanXuat = moto.MaHangSanXuat ?? "",
                AnhMoTaUrl = moto.AnhMoTaUrl ?? "",
                GiaBanMoTa = moto.GiaBanMoTa ?? "",

                TrongLuong = moto.TrongLuong ?? "",
                KichThuoc = moto.KichThuoc ?? "",
                KhoangCachTrucBanhXe = moto.KhoangCachTrucBanhXe ?? "",
                DoCaoYen = moto.DoCaoYen ?? "",
                DoCaoGamXe = moto.DoCaoGamXe ?? "",
                DungTichBinhXang = moto.DungTichBinhXang ?? "",
                KichCoLop = moto.KichCoLop ?? "",
                PhuocTruoc = moto.PhuocTruoc ?? "",
                PhuocSau = moto.PhuocSau ?? "",
                LoaiDongCo = moto.LoaiDongCo ?? "",
                CongSuatToiDa = moto.CongSuatToiDa ?? "",
                MucTieuThuNhienLieu = moto.MucTieuThuNhienLieu ?? "",
                HeThongKhoiDong = moto.HeThongKhoiDong ?? "",
                MomentCucDai = moto.MomentCucDai ?? "",
                DungTichXyLanh = moto.DungTichXyLanh ?? "",
                DuongKinhHanhTrinhPittong = moto.DuongKinhHanhTrinhPittong ?? "",
                TySoNen = moto.TySoNen ?? "",
                MaLibrary = moto.MaLibrary,

                MaHangSanXuatNavigation = new Brand
                {
                    TenHangSanXuat = moto.MaHangSanXuatNavigation.TenHangSanXuat,
                    MaHangSanXuat = moto.MaHangSanXuatNavigation.MaHangSanXuat
                },

                MaLibraryNavigation = new MotoLibrary
                {
                    MaLibrary = moto.MaLibraryNavigation.MaLibrary,
                    LibraryImages = moto.MaLibraryNavigation.LibraryImages.Select(img => new LibraryImage
                    {
                        ImageId = img.ImageId,
                        ImageUrl = img.ImageUrl,
                    }).ToList()
                },

                MaLoaiNavigation = new MotoType
                {
                    MaLoai = moto.MaLoaiNavigation.MaLoai,
                    TenLoai = moto.MaLoaiNavigation.TenLoai
                },

                TienIch = moto.TienIch,
                TinhNangNoiBat = moto.TinhNangNoiBat,
                ThietKe = moto.ThietKe,

                MotoVersions = moto.MotoVersions.Select(v => new MotoVersion
                {
                    MaVersion = v.MaVersion,
                    TenVersion = v.TenVersion ?? "",
                    GiaBanVersion = v.GiaBanVersion ?? "",
                    VersionColors = v.VersionColors.Select(c => new VersionColor
                    {
                        MaVersionColor = c.MaVersionColor,
                        TenMau = c.TenMau,
                        VersionImages = c.VersionImages.Select(i => new VersionImage
                        {
                            ImageId = i.ImageId,
                            ImageUrl = i.ImageUrl,
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
            return View(result);
        }*/

        [HttpGet]
        public IActionResult AddMoto()
        {
            var model = new AddMotoVM
            {
                Brands = _db.Brands.ToList(),
                MotoTypes = _db.MotoTypes.ToList(),
            };
            return View(model);
        }
        [HttpPost]
        public IActionResult AddMoto(AddMotoVM model)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    _logger.LogError("NULLLL CMNR");
                    return View();
                }
                else
                {
                    #region LOGGER
                    _logger.LogError("Play roi ------");
                    if (model.MotoVersionsVM != null)
                    {
                        _logger.LogError("-------MotoVersionVM k null ------");
                        _logger.LogError("Co bang nay version" + model.MotoVersionsVM.Count);
                        foreach (var m in model.MotoVersionsVM)
                        {
                            _logger.LogError("Ten Pb: " + m.TenVersion);
                            _logger.LogError("Gia Ban: " + m.GiaBanVersion);
                            _logger.LogError("Phien ban Nay có so mau la: " + m.VersionColorsVM.Count);
                            foreach (var c in m.VersionColorsVM)
                            {
                                _logger.LogError("Ten mau: " + c.TenMau);
                                _logger.LogError(c.TenMau + "có bằng này hình" + c.versionImageIF.Count);
                                foreach (var f in c.versionImageIF)
                                {
                                    _logger.LogError("Ten file Anh: " + f.FileName);
                                }
                            }
                        }
                    }
                    else
                    {
                        _logger.LogError("--------MotoversionVm null");
                    }
                    if (model.AnhMoTaIF != null)
                    {
                        //_logger.LogError("Anh nay leeeeeeuueueueueue"+MyTool.UploadHinh(model.AnhMoTaIF, "MoTa"));
                        _logger.LogError("Anh mota nay: " + model.AnhMoTaIF.FileName);
                    }
                    else { _logger.LogError("File Anh MOTA NULL"); };

                    if (model.LibraryImageIF != null)
                    {
                        foreach (var img in model.LibraryImageIF)
                        {
                            _logger.LogError("Anh Library" + img.FileName);
                        }
                    }
                    else _logger.LogError("Anh Library is NULL");
                    #endregion

                    string libraryId = MyTool.GenarateRandomKey();
                    var moto = new MotoBike
                    {
                        MaXe = MyTool.GenarateRandomKey(),
                        TenXe = model.MotoBike.TenXe,
                        MaLoai = model.MotoBike.MaLoai,
                        //
                        MaHangSanXuat = model.MotoBike.MaHangSanXuat,
                        //
                        AnhMoTaUrl = MyTool.UploadHinh(model.AnhMoTaIF, "MoTa"),
                        //AnhMoTaUrl = "anhmota",
                        GiaBanMoTa = model.MotoBike.GiaBanMoTa,
                        TrongLuong = model.MotoBike.TrongLuong,
                        KichThuoc = model.MotoBike.KichThuoc,
                        KhoangCachTrucBanhXe = model.MotoBike.KhoangCachTrucBanhXe,
                        DoCaoYen = model.MotoBike.DoCaoYen,
                        DoCaoGamXe = model.MotoBike.DoCaoGamXe,
                        DungTichBinhXang = model.MotoBike.DungTichBinhXang,
                        KichCoLop = model.MotoBike.KichCoLop,
                        PhuocTruoc = model.MotoBike.PhuocTruoc,
                        PhuocSau = model.MotoBike.PhuocSau,
                        LoaiDongCo = model.MotoBike.LoaiDongCo,
                        CongSuatToiDa = model.MotoBike.CongSuatToiDa,
                        MucTieuThuNhienLieu = model.MotoBike.MucTieuThuNhienLieu,
                        HeThongKhoiDong = model.MotoBike.HeThongKhoiDong,
                        MomentCucDai = model.MotoBike.MomentCucDai,
                        DungTichXyLanh = model.MotoBike.DungTichXyLanh,
                        DuongKinhHanhTrinhPittong = model.MotoBike.DuongKinhHanhTrinhPittong,
                        TySoNen = model.MotoBike.TySoNen,
                        TinhNangNoiBat = model.MotoBike.TinhNangNoiBat,
                        ThietKe = model.MotoBike.ThietKe,
                        TienIch = model.MotoBike.TienIch,
                        MaLibrary = libraryId,

                    };
                    //them library
                    var motoLibray = new MotoLibrary
                    {
                        MaLibrary = moto.MaLibrary
                    };
                    _db.MotoLibraries.Add(motoLibray);
                    // them anh vao thu vien
                    foreach (var imgF in model.LibraryImageIF)
                    {
                        string imgUrl = MyTool.UploadHinh(imgF, "LibraryImgs");
                        var libraryImg = new LibraryImage
                        {
                            MaLibrary = moto.MaLibrary,
                            ImageUrl = imgUrl,
                        };
                        _db.LibraryImages.Add(libraryImg);
                    }
                    //them xe
                    _db.Add(moto);

                    // them version
                    foreach (var version in model.MotoVersionsVM)
                    {
                        var v = new MotoVersion
                        {
                            MaXe = moto.MaXe,
                            MaVersion = MyTool.GenarateRandomKey(),
                            TenVersion = version.TenVersion,
                            GiaBanVersion = version.GiaBanVersion,
                        };
                        _db.MotoVersions.Add(v);

                        foreach (var color in version.VersionColorsVM)
                        {
                            var c = new VersionColor
                            {
                                MaVersion = v.MaVersion,
                                MaVersionColor = MyTool.GenarateRandomKey(),
                                TenMau = color.TenMau,
                            };
                            _db.VersionColors.Add(c);
                            foreach (var file in color.versionImageIF)
                            {
                                string imgUrl = MyTool.UploadHinh(file, "AnhVersion");
                                var i = new VersionImage
                                {
                                    MaVersionColor = c.MaVersionColor,
                                    ImageUrl = imgUrl
                                };
                                _db.VersionImages.Add(i);

                            }
                        }
                    }

                    try
                    {
                        var result = _db.SaveChanges();
                        if (result > 0)
                        {
                            _logger.LogError("Luu Xe Thanh Cong");
                            return RedirectToAction("ListMoto", "Moto");
                        }
                        else
                        {
                            _logger.LogError("Không lưu được XE, không có hàng nào bị ảnh hưởng.");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Lỗi khi lưu XE vào cơ sở dữ liệu.");
                        throw; // Ném lại lỗi để dễ dàng kiểm tra khi debug
                    }
                    return View();
                }
            }
            else
            {
                model.Brands = _db.Brands.ToList();
                model.MotoTypes = _db.MotoTypes.ToList();
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult RemoveMoto(string? maXe)
        {
            if (maXe == null)
            {

            }
            //string id = "00";
            var moto = _db.MotoBikes.FirstOrDefault(p => p.MaXe == maXe);
            var result = new MotoDetailVM
            {
                MaXe = moto.MaXe,
                TenXe = moto.TenXe ?? "",
                MaHangSanXuat = moto.MaHangSanXuat ?? "",
                AnhMoTaUrl = moto.AnhMoTaUrl ?? "",
                GiaBanMoTa = moto.GiaBanMoTa ?? "",
            };
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmRemoveMoto(string maXe)
        {
            if (!string.IsNullOrEmpty(maXe))
            {

            }
            //string id = "00";
            var moto = _db.MotoBikes.Include(p => p.MotoVersions)
                    .ThenInclude(v => v.VersionColors)
                        .ThenInclude(i => i.VersionImages)
                    .Include(l => l.MaLibraryNavigation)
                        .ThenInclude(i2 => i2.LibraryImages)
                    .Include(b => b.MaHangSanXuatNavigation)
                    .Include(t => t.MaLoaiNavigation)
                        .FirstOrDefault(p => p.MaXe == maXe);

            if (moto == null)
            {
                _logger.LogError("Khong tim thay xe co ma " +maXe);
            }
            else
            {
                // xoa version truoc nay
                foreach (var v in moto.MotoVersions)
                {
                    foreach (var c in v.VersionColors)
                    {
                        foreach (var i in c.VersionImages)
                        {
                            
                            var imageToDelete = _db.VersionImages.FirstOrDefault(img => img.ImageId == i.ImageId);
                            if (imageToDelete != null)
                            {
                                _db.VersionImages.Remove(imageToDelete); // Xóa ảnh khỏi cơ sở dữ liệu
                                MyTool.DeleteImg(i.ImageUrl, "AnhVersion");
                            }
                            else
                            {
                                _logger.LogError("imageVersionToDelete is null");
                            }
                        }
                        var colorToDelete = _db.VersionColors.FirstOrDefault(color => color.MaVersionColor == c.MaVersionColor);
                        if (colorToDelete != null)
                        {
                            _db.VersionColors.Remove(colorToDelete); // Xóa màu khỏi cơ sở dữ liệu
                        }
                        else
                        {
                            _logger.LogError("colorVersionToDelete is null");
                        }
                    }
                    var versionToDelete = _db.MotoVersions.FirstOrDefault(Version => Version.MaVersion == v.MaVersion);
                    if (versionToDelete != null)
                    {
                        _db.MotoVersions.Remove(versionToDelete);
                    }
                    else
                    {
                        _logger.LogError("motoVersionToDelete is null");
                    }
                }

                // xoa moto
                var motoToRemove = _db.MotoBikes.FirstOrDefault(m=> m.MaXe == moto.MaXe);
                MyTool.DeleteImg(motoToRemove.AnhMoTaUrl, "MoTa");
                _db.MotoBikes.Remove(motoToRemove);

                // xóa anh trong thư viện
                foreach (var i in moto.MaLibraryNavigation.LibraryImages)
                { 
                    var imgToDelete = _db.LibraryImages.FirstOrDefault(img => img.ImageId == i.ImageId);
                    if (imgToDelete != null) 
                    {
                        MyTool.DeleteImg(i.ImageUrl, "LibraryImgs");
                        _db.LibraryImages.Remove(imgToDelete);
                    }
                    else
                    {
                        _logger.LogError("imageLibraryToDelete is null");
                    }
                }
                
                // xoa thu vien

                var libraryToRemove = _db.MotoLibraries.FirstOrDefault(l=>l.MaLibrary == moto.MaLibrary);
                if (libraryToRemove != null)
                {
                    _db.MotoLibraries.Remove(libraryToRemove);
                }
                else
                {
                    _logger.LogError("library is null");
                }

                try
                {
                    var result = _db.SaveChanges();
                    if (result > 0)
                    {
                        _logger.LogError("Xoa Xe Thanh Cong");
                        return RedirectToAction("ListMoto", "Moto");
                    }
                    else
                    {
                        _logger.LogError("Không xoa được XE,khong có hàng nào bị ảnh hưởng.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi XOA XE vào cơ sở dữ liệu.");
                    throw; // Ném lại lỗi để dễ dàng kiểm tra khi debug
                }
            }

            return RedirectToAction("ListMoto", "Moto");
        }

        [HttpGet]
        public IActionResult EditMoto(string maXe)
        {
            if (!string.IsNullOrEmpty(maXe))
            {

            }
            //string id = "00";
            var moto = _db.MotoBikes.Include(p => p.MotoVersions)
                    .ThenInclude(v => v.VersionColors)
                        .ThenInclude(i => i.VersionImages)
                    .Include(l => l.MaLibraryNavigation)
                        .ThenInclude(i2 => i2.LibraryImages)
                    .Include(b => b.MaHangSanXuatNavigation)
                    .Include(t => t.MaLoaiNavigation)
                        .FirstOrDefault(p => p.MaXe == maXe);

            var motoBike = new MotoBike
            {
                MaXe = moto.MaXe,
                TenXe = moto.TenXe ?? "",
                MaHangSanXuat = moto.MaHangSanXuat ?? "",
                MaLoai = moto.MaLoai ?? "",
                AnhMoTaUrl = moto.AnhMoTaUrl ?? "",
                GiaBanMoTa = moto.GiaBanMoTa ?? "",

                TrongLuong = moto.TrongLuong ?? "",
                KichThuoc = moto.KichThuoc ?? "",
                KhoangCachTrucBanhXe = moto.KhoangCachTrucBanhXe ?? "",
                DoCaoYen = moto.DoCaoYen ?? "",
                DoCaoGamXe = moto.DoCaoGamXe ?? "",
                DungTichBinhXang = moto.DungTichBinhXang ?? "",
                KichCoLop = moto.KichCoLop ?? "",
                PhuocTruoc = moto.PhuocTruoc ?? "",
                PhuocSau = moto.PhuocSau ?? "",
                LoaiDongCo = moto.LoaiDongCo ?? "",
                CongSuatToiDa = moto.CongSuatToiDa ?? "",
                MucTieuThuNhienLieu = moto.MucTieuThuNhienLieu ?? "",
                HeThongKhoiDong = moto.HeThongKhoiDong ?? "",
                MomentCucDai = moto.MomentCucDai ?? "",
                DungTichXyLanh = moto.DungTichXyLanh ?? "",
                DuongKinhHanhTrinhPittong = moto.DuongKinhHanhTrinhPittong ?? "",
                TySoNen = moto.TySoNen ?? "",
                MaLibrary = moto.MaLibrary,

                MaHangSanXuatNavigation = new Brand
                {
                    TenHangSanXuat = moto.MaHangSanXuatNavigation.MaHangSanXuat,
                    MaHangSanXuat = moto.MaHangSanXuatNavigation.TenHangSanXuat
                },

                MaLibraryNavigation = new MotoLibrary
                {
                    MaLibrary = moto.MaLibraryNavigation.MaLibrary,
                    LibraryImages = moto.MaLibraryNavigation.LibraryImages.Select(img => new LibraryImage
                    {
                        ImageId = img.ImageId,
                        ImageUrl = img.ImageUrl,
                    }).ToList()
                },

                MaLoaiNavigation = new MotoType
                {
                    MaLoai = moto.MaLoaiNavigation.MaLoai,
                    TenLoai = moto.MaLoaiNavigation.TenLoai
                },

                TienIch = moto.TienIch,
                TinhNangNoiBat = moto.TinhNangNoiBat,
                ThietKe = moto.ThietKe,

                MotoVersions = moto.MotoVersions.Select(v => new MotoVersion
                {
                    MaVersion = v.MaVersion,
                    TenVersion = v.TenVersion ?? "",
                    GiaBanVersion = v.GiaBanVersion ?? "",
                    VersionColors = v.VersionColors.Select(c => new VersionColor
                    {
                        MaVersionColor = c.MaVersionColor,
                        TenMau = c.TenMau,
                        VersionImages = c.VersionImages.Select(i => new VersionImage
                        {
                            ImageId = i.ImageId,
                            ImageUrl = i.ImageUrl,
                        }).ToList()
                    }).ToList()
                }).ToList()
            };
            var brand = _db.Brands.ToList();

            var model = new EditMotoVM
            {
                MotoBike = motoBike,
                Brands = _db.Brands.ToList(),
                MotoTypes = _db.MotoTypes.ToList(),
            };
            foreach (var v in motoBike.MotoVersions)
            {
                var versionVM = new MotoVersionVM
                {
                    MaVersion = v.MaVersion,
                    TenVersion = v.TenVersion,
                    GiaBanVersion = v.GiaBanVersion,
                };
                foreach (var c in v.VersionColors)
                {
                    var colorVM = new VersionColorVM
                    {
                        MaVersionColor = c.MaVersionColor,
                        TenMau = c.TenMau,
                    };
                    foreach(var i in c.VersionImages)
                    {
                        var imgUrl = new VersionImageVM
                        {
                            ImageId = i.ImageId,
                            ImageUrl = i.ImageUrl,
                        };
                        colorVM.versionImageVM.Add(imgUrl);
                    }
                    versionVM.VersionColorsVM.Add(colorVM);
                }
                model.MotoVersionsVM.Add(versionVM);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult EditMoto(EditMotoVM model, string maXe)
        {
            if (ModelState.IsValid)
            {
                if (model == null)
                {
                    _logger.LogError(" Model Editmoto NULLLL");
                    return View(maXe);
                }
                else
                {
                    _logger.LogError("Ma cua model :" + model.MotoBike.MaXe);
                    _logger.LogError("Ma tu model la "+ maXe);
                    var moto = _db.MotoBikes.Include(p => p.MotoVersions)
                   .ThenInclude(v => v.VersionColors)
                       .ThenInclude(i => i.VersionImages)
                       .FirstOrDefault(p => p.MaXe == maXe);
                    if (moto == null)
                    {
                        _logger.LogError("Khong tim thay moto trong database");
                    }
                    else
                    {
                        #region Set XE
                        moto.TenXe = model.MotoBike.TenXe;
                        moto.MaLoai = model.MotoBike.MaLoai ?? "";
                        moto.MaHangSanXuat = model.MotoBike.MaHangSanXuat ?? "";
                        if (model.AnhMoTaIF !=null)
                        {
                            var imageName = model.AnhMoTaIF?.Name;
                            MyTool.DeleteImg(imageName, "MoTa");
                            moto.AnhMoTaUrl = MyTool.UploadHinh(model.AnhMoTaIF, "MoTa");
                        }
                        moto.GiaBanMoTa = model.MotoBike.GiaBanMoTa;
                        moto.TrongLuong = model.MotoBike.TrongLuong;
                        moto.KhoangCachTrucBanhXe = model.MotoBike.KhoangCachTrucBanhXe;
                        moto.DoCaoYen = model.MotoBike.DoCaoYen;
                        moto.DoCaoGamXe = model.MotoBike.DoCaoGamXe;
                        moto.DungTichBinhXang = model.MotoBike.DungTichBinhXang;
                        moto.KichCoLop = model.MotoBike.KichCoLop;
                        moto.PhuocTruoc = model.MotoBike.PhuocTruoc;
                        moto.PhuocSau = model.MotoBike.PhuocSau;
                        moto.LoaiDongCo = model.MotoBike.LoaiDongCo;
                        moto.CongSuatToiDa = model.MotoBike.CongSuatToiDa;
                        moto.MucTieuThuNhienLieu = model.MotoBike.MucTieuThuNhienLieu;
                        moto.HeThongKhoiDong = model.MotoBike.HeThongKhoiDong;
                        moto.MomentCucDai = model.MotoBike.MomentCucDai;
                        moto.DungTichXyLanh = model.MotoBike.DungTichXyLanh;
                        moto.DuongKinhHanhTrinhPittong = model.MotoBike.DuongKinhHanhTrinhPittong;
                        moto.TySoNen = model.MotoBike.TySoNen;
                        moto.TinhNangNoiBat = model.MotoBike.TinhNangNoiBat;
                        moto.ThietKe = model.MotoBike.ThietKe;
                        moto.TienIch = model.MotoBike.TienIch;
                        #endregion

                        #region Set Library
                        //_logger.LogError("ma library ==" + moto.MaLibrary);
                        var library = _db.MotoLibraries.Include(ib => ib.LibraryImages)
                            .FirstOrDefault(l => l.MaLibrary == moto.MaLibrary);
                        //_logger.LogError("Ma cua library trong database "+ library.MaLibrary);
                        foreach (var imgF in model.LibraryImageIF)
                        {
                            foreach (var i in library.LibraryImages)
                            {
                                var imgToDelete = _db.LibraryImages.FirstOrDefault(img => img.ImageId == i.ImageId);
                                //_logger.LogError("Ma anh de xoa "+ i.ImageId);
                                if (imgToDelete != null)
                                { 
                                    MyTool.DeleteImg(i.ImageUrl, "LibraryImgs");
                                    _db.LibraryImages.Remove(imgToDelete);
                                }
                                else
                                {
                                    _logger.LogError("imageLibraryToDelete is null");
                                }
                            }
                            string imgUrl = MyTool.UploadHinh(imgF, "LibraryImgs");
                            var libraryImg = new LibraryImage
                            {
                                MaLibrary = moto.MaLibrary,
                                ImageUrl = imgUrl,
                            };
                            _db.LibraryImages.Add(libraryImg);
                        }
                        #endregion

                        #region Set Version

                        var listVersionToRemove = new List<MotoVersion>();
                        var listImageToSave = new List<int>(); 
                        // đánh dấu các version cần xóa
                        foreach (var v in moto.MotoVersions)
                        {
                            listVersionToRemove.Add(v);
                        }
                        
                        // Thêm version mới vào
                        foreach (var version in model.MotoVersionsVM)
                        {
                            var v = new MotoVersion
                            {
                                MaXe = moto.MaXe,
                                MaVersion = MyTool.GenarateRandomKey(),
                                TenVersion = version.TenVersion,
                                GiaBanVersion = version.GiaBanVersion,
                            };
                            _db.MotoVersions.Add(v);

                            foreach (var color in version.VersionColorsVM)
                            {
                                var c = new VersionColor
                                {
                                    MaVersion = v.MaVersion,
                                    MaVersionColor = MyTool.GenarateRandomKey(),
                                    TenMau = color.TenMau,
                                };
                                _db.VersionColors.Add(c);

                                // Kiểm tra nếu không có ảnh mới được chọn
                                if (color.versionImageIF == null || color.versionImageIF.Count == 0)
                                {
                                    // Giữ nguyên các ảnh cũ
                                   // _logger.LogError("Có bằng này ảnh cũ: " + color.versionImageVM.Count);
                                    foreach (var img in color.versionImageVM)
                                    {  
                                        var i = _db.VersionImages.FirstOrDefault(i1 =>i1.ImageId == img.ImageId);
                                        if (i != null)
                                        {
                                           // _logger.LogError("ten anh: " + i.ImageUrl);
                                            i.MaVersionColor = c.MaVersionColor; 
                                            listImageToSave.Add(i.ImageId);     
                                        }
                                        else
                                        {
                                            _logger.LogError("Khong tim thay anh de thay ma color");
                                        }
                                    }
                                }
                                else
                                {
                                    // Thêm ảnh mới
                                    foreach (var file in color.versionImageIF)
                                    {
                                        string imgUrl = MyTool.UploadHinh(file, "AnhVersion");
                                        var i = new VersionImage
                                        {
                                            MaVersionColor = c.MaVersionColor,
                                            ImageUrl = imgUrl
                                        };
                                        _db.VersionImages.Add(i);
                                    }
                                }
                            }
                        }

                        // sau khi thêm xong thì xóa các cái đã đánh dấu
                        foreach (var v in moto.MotoVersions)
                        {
                            var versionChecker = listVersionToRemove.FirstOrDefault(v1=>v1.MaVersion == v.MaVersion);
                            if (versionChecker !=null)
                            {
                                foreach (var c in v.VersionColors)
                                {
                                    foreach (var i in c.VersionImages)
                                    {

                                        var imageToDelete = _db.VersionImages.FirstOrDefault(img => img.ImageId == i.ImageId);
                                        if (imageToDelete != null)
                                        {
                                            var imageCheck = listImageToSave.FirstOrDefault(img2 => img2 == imageToDelete.ImageId);
                                            if (imageCheck == 0)
                                            {
                                                _db.VersionImages.Remove(imageToDelete); // Xóa ảnh khỏi cơ sở dữ liệu
                                                MyTool.DeleteImg(i.ImageUrl, "AnhVersion");
                                            }
                                        }
                                        else
                                        {
                                            _logger.LogError("imageVersionToDelete is null");
                                        }
                                    }
                                    var colorToDelete = _db.VersionColors.FirstOrDefault(color => color.MaVersionColor == c.MaVersionColor);
                                    if (colorToDelete != null)
                                    {
                                        _db.VersionColors.Remove(colorToDelete); // Xóa màu khỏi cơ sở dữ liệu
                                    }
                                    else
                                    {
                                        _logger.LogError("colorVersionToDelete is null");
                                    }
                                }
                                var versionToDelete = _db.MotoVersions.FirstOrDefault(Version => Version.MaVersion == v.MaVersion);
                                if (versionToDelete != null)
                                {
                                    _db.MotoVersions.Remove(versionToDelete);
                                }
                                else
                                {
                                    _logger.LogError("motoVersionToDelete is null");
                                }
                            }
                        }
                        #endregion

                        try
                        {
                            var result = _db.SaveChanges();
                            if (result > 0)
                            {
                                _logger.LogError("Sua Xe Thanh Cong");
                                return RedirectToAction("ListMoto", "Moto");
                            }
                            else
                            {
                                _logger.LogError("Không sua được XE, không có hàng nào bị ảnh hưởng.");
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Lỗi khi sua XE vào cơ sở dữ liệu.");
                            throw; // Ném lại lỗi để dễ dàng kiểm tra khi debug
                        }
                    }
                }
                return View(model);
            }
            else
            {
                model.Brands = _db.Brands.ToList();
                model.MotoTypes = _db.MotoTypes.ToList();
                return View(model);
            }
            
        }
    }
}