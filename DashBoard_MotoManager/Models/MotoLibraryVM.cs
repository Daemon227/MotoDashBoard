using DashBoard_MotoManager.Datas;

namespace DashBoard_MotoManager.Models
{
    public class MotoLibraryVM
    {
        public string MaLibrary { get; set; } = null!;

        public virtual ICollection<LibraryImageVM> LibraryImages { get; set; } = new List<LibraryImageVM>();

        //public virtual ICollection<MotoVM> MotoBikes { get; set; } = new List<MotoVM>();
    }
}
