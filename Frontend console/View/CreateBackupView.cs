using Backend.Backup;

namespace Frontend_console.View
{
    public class CreateBackupView
    {
        public static void CreateBackup(BackupManager backupManager)
        {
            Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["creating_new_backup"]);
            string backupNumber;
            string backupType;
            do
            {
                Console.WriteLine($"{backupManager.Settings.LanguageSettings.LanguageData["enter_backup_type"]}");
                Console.WriteLine($"1. {backupManager.Settings.LanguageSettings.LanguageData["backup_full"]}");
                Console.WriteLine($"2. {backupManager.Settings.LanguageSettings.LanguageData["backup_differential"]}");
                backupNumber = Console.ReadLine();

                if (backupNumber != "1" && backupNumber != "2")
                {
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["invalid_backup_type_prompt"]);
                }
            } while (backupNumber != "1" && backupNumber != "2");
            Console.Write(backupManager.Settings.LanguageSettings.LanguageData["enter_backup_name"]);
            string name = Console.ReadLine();
            Console.Write(backupManager.Settings.LanguageSettings.LanguageData["enter_source_directory"]);
            string sourceDirectory = Console.ReadLine();
            Console.Write(backupManager.Settings.LanguageSettings.LanguageData["enter_target_directory"]);
            string targetDirectory = Console.ReadLine();

            try
            {
                backupManager.AddBackup(backupNumber, name, sourceDirectory, targetDirectory);
                Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["backup_created_successfully"]);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(string.Format(backupManager.Settings.LanguageSettings.LanguageData["error_creating_backup"], ex.Message));

            }
        }
    }
}
