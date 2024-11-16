using DashBoard_MotoManager.Datas;

namespace DashBoard_MotoManager.Models
{
    public class MotoLibraryVM
    {
        public string MaLibrary { get; set; } = null!;

        public virtual ICollection<LibraryImage> LibraryImages { get; set; } = new List<LibraryImage>();

        public virtual ICollection<MotoDetailVM> MotoBikes { get; set; } = new List<MotoDetailVM>();
    }
}
