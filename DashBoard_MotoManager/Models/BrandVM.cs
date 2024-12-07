using DashBoard_MotoManager.Datas;
using System.ComponentModel.DataAnnotations;

namespace DashBoard_MotoManager.Models
{
    public class BrandVM
    {
        public string MaHangSanXuat { get; set; } = null!;
        [Required(ErrorMessage = "Tên Hãng Sản Xuất Không Được Để Trống")]
        public string? TenHangSanXuat { get; set; }
        public string? QuocGia { get; set; }
        public string? MoTaNgan { get; set; }
        //public virtual ICollection<MotoBike> MotoBikes { get; set; } = new List<MotoBike>();
    }
}
