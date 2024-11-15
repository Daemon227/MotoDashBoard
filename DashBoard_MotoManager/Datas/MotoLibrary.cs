using System;
using System.Collections.Generic;

namespace DashBoard_MotoManager.Datas;

public partial class MotoLibrary
{
    public string MaLibrary { get; set; } = null!;

    public virtual ICollection<LibraryImage> LibraryImages { get; set; } = new List<LibraryImage>();

    public virtual ICollection<MotoBike> MotoBikes { get; set; } = new List<MotoBike>();
}
