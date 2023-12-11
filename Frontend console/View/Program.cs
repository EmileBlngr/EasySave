//Frontend version console
using Backend.Backup;
using Backend.Settings;
using Frontend_console;
using Frontend_console.View;

BackupManager backupManager;
backupManager = new BackupManager();
bool run = true;

//var language = new Language();
//language.ShowFirstValue();

//var language = new Language();
//language.ShowFirstValue();

//bool userWantsJson = false;
//bool userWantsTxt = true;

//Logs.DemonstrateLogging(userWantsJson, userWantsTxt);

//backupManager.AddBackup("differential", "backup1", "C:\\backupSource", "C:\\backupTarget");
//backupManager.BackupList[0].ScanFiles();
//backupManager.BackupList[0].PerformBackup();


while (run)
{
    try
    {

        Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["choose_options"]);
        Console.WriteLine("1. " + backupManager.Settings.LanguageSettings.LanguageData["new_backup_access"]);
        Console.WriteLine("2. " + backupManager.Settings.LanguageSettings.LanguageData["settings_access"]);
        Console.WriteLine("3. " + backupManager.Settings.LanguageSettings.LanguageData["my_backups_title"]);
        Console.WriteLine("4. " + backupManager.Settings.LanguageSettings.LanguageData["logs_access"]);
        Console.WriteLine("5. " + backupManager.Settings.LanguageSettings.LanguageData["exit"]);

        string userInput = Console.ReadLine();

        switch (userInput)
        {
            case "1":
                Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["creating_backup"]);
                CreateBackupView.CreateBackup(backupManager);
                break;
            case "2":
                Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["accessing_settings"]);
                SettingsView.AccessSettings(backupManager);
                break;
            case "3":
                Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["accessing_backups"]);
                BackupsView.ListBackups(backupManager);
                break;
            case "4":
                Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["accessing_logs"]);
                break;
            case "5":
                Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["exiting"]);
                run = false;
                break;
            default:
                Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["invalid_choice"]);
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error : {ex.Message}");
        run = false;
    }
}