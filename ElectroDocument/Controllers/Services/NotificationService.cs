using DocumentFormat.OpenXml.Office2010.Excel;
using ElectroDocument.Controllers.AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections;

namespace ElectroDocument.Controllers.Services
{
    public class NotificationService
    {
        private ElectroDocumentDevContext context;
        private IDistributedCache distributedCache;
        private DocsService docs;

        public NotificationService(ElectroDocumentDevContext context, IDistributedCache distributedCache, DocsService docs)
        {
            this.context = context;
            this.distributedCache = distributedCache;
            this.docs = docs;
        }

        public async Task<int> GetNotificationCount(long empId)
        {
            await context.Docs.LoadAsync();
            List<Doc> qdocs =  new List<Doc>(docs.GetFullDocsByUserIdNotifyExt(empId).AsQueryable());
            

            int simpleDocCount = qdocs.Count(doc => doc.EmployeeId == empId && !Convert.ToBoolean(doc.Notified));
            int responsibleDocCount = qdocs.Count(doc => doc.Responsible == empId && !Convert.ToBoolean(doc.ResponsibleNotified));

            return simpleDocCount + responsibleDocCount;
        }


        public async Task<IEnumerable<Doc>> GetUnseenDocuments(long empId)
        {
            await context.Docs.LoadAsync();
            List<Doc> qdocs = new List<Doc>(docs.GetFullDocsByUserIdNotifyExt(empId).AsQueryable());

            return qdocs.Where(doc => (doc.EmployeeId == empId || doc.Responsible == empId)
                &&
                (!Convert.ToBoolean(doc.Notified) || !Convert.ToBoolean(doc.ResponsibleNotified))
                );
        }

        public async Task NotifiedAll(long empId)
        {
            IEnumerable<Doc> docs = await GetUnseenDocuments(empId);
            foreach (Doc d in docs)
            {
                if (d.EmployeeId == empId)
                {
                    d.Notified = 1;
                }
                else if (d.Responsible == empId)
                {
                    d.ResponsibleNotified = 1;
                }
            }
            await context.SaveChangesAsync();
        }

        public async Task<bool> Notified(long empId, long docId)
        {
            await context.Docs.LoadAsync();
            List<Doc> qdocs = new List<Doc>(docs.GetFullDocsByUserIdNotifyExt(empId).AsQueryable());

            Doc? doc =  qdocs.Where(
                d => d.EmployeeId == empId || d.Responsible == empId).First();
            if (doc is not null)
            {
                if (doc.EmployeeId == empId)
                {
                    doc.Notified = 1;
                }
                else if(doc.Responsible == empId) 
                {
                    doc.ResponsibleNotified = 1;
                }
                await context.SaveChangesAsync();
                return true;
            }

            return false;

        }

    }
}
