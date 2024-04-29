using ElectroDocument.Controllers.AppContext;
using ElectroDocument.Controllers.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.IdentityModel.Abstractions;
using System.Security.Claims;

namespace ElectroDocument.Controllers
{
    public record DefaultNotificationRequest(long EmpId);
    public record NotificationUpdateStatusRequest(long EmpId, long DocId);

    [Authorize(Policy = "AdminOrUser")]
    public class NotificationController : Controller
    {
        private NotificationService notificationService;
        private DocsService documentService;

        public NotificationController(NotificationService notificationService, DocsService documentService)
        {
            this.notificationService = notificationService;
            this.documentService = documentService;

        }

        public async Task<ActionResult> Index()
        {
            var userClaims = User.Claims;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var docsCount = await notificationService.GetNotificationCount(Convert.ToInt64(userId));

            if (docsCount <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new
            {
                docs = await notificationService.GetUnseenDocuments(Convert.ToInt64(userId))
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IResult> GetNotificationCount([FromBody] DefaultNotificationRequest req)
        {
            int count = await notificationService.GetNotificationCount(req.EmpId);

            var response = new
            {
                count = count
            };

            return Results.Json(response);
        }

        [HttpPost]
        public async Task<IResult> AcceptAllNotifications([FromBody] DefaultNotificationRequest req)
        {
            await notificationService.NotifiedAll(req.EmpId);
            return Results.Ok(null);
        }


        [HttpPost]
        public async Task<IEnumerable<Doc>> GetUnseenDocs([FromBody] DefaultNotificationRequest req)
        {
            return await notificationService.GetUnseenDocuments(req.EmpId);
        }

        [HttpPost]
        public async Task<IResult> SetNotification([FromBody] NotificationUpdateStatusRequest req)
        {
            return Results.Ok(await notificationService.Notified(req.EmpId, req.DocId));
        }

    }
}
