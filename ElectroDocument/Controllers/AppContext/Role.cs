using System;
using System.Collections.Generic;

namespace ElectroDocument.Controllers.AppContext;

public partial class Role
{
    public long Id { get; set; }

    public string Title { get; set; } = null!;

    public string AccessLevel { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
