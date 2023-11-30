//Frontend version console
using Backend.Model.Backup;

BackupManager backupManager;
backupManager = new BackupManager();

backupManager.AddBackup("differential", "backup1", "C:\\backupSource", "C:\\backupTarget");
backupManager.BackupList[0].ScanFiles();
backupManager.BackupList[0].PerformBackup();

//Console.WriteLine(backupManager.BackupList[0].Name);