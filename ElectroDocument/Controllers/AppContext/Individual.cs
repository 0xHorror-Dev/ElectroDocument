using System;
using System.Collections.Generic;

namespace ElectroDocument.Controllers.AppContext;

public partial class Individual
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Patronymic { get; set; } = null!;

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
