using ElectroDocument.Controllers.AppContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace ElectroDocument.Controllers.Services
{
    public class NotificationService
    {
        private ElectroDocumentContext context;
        private IDistributedCache distributedCache;

        public NotificationService(ElectroDocumentContext context, IDistributedCache distributedCache)
        {
            this.context = context;
            this.distributedCache = distributedCache;
        }

        public async Task<int> GetNotificationCount(long empId)
        {
            await context.Docs.LoadAsync();
            int simpleDocCount = await context.Docs.CountAsync(doc => doc.EmployeeId == empId && !Convert.ToBoolean(doc.Notified));
            int responsibleDocCount = await context.Docs.CountAsync(doc => doc.Responsible == empId && !Convert.ToBoolean(doc.ResponsibleNotified));

            return simpleDocCount + responsibleDocCount;
        }


        public async Task<IEnumerable<Doc>> GetUnseenDocuments(long empId)
        {
            await context.Docs.LoadAsync();
            return context.Docs.Where(doc => (doc.EmployeeId == empId || doc.Responsible == empId)
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

            Doc? doc = await context.Docs.Where(
                d => d.EmployeeId == empId || d.Responsible == empId).FirstAsync();
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
