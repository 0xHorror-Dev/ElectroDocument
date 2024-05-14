using System;
using System.Collections.Generic;

namespace ElectroDocument.Controllers.AppContext;

public partial class Employee
{
    public long Id { get; set; }

    public long CredentialsId { get; set; }

    public long IndividualId { get; set; }

    public long RoleId { get; set; }

    public virtual EmployeeCredential Credentials { get; set; } = null!;

    public virtual ICollection<Doc> DocEmployees { get; set; } = new List<Doc>();

    public virtual ICollection<Doc> DocResponsibleNavigations { get; set; } = new List<Doc>();

    public virtual ICollection<DocumentVersion> DocumentVersions { get; set; } = new List<DocumentVersion>();

    public virtual Individual Individual { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
