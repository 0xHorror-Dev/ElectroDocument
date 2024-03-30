using System;
using System.Collections.Generic;

namespace ElectroDocument.Controllers.AppContext;

public partial class Doc
{
    public long Id { get; set; }

    public sbyte? Type { get; set; }

    public DateOnly? FirstDay { get; set; }

    public DateOnly? SecondDay { get; set; }

    public string? DescFirst { get; set; }

    public string? DescSecond { get; set; }

    public virtual ICollection<History> Histories { get; set; } = new List<History>();
}
