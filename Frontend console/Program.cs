//Frontend version console
using Backend.Model.Backup;
using Backend.Model.Settings;

BackupManager backupManager;
backupManager = new BackupManager();

//backupManager.AddBackup("full", "backup1", "C:\\backupSource", "C:\\backupTarget");
//backupManager.BackupList[0].ScanFiles();
//backupManager.BackupList[0].PerformBackup();

var language = new Language(EnumLanguages.English);
language.ShowFirstValue(); // Affiche "New backup" pour l'anglais

language = new Language(EnumLanguages.Français);
language.ShowFirstValue(); // Affiche "Nouvelle sauvegarde" pour le français



//Console.WriteLine(backupManager.BackupList[0].Name);