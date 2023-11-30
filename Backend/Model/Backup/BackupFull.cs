namespace Backend.Model.Backup
{
    /// <summary>
    /// BackupFull class represents a full backup strategy 
    /// and inherits from the ABackup abstract class.
    /// </summary>
    public class BackupFull : ABackup
    {
        public BackupFull(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        { }
        public override void PerformBackup()
        {
            try
            {
                // Obtient la liste des fichiers dans le répertoire source
                string[] files = Directory.GetFiles(SourceDirectory);

                // Copie chaque fichier vers le répertoire cible
                foreach (string filePath in files)
                {
                    string fileName = Path.GetFileName(filePath);
                    string destinationPath = Path.Combine(TargetDirectory, fileName);
                    File.Copy(filePath, destinationPath, true); // true pour écraser les fichiers existants
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying files: {ex.Message}");
            }
        }
    }
}
