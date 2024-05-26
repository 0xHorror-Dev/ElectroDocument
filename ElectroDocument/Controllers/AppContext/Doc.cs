using System;
using System.Collections.Generic;

namespace ElectroDocument.Controllers.AppContext;

public partial class Doc
{
    public long Id { get; set; }

    public long Number { get; set; }

    public sbyte? DocType { get; set; }

    public DateOnly Date { get; set; }

    public DateOnly? DateSecond { get; set; }

    public DateOnly? DateThird { get; set; }

    public long? EmployeeId { get; set; }

    public string? Reason { get; set; }

    public string? Desc { get; set; }

    public string? DescSecond { get; set; }

    public int? Sum { get; set; }

    public string? Title { get; set; }

    public short Notified { get; set; }

    public long? Responsible { get; set; }

    public short? ResponsibleNotified { get; set; }

    public virtual ICollection<DocumentVersion> DocumentVersionDocIdSrcNavigations { get; set; } = new List<DocumentVersion>();

    public virtual ICollection<DocumentVersion> DocumentVersionDocs { get; set; } = new List<DocumentVersion>();

    public virtual ICollection<DocumentVersion> DocumentVersionNewDocs { get; set; } = new List<DocumentVersion>();

    public virtual Employee? Employee { get; set; }

    public virtual Employee? ResponsibleNavigation { get; set; }
}
