using Azure.Storage.Blobs;
using Microsoft.Data.Sqlite;

namespace WebAPI.Infrastructure
{
    public class SqliteBackupHostedService : IHostedService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<SqliteBackupHostedService> logger;
        private Timer? timer;

        public SqliteBackupHostedService(IConfiguration configuration, ILogger<SqliteBackupHostedService> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("SqliteBackupHostedService is running");
            timer = new Timer(RunBackup, null, TimeSpan.FromMinutes(2), TimeSpan.FromHours(4));

            return Task.CompletedTask;
        }

        private void RunBackup(object? state)
        {
            try
            {
                RunBackupCore();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Exception during backup of sqlite");
            }
        }

        private void RunBackupCore()
        {
            logger.LogInformation("Running backup");
            var connectionString = configuration.GetConnectionString("Database");
            using (var source = new SqliteConnection(connectionString))
            {
                source.Open();
                using (var destination = new SqliteConnection("Data Source=backup.db;Mode=ReadWriteCreate;"))
                {
                    source.BackupDatabase(destination);
                }
                source.Close();
            }
            Task.Delay(TimeSpan.FromMinutes(30)).Wait(); // massive cringe because backup.db is locked by sqlite backup and the Microsoft.Data.Sqlite api doesnt offer a way to wait for finish
            var backupFilePath = Path.GetFileName("backup.db");
            if (!File.Exists(backupFilePath))
            {
                logger.LogError("failed to backup sqlite - backup.db file doesnt exist, ");
                return;
            }

            var blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BackupStorageAccount"));
            var container = blobServiceClient.GetBlobContainerClient("dbbackupcontainer");
            if (!container.Exists())
            {
                logger.LogError($"failed to upload sqlite backup - storage container {container.Name} not found in account {blobServiceClient.AccountName}");
                return;
            }
            var blobClient = container.GetBlobClient($"backup{DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm")}.db");
            blobClient.Upload(backupFilePath, true);

            logger.LogInformation("successfully uploaded backup");

            var existingBackups = container.GetBlobs().OrderBy(blob => blob.Properties.CreatedOn).ToList();
            if (existingBackups.Count > 14)
            {
                var blobName = existingBackups.First().Name;
                var deleteBlobClient = container.GetBlobClient(blobName);
                deleteBlobClient.Delete();
                logger.LogInformation("successfully deleted old backup" + blobName);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
