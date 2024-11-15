using DashBoard_MotoManager.Datas;

namespace DashBoard_MotoManager.Models
{
    public class MotoDetailVM
    {
        public string MaXe { get; set; } = null!;

        public string? TenXe { get; set; }

        public string? MaLoai { get; set; }

        public string? MaHangSanXuat { get; set; }

        public string? AnhMoTaUrl { get; set; }

        public string? GiaBanMoTa { get; set; }

        public string? TrongLuong { get; set; }

        public string? KichThuoc { get; set; }

        public string? KhoangCachTrucBanhXe { get; set; }

        public string? DoCaoYen { get; set; }

        public string? DoCaoGamXe { get; set; }

        public string? DungTichBinhXang { get; set; }

        public string? KichCoLop { get; set; }

        public string? PhuocTruoc { get; set; }

        public string? PhuocSau { get; set; }

        public string? LoaiDongCo { get; set; }

        public string? CongSuatToiDa { get; set; }

        public string? MucTieuThuNhienLieu { get; set; }

        public string? HeThongKhoiDong { get; set; }

        public string? MomentCucDai { get; set; }

        public string? DungTichXyLanh { get; set; }

        public string? DuongKinhHanhTrinhPittong { get; set; }

        public string? TySoNen { get; set; }

        public string? TinhNangNoiBat { get; set; }

        public string? ThietKe { get; set; }

        public string? TienIch { get; set; }

        public string? MaLibrary { get; set; }

        public virtual Brand? MaHangSanXuatNavigation { get; set; }

        public virtual MotoLibrary? MaLibraryNavigation { get; set; }

        public virtual MotoType? MaLoaiNavigation { get; set; }

        public virtual ICollection<MotoVersion> MotoVersions { get; set; } = new List<MotoVersion>();
    }
}
