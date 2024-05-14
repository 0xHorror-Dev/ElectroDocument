using System;
using System.Collections.Generic;

namespace ElectroDocument.Controllers.AppContext;

public partial class DocDetail
{
    public long Id { get; set; }

    public string? Reason { get; set; }

    public string? Desc { get; set; }

    public string? DescSecond { get; set; }

    public virtual ICollection<Doc> Docs { get; set; } = new List<Doc>();
}
