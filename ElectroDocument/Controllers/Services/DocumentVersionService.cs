using DocumentFormat.OpenXml.Office2010.Excel;
using ElectroDocument.Controllers.AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq;

namespace ElectroDocument.Controllers.Services
{
    public class DocumentVersionService
    {
        private IDistributedCache cache;
        private ElectroDocumentContext db;

        public DocumentVersionService(ElectroDocumentContext context, IDistributedCache distributedCache)
        {
            cache = distributedCache;
            db = context;
        }

        private bool DocQuery(DocumentVersion ver, long id)
        {
            return ver.NewDocId != null ? ver.NewDocId == id : ver.DocIdSrc == id;
        }

        public IEnumerable<DocumentVersion> GetDocumentVersion(long Id)
        {
            DocumentVersion currentVersion = db.DocumentVersions.Where(ver=> ver.NewDocId != null ? ver.NewDocId == Id : ver.DocIdSrc == Id).First();

            return db.DocumentVersions.Where(ver => ver.DocIdSrc == currentVersion.DocIdSrc);
        }

    }
}
