using Backend.Backup;

namespace Frontend_console.View
{
    public class ManageBackupView
    {
        public static void ManageBackup(ABackup backup)
        {
            Console.WriteLine($"Managing backup: {backup.Name}");
            Console.WriteLine($"Currently: {backup.State.State}");
            backup.ScanFiles();

            switch (backup.State.State)
            {
                case EnumState.NotStarted:
                    Console.WriteLine("Options:");
                    Console.WriteLine("1. Launch Backup");
                    Console.WriteLine("5. Back");
                    break;
                case EnumState.InProgress:
                    Console.WriteLine("Options:");
                    Console.WriteLine("2. Pause Backup");
                    Console.WriteLine("3. Cancel Backup");
                    Console.WriteLine("5. Back");
                    break;
                case EnumState.Paused:
                    Console.WriteLine("Options:");
                    Console.WriteLine("3. Cancel Backup");
                    Console.WriteLine("4. Resume Backup");
                    Console.WriteLine("5. Back");
                    break;
                case EnumState.Finished:
                    Console.WriteLine("Options:");
                    Console.WriteLine("1. Launch Backup");
                    Console.WriteLine("5. Back");
                    break;
                case EnumState.Failed:
                    Console.WriteLine("Options:");
                    Console.WriteLine("1. Retry Backup");
                    Console.WriteLine("5. Back");
                    break;
                case EnumState.Cancelled:
                    Console.WriteLine("Options:");
                    Console.WriteLine("1. Launch Backup");
                    Console.WriteLine("5. Back");
                    break;
                default:
                    Console.WriteLine("Invalid state.");
                    break;
            }

            Console.Write("Enter your choice: ");
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
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }
    }
}
