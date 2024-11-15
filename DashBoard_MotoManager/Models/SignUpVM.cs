using System.ComponentModel.DataAnnotations;

namespace DashBoard_MotoManager.Models
{
    public class SignUpVM
    {
        [Key]
        [MaxLength(5, ErrorMessage = "Username tối đa 25 ký tự")]
        public string UserId { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập tối đa 25 ký tự")]
        [MaxLength(25, ErrorMessage = "Username tối đa 25 ký tự")]
        public string Username { get; set; }

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Mật Khẩu tối đa 25 ký tự")]
        [MaxLength(20, ErrorMessage = "Password tối đa 25 ký tự")]
        public string Password { get; set; }
    }
}
