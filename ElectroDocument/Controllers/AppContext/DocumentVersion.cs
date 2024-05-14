using System;
using System.Collections.Generic;

namespace ElectroDocument.Controllers.AppContext;

public partial class DocumentVersion
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public long DocId { get; set; }

    public long? NewDocId { get; set; }

    public long DocIdSrc { get; set; }

    public long EditorId { get; set; }

    public virtual Doc Doc { get; set; } = null!;

    public virtual Doc DocIdSrcNavigation { get; set; } = null!;

    public virtual Employee Editor { get; set; } = null!;

    public virtual Doc? NewDoc { get; set; }
}
