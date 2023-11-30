using Backend.Model.Backup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend_console
{
    public class BackupsView
    {
        public static void ListBackups(BackupManager backupManager)
        {
            Console.WriteLine("List of Backups:");

            if (backupManager.BackupList.Count == 0)
            {
                Console.WriteLine("No backups available.\n");
            }
            else
            {
                foreach (var backup in backupManager.BackupList)
                {
                    Console.WriteLine($"- {backup.Name} ({backup.GetType().Name}):");
                    Console.WriteLine($"  Source Directory: {backup.SourceDirectory}");
                    Console.WriteLine($"  Target Directory: {backup.TargetDirectory}\n");
                }
                Console.WriteLine("Options:");
                Console.WriteLine("1. Go Back");
                Console.WriteLine("2. Manage a backup");
                Console.WriteLine("3. Launch all backups");

                string userInputOption = Console.ReadLine();

                switch (userInputOption)
                {
                    case "1":
                        // Do nothing, simply return to the main menu
                        break;
                    case "2":
                        Console.WriteLine($"Backup number (between 1-{backupManager.BackupList.Count}): ");
                        string userInputBackup = Console.ReadLine();
                        if (int.TryParse(userInputBackup, out int backupIndex) && backupIndex > 0 && backupIndex <= backupManager.BackupList.Count)
                        {
                            ManageBackupView.ManageBackup(backupManager.BackupList[backupIndex - 1]);
                        }
                        else
                        {
                            Console.WriteLine("Invalid backup number.");
                        }
                        break;
                    case "3":
                        //LaunchAllBackups(backupManager);
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
}
