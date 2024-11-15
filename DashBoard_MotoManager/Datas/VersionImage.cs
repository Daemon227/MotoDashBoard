using System;
using System.Collections.Generic;

namespace DashBoard_MotoManager.Datas;

public partial class VersionImage
{
    public int ImageId { get; set; }

    public string? MaVersionColor { get; set; }

    public string? ImageUrl { get; set; }

    public virtual VersionColor? MaVersionColorNavigation { get; set; }
}
