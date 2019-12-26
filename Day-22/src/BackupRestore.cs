using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MarcusTurewicz.BackupRestore
{
    public static class BackupRestore
    {
        [FunctionName("Backup")]
        public static async Task<IActionResult> BackupRun(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var service = new BackupRestoreService();
            await service.BackupAsync(log);

            return new OkResult();
        }

        [FunctionName("Restore")]
        public static async Task<IActionResult> RestoreRun(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var service = new BackupRestoreService();
            await service.RestoreAsync();

            return new OkResult();
        }
    }
}
