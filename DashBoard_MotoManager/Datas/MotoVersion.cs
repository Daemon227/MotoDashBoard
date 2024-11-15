using System;
using System.Collections.Generic;

namespace DashBoard_MotoManager.Datas;

public partial class MotoVersion
{
    public string MaVersion { get; set; } = null!;

    public string? TenVersion { get; set; }

    public string? GiaBanVersion { get; set; }

    public string? MaXe { get; set; }

    public virtual MotoBike? MaXeNavigation { get; set; }

    public virtual ICollection<VersionColor> VersionColors { get; set; } = new List<VersionColor>();
}
