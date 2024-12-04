using DashBoard_MotoManager.Datas;
using System.ComponentModel.DataAnnotations;

namespace DashBoard_MotoManager.Models
{
    public class MotoTypeVM
    {
        public string? MaLoai { get; set; }

        [Required(ErrorMessage = "Tên Loại Xe Không Được Để Trống")]
        public string? TenLoai { get; set; }

        public string? DoiTuongSuDung { get; set; }

        public string? MoTaNgan { get; set; }

        //public virtual ICollection<MotoBike> MotoBikes { get; set; } = new List<MotoBike>();
    }
}
