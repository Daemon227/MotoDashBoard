using DashBoard_MotoManager.Datas;
using System.ComponentModel.DataAnnotations;

namespace DashBoard_MotoManager.Models
{
    public class EditMotoVM
    {
        public MotoVM MotoBike { get; set; } = new MotoVM();
        //[Required(ErrorMessage = "Vui Lòng Thêm Ảnh")]
        public IFormFile? AnhMoTaIF { get; set; }
        public List<BrandVM> Brands { get; set; } = new List<BrandVM>();
        public List<MotoTypeVM> MotoTypes { get; set; } = new List<MotoTypeVM>();
        public List<IFormFile> LibraryImageIF { get; set; } = new List<IFormFile>();
        public List<MotoVersionVM> MotoVersionsVM { get; set; } = new List<MotoVersionVM>();
    }
}
