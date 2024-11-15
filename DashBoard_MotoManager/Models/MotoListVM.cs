using DashBoard_MotoManager.Datas;

namespace DashBoard_MotoManager.Models
{
    public class MotoListVM
    {
        public string MaXe { get; set; } 

        public string? TenXe { get; set; }

        public string? MaLoai { get; set; }

        public string? MaHangSanXuat { get; set; }

        public string? AnhMoTaUrl { get; set; }

        public string? GiaBanMoTa { get; set; }
  
        public virtual Brand? MaHangSanXuatNavigation { get; set; }

        public virtual MotoType? MaLoaiNavigation { get; set; }

    }
}
