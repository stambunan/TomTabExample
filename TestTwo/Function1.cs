using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.Tabular;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting.Server;

namespace TestTwo
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                string workspaceConnection = "";
                string tenantId = "";
                string appId = "";
                string appSecret = "";

                string connectStringApp =
                    $"DataSource={workspaceConnection};User ID=app:{appId}@{tenantId};Password={appSecret};";

                // connect to the Power BI workspace referenced in connect string
                Server server = new Server();
                server.Connect(connectStringApp);

                // enumerate through datasets in workspace to display their names
                string databaseName = "";
                foreach (Database database in server.Databases)
                {
                    log.LogInformation(database.Name);
                    databaseName += database.Name;
                }


                string responseMessage = $"Databases: {databaseName}. This HTTP triggered function executed successfully.";

                return new OkObjectResult(responseMessage);
            }
            catch (Exception ex)
            {
                return new OkObjectResult($"Exception:{ex}");
            }
        }
    }
}
