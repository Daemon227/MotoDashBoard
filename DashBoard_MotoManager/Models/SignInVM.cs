using System.ComponentModel.DataAnnotations;

namespace DashBoard_MotoManager.Models
{
    public class SignInVM
    {

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập tối đa 25 ký tự")]
        [MaxLength(25, ErrorMessage = "Username tối đa 25 ký tự")]
        public string Username { get; set; }

        [Display(Name = "Mật Khẩu")]
        [Required(ErrorMessage = "Mật Khẩu tối đa 25 ký tự")]
        [DataType(DataType.Password)]
        [MaxLength(20, ErrorMessage = "Password tối đa 25 ký tự")]


        public string Password { get; set; }
    }
}
