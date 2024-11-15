using DashBoard_MotoManager.Datas;

namespace DashBoard_MotoManager.Models
{
    public class VersionImageVM
    {
        public int ImageId { get; set; }

        public string? MaVersionColor { get; set; }

        public string? ImageUrl { get; set; }

        public virtual VersionColor? MaVersionColorNavigation { get; set; }
    }
}
