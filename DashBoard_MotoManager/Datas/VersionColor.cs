using System;
using System.Collections.Generic;

namespace DashBoard_MotoManager.Datas;

public partial class VersionColor
{
    public string MaVersionColor { get; set; } = null!;

    public string? TenMau { get; set; }

    public string? MaVersion { get; set; }

    public virtual MotoVersion? MaVersionNavigation { get; set; }

    public virtual ICollection<VersionImage> VersionImages { get; set; } = new List<VersionImage>();
}
