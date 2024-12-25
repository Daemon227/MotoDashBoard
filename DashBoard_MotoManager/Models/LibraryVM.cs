using DashBoard_MotoManager.Datas;

namespace DashBoard_MotoManager.Models
{
    public class LibraryVM 
    {
        public string MaLibrary { get; set; } = null!;

        public virtual ICollection<LibraryImageVM> LibraryImageVM { get; set; } = new List<LibraryImageVM>();

        //public virtual ICollection<MotoVM> MotoBikes { get; set; } = new List<MotoVM>();
    }
}
