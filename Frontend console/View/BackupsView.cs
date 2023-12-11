using Backend.Backup;
using Backend.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Frontend_console.View
{
    public class BackupsView
    {
        public static void ListBackups(BackupManager backupManager)
        {
            Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["list_of_backups"]);

            if (backupManager.BackupList.Count == 0)
            {
                Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["no_backups_available"]);
            }
            else
            {
                foreach (var backup in backupManager.BackupList)
                {
                    Console.WriteLine($"- {backup.Name} ({backup.GetType().Name}):");
                    Console.WriteLine(string.Format(backupManager.Settings.LanguageSettings.LanguageData["source_directory"], backup.SourceDirectory));
                    Console.WriteLine(string.Format(backupManager.Settings.LanguageSettings.LanguageData["target_directory"], backup.TargetDirectory));
                }
                Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["options"]);
                Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["go_back"]);
                Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["manage_backup"]);
                Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["launch_all_backups"]);

                string userInputOption = Console.ReadLine();

                switch (userInputOption)
                {
                    case "1":
                        // Do nothing, simply return to the main menu
                        break;
                    case "2":
                        Console.WriteLine(string.Format(backupManager.Settings.LanguageSettings.LanguageData["backup_number_prompt"], backupManager.BackupList.Count));
                        string userInputBackup = Console.ReadLine();
                        if (int.TryParse(userInputBackup, out int backupIndex) && backupIndex > 0 && backupIndex <= backupManager.BackupList.Count)
                        {
                            // Pass the selected backup and the instance of BackupManager
                            ManageBackupView.ManageBackup(backupManager.BackupList[backupIndex - 1], backupManager);
                        }
                        else
                        {
                            Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["invalid_backup_number"]);
                        }
                        break;
                    case "3":
                        //LaunchAllBackups(backupManager);
                        break;
                    default:
                        Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["invalid_choice"]);
                        break;
                }
            }
        }
    }
}
