using DashBoard_MotoManager.Datas;
using System.ComponentModel.DataAnnotations;

namespace DashBoard_MotoManager.Models
{
    public class MotoVersionVM
    {
        public string? MaVersion { get; set; }

        [Required(ErrorMessage = "Tên Phiên Bản không được để trống.")]
        [MaxLength(50, ErrorMessage = "Độ dài không được quán 50 ký tự")]
        public string? TenVersion { get; set; }

        [Required(ErrorMessage = "Giá bán của phiên bản không được để trống.")]
        [MaxLength(50, ErrorMessage = "Độ dài không được quán 50 ký tự")]
        public string? GiaBanVersion { get; set; }

        public string? MaXe { get; set; }

        //public virtual MotoBike? MaXeNavigation { get; set; }

        public virtual List<VersionColorVM>? VersionColorVM { get; set; }
    }
}
