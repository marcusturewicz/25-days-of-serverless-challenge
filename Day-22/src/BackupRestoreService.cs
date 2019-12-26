using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace MarcusTurewicz.BackupRestore
{
    public class BackupRestoreService
    {
        private readonly SecretClient _secretClient;
        private readonly BlobServiceClient _blobClient;

        public BackupRestoreService()
        {
            var keyVaultName = Environment.GetEnvironmentVariable("KeyVaultName");
            var kvUri = $"https://{keyVaultName}.vault.azure.net";
            _secretClient = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            var connectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
            _blobClient = new BlobServiceClient(connectionString);
            try
            {
                _blobClient.CreateBlobContainer("backup");
            }
            catch (Exception e)
            {
                // Container already exists
            }
        }

        public async Task BackupAsync(ILogger log)
        {
            var secrets = new List<Secret>();
            await foreach (var prop in _secretClient.GetPropertiesOfSecretsAsync())
            {
                var kvSecret = await _secretClient.GetSecretAsync(prop.Name);
                secrets.Add(new Secret { Key = prop.Name, Value = kvSecret.Value.Value });
            }

            var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(secrets));
            var filename = $"secretsbackup-{DateTime.UtcNow.ToString("o")}";
            var container = _blobClient.GetBlobContainerClient("backup");
            using (var stream = new MemoryStream(bytes))
            {
                await container.UploadBlobAsync(filename, stream);
            }
        }

        public async Task RestoreAsync()
        {
            var container = _blobClient.GetBlobContainerClient("backup");
            var props = await container.GetPropertiesAsync();
            var files = new List<string>();
            await foreach (var blobItem in container.GetBlobsAsync())
            {
                files.Add(blobItem.Name);
            }

            var latest = files.OrderByDescending(x => x).First();

            var blobInfo = await container.GetBlobClient(latest).DownloadAsync();
            var secrets = await JsonSerializer.DeserializeAsync<List<Secret>>(blobInfo.Value.Content);

            foreach (var secret in secrets)
            {
                await _secretClient.SetSecretAsync(secret.Key, secret.Value);
            }
        }
    }
}