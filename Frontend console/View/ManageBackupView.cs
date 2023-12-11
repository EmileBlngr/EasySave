using Backend.Backup;
using Backend.Settings;

namespace Frontend_console.View
{
    public class ManageBackupView
    {
        public static void ManageBackup(ABackup backup , BackupManager backupManager)
        {
            Console.WriteLine(string.Format(backupManager.Settings.LanguageSettings.LanguageData["managing_backup"], backup.Name));
            Console.WriteLine(string.Format(backupManager.Settings.LanguageSettings.LanguageData["current_state"], backup.State.State));

            backup.ScanFiles();

            switch (backup.State.State)
            {
                case EnumState.NotStarted:
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["options"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["launch_backup"]);
       
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["back_option"]);
                    break;
                case EnumState.InProgress:
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["options"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["pause_backup"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["cancel_backup"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["back_option"]);
                    break;
                case EnumState.Paused:
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["options"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["cancel_backup"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["resume_backup"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["back_option"]);
                    break;
                case EnumState.Finished:
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["options"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["launch_backup"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["back_option"]);
                    break;
                case EnumState.Failed:
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["options"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["retry_backup"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["back_option"]);
                    break;
                case EnumState.Cancelled:
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["options"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["launch_backup"]);
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["back_option"]);
                    break;
                default:
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["invalid_state"]);
                    break;
            }

            Console.Write(backupManager.Settings.LanguageSettings.LanguageData["enter_your_choice"]);
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    backup.PerformBackup();
                    break;
                case "2":
                    //HandleOption2(backup);
                    break;
                case "3":
                    //HandleOption3(backup);
                    break;
                case "4":
                    //HandleOption2(backup);
                    break;
                case "5":
                    //HandleOption2(backup);
                    break;
                default:
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["invalid_choice"]);
                    break;
            }
        }
    }
}
