using System.ComponentModel.DataAnnotations;

namespace DashBoard_MotoManager.Models
{
    public class SignUpVM
    {
        [Key]
        public string? UserId { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [MaxLength(25, ErrorMessage = "Username tối đa 25 ký tự")]
        
        public string Username { get; set; }

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Mật Khẩu không được để trống")]
        [MaxLength(20, ErrorMessage = "Password tối đa 25 ký tự")]
        public string Password { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }
    }
}
