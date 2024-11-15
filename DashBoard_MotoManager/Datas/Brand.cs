using System;
using System.Collections.Generic;

namespace DashBoard_MotoManager.Datas;

public partial class Brand
{
    public string MaHangSanXuat { get; set; } = null!;

    public string? TenHangSanXuat { get; set; }

    public string? QuocGia { get; set; }
    public string? MoTaNgan {  get; set; }
    public virtual ICollection<MotoBike> MotoBikes { get; set; } = new List<MotoBike>();
}
