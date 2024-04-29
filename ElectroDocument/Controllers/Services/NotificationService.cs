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
            return await context.Docs.CountAsync(doc => doc.EmployeeId == empId && !Convert.ToBoolean(doc.Notified));
        }


        public async Task<IEnumerable<Doc>> GetUnseenDocuments(long empId)
        {
            await context.Docs.LoadAsync();
            return context.Docs.Where(doc => doc.EmployeeId == empId && !Convert.ToBoolean(doc.Notified));
        }

        public async Task NotifiedAll(long empId)
        {
            IEnumerable<Doc> docs = await GetUnseenDocuments(empId);
            foreach (Doc d in docs)
            {
                d.Notified = 1;
            }
            await context.SaveChangesAsync();
        }

        public async Task<bool> Notified(long empId, long docId)
        {
            await context.Docs.LoadAsync();

            Doc? doc = await context.Docs.FindAsync(docId);
            if (doc is not null)
            {
                doc.Notified = 1;
                await context.SaveChangesAsync();
                return true;
            }

            return false;

        }

    }
}
