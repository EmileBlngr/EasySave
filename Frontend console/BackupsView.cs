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
                Console.WriteLine("No backups available.");
            }
            else
            {
                foreach (var backup in backupManager.BackupList)
                {
                    Console.WriteLine($"- {backup.Name} ({backup.GetType().Name}):");
                    Console.WriteLine($"  Source Directory: {backup.SourceDirectory}");
                    Console.WriteLine($"  Target Directory: {backup.TargetDirectory}");
                    Console.WriteLine();

                }
            }
        }
    }
}
