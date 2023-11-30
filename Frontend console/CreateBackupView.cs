using Backend.Model.Backup;

namespace Frontend_console
{
    public class CreateBackupView
    {
        public static void CreateBackup(BackupManager backupManager)
        {
            Console.WriteLine("Creating a new backup:");
            string backupType;
            do
            {
                Console.Write("Enter backup type (full/differential): ");
                backupType = Console.ReadLine().ToLower();

                if (backupType != "full" && backupType != "differential")
                {
                    Console.WriteLine("Invalid backup type. Please enter 'full' or 'differential'.");
                }

            } while (backupType != "full" && backupType != "differential");
            Console.Write("Enter backup name: ");
            string name = Console.ReadLine();
            Console.Write("Enter source directory: ");
            string sourceDirectory = Console.ReadLine();
            Console.Write("Enter target directory: ");
            string targetDirectory = Console.ReadLine();

            try
            {
                backupManager.AddBackup(backupType, name, sourceDirectory, targetDirectory);
                Console.WriteLine("Backup created successfully!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error creating backup: {ex.Message}");
            }
        }
    }
}
