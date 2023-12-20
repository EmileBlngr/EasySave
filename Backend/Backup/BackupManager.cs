using System.Diagnostics;

namespace Backend.Backup
{
    /// <summary>
    /// Manages backup operations and provides functionality to add, convert paths, and perform backups.
    /// </summary>
    public class BackupManager
    {
        public Backend.Settings.Settings settings;
        public List<ABackup> BackupList { get; set; }
        /// <summary>
        /// Constructor of BackupManager.
        /// Initializes a new instance of the BackupManager class.
        /// </summary>
        public BackupManager()
        {
            settings = Settings.Settings.GetInstance();
            BackupList = new List<ABackup>();
        }
        /// <summary>
        /// Adds a new backup to the BackupManager, in BackupList.
        /// </summary>
        /// <param name="backupType"></param>
        /// <param name="name"></param>
        /// <param name="sourceDirectory"></param>
        /// <param name="targetDirectory"></param>
        /// <exception cref="ArgumentException"></exception>
        public void AddBackup(string backupType, string name, string sourceDirectory, string targetDirectory)
        {
            sourceDirectory = ConvertToLocalUNC(sourceDirectory);
            targetDirectory = ConvertToLocalUNC(targetDirectory);

            if (backupType == "1")
            {

                BackupList.Add(new BackupFull(name, sourceDirectory, targetDirectory));


            }
            else if (backupType == "2")
            {
                BackupList.Add(new BackupDifferential(name, sourceDirectory, targetDirectory));

            }
            else
            {
                string errorMessage = settings.LanguageSettings.LanguageData["unsupported_backup_type"];
                throw new ArgumentException(errorMessage);
            }
        }
        /// <summary>
        /// Converts a local path to its UNC equivalent.
        /// </summary>
        /// <param name="localPath">The local path to be converted.</param>
        /// <returns>The UNC path.</returns>
        public string ConvertToLocalUNC(string localPath)
        {
            try
            {
                string uncPath = Path.GetFullPath(localPath);
                return uncPath;
            }
            catch (Exception ex)
            {
                string errorMessageUNC = settings.LanguageSettings.LanguageData["converting_path_unc_error"];
                Console.WriteLine($"{errorMessageUNC} {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Performs all backup operations asynchronously, running each backup in a separate task.
        /// </summary>
        public void PerformAllBackups()
        {
            List<Task> backupTasks = new List<Task>();

            foreach (ABackup backup in BackupList)
            {
                Task backupTask = Task.Run(() => backup.PerformBackup());
                backupTasks.Add(backupTask);
            }
            Task.WaitAll(backupTasks.ToArray());
        }

        /// <summary>
        /// Checks if the business software associated with the backups is currently running.
        /// </summary>
        /// <returns>True if the business software is running; otherwise, false.</returns>
        public static bool IsBusinessSoftwareRunning()
        {
            string businessSoftware = Settings.Settings.GetInstance().GetBusinessSoftware();         
            if (string.IsNullOrEmpty(businessSoftware))
            {
                
                return false;
            }

            string processName = Path.GetFileNameWithoutExtension(businessSoftware);
            var runningProcesses = Process.GetProcessesByName(processName);
            return runningProcesses.Length > 0;
        }
    }
}
