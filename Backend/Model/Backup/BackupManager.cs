namespace Backend.Model.Backup
{
    public class BackupManager
    {
        //public Settings? Settings { get; set; }
        public List<ABackup> BackupList { get; set; }

        public BackupManager()
        {
            BackupList = new List<ABackup>();
        }
        public void UpdateDailyLogs() 
        {
        
        }
        public void UpdateRealTimeLogs()
        {
            
        }
        public void AddBackup(string backupType, string name, string sourceDirectory, string targetDirectory)
        {
            sourceDirectory = ConvertToLocalUNC(sourceDirectory);
            targetDirectory = ConvertToLocalUNC(targetDirectory);
            Console.WriteLine(sourceDirectory);
            Console.WriteLine(targetDirectory);

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
                // Gérer d'autres types de sauvegarde si nécessaire.
                throw new ArgumentException("Type de sauvegarde non pris en charge");
            }
        }
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
