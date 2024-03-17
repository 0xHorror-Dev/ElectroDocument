using System;
using System.Collections.Generic;

namespace ElectroDocument.Controllers.AppContext;

public partial class EmployeeCredential
{
    public long Id { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
