using DashBoard_MotoManager.Datas;
using System.ComponentModel.DataAnnotations;

namespace DashBoard_MotoManager.Models
{
    public class EditMotoVM
    {
        public MotoBike MotoBike { get; set; } = new MotoBike();
        //[Required(ErrorMessage = "Vui Lòng Thêm Ảnh")]
        public IFormFile? AnhMoTaIF { get; set; }
        public List<Brand> Brands { get; set; } = new List<Brand>();
        public List<MotoType> MotoTypes { get; set; } = new List<MotoType>();
        public List<IFormFile> LibraryImageIF { get; set; } = new List<IFormFile>();
        public List<MotoVersionVM> MotoVersionsVM { get; set; } = new List<MotoVersionVM>();
    }
}
