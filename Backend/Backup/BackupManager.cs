using Backend.Settings;
namespace Backend.Backup
{
    public class BackupManager
    {
        public Backend.Settings.Settings Settings { get; set; }
        public List<ABackup> BackupList { get; set; }
        /// <summary>
        /// Constructor of BackupManager.
        /// Initializes a new instance of the BackupManager class.
        /// </summary>
        public BackupManager()
        {
            Settings = new Backend.Settings.Settings();
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
                string errorMessage = Settings.LanguageSettings.LanguageData["unsupported_backup_type"];
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
                string errorMessageUNC = Settings.LanguageSettings.LanguageData["converting_path_unc_error"];
                Console.WriteLine($"{errorMessageUNC} {ex.Message}");
                return null;
            }
        }

    }
}
