

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DashBoard_MotoManager.Models
{
    public class VersionColorVM
    {
        public string? MaVersionColor;

        [Required(ErrorMessage = "Tên màu không được để trống.")]
        [MaxLength(50, ErrorMessage = "Độ dài không được quán 50 ký tự")]
        public string TenMau {  get; set; }
        public string? MaVersion { get; set; }
        public List<VersionImageVM>? VersionImageVM { get; set; } 
        public List<IFormFile> versionImageIF { get; set; } = new List<IFormFile>();
    }
}
