using System;
using System.Collections.Generic;

namespace ElectroDocument.Controllers.AppContext;

public partial class History
{
    public long Id { get; set; }

    public DateOnly Date { get; set; }

    public string Desc { get; set; } = null!;

    public long? Document { get; set; }

    public long Owner { get; set; }

    public long Employee { get; set; }

    public virtual Doc? DocumentNavigation { get; set; }

    public virtual Employee EmployeeNavigation { get; set; } = null!;

    public virtual Employee OwnerNavigation { get; set; } = null!;
}
