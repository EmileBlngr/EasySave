//Frontend version console
using Backend.Backup;
using Backend.Settings;
using Frontend_console;
using Frontend_console.View;

BackupManager backupManager;
backupManager = new BackupManager();
bool run = true;

//var language = new Language(EnumLanguages.English);
//language.ShowFirstValue();

//language = new Language(EnumLanguages.Français);
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
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1. Create a backup");
        Console.WriteLine("2. Go to settings");
        Console.WriteLine("3. View existing backups");
        Console.WriteLine("4. View logs");
        Console.WriteLine("5. Exit");

        string userInput = Console.ReadLine();

        switch (userInput)
        {
            case "1":
                Console.WriteLine("Creating a backup...");
                CreateBackupView.CreateBackup(backupManager);
                break;
            case "2":
                Console.WriteLine("Going to settings...");
                break;
            case "3":
                Console.WriteLine("Viewing existing backups...");
                BackupsView.ListBackups(backupManager);
                break;
            case "4":
                Console.WriteLine("Viewing logs...");
                break;
            case "5":
                Console.WriteLine("Exiting...");
                run = false;
                break;
            default:
                Console.WriteLine("Invalid choice. Please enter a valid option.");
                break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error : {ex.Message}");
        run = false;
    }
}