namespace Backend.Backup
{
    public class BackupManager
    {
        //public Settings? Settings { get; set; }
        public List<ABackup> BackupList { get; set; }
        /// <summary>
        /// Constructor of BackupManager.
        /// Initializes a new instance of the BackupManager class.
        /// </summary>
        public BackupManager()
        {
            BackupList = new List<ABackup>();
        }
        /// <summary>
        /// Updates daily logs for the backups.
        /// </summary>
        public void UpdateDailyLogs()
        {

        }
        /// <summary>
        /// Updates real-time logs for the backups.
        /// </summary>
        public void UpdateRealTimeLogs()
        {

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

            if (backupType == "full")
            {
                BackupList.Add(new BackupFull(name, sourceDirectory, targetDirectory));
            }
            else if (backupType == "differential")
            {
                BackupList.Add(new BackupDifferential(name, sourceDirectory, targetDirectory));
            }
            else
            {
                throw new ArgumentException("Type de sauvegarde non pris en charge");
            }
        }
        /// <summary>
        /// Converts a local path to its UNC equivalent.
        /// </summary>
        /// <param name="localPath">The local path to be converted.</param>
        /// <returns>The UNC path.</returns>
        static string ConvertToLocalUNC(string localPath)
        {
            try
            {
                string uncPath = Path.GetFullPath(localPath);
                return uncPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting path to UNC: {ex.Message}");
                return null;
            }
        }

    }
}
