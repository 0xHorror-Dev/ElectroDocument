using System;
using System.Collections.Generic;

namespace ElectroDocument.Controllers.AppContext;

public partial class Employee
{
    public long Id { get; set; }

    public long CredentialsId { get; set; }

    public long IndividualId { get; set; }

    public long RoleId { get; set; }

    public string? ImageUrl { get; set; }

    public virtual EmployeeCredential Credentials { get; set; } = null!;

    public virtual ICollection<History> HistoryEmployeeNavigations { get; set; } = new List<History>();

    public virtual ICollection<History> HistoryOwnerNavigations { get; set; } = new List<History>();

    public virtual Individual Individual { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
