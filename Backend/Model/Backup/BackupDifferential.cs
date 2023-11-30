using System.Xml.Linq;
using System;

namespace Backend.Model.Backup
{
    /// <summary>
    /// BackupDifferential class represents a differential backup strategy 
    /// and inherits from the ABackup abstract class.
    /// </summary>
    public class BackupDifferential : ABackup
    {
        public BackupDifferential(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        {

        }
        public override void PerformBackup()
        {
            try
            {
                // Obtient la liste des fichiers dans le répertoire source
                string[] sourceFiles = Directory.GetFiles(SourceDirectory);

                // Parcours tous les fichiers du répertoire source
                foreach (string sourceFilePath in sourceFiles)
                {
                    string fileName = Path.GetFileName(sourceFilePath);
                    string targetFilePath = Path.Combine(TargetDirectory, fileName);

                    // Si le fichier n'existe pas dans le répertoire cible ou s'il a été modifié
                    if (!File.Exists(targetFilePath) || File.GetLastWriteTimeUtc(sourceFilePath) > File.GetLastWriteTimeUtc(targetFilePath))
                    {
                        File.Copy(sourceFilePath, targetFilePath, true); // true pour écraser les fichiers existants
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error copying modified or new files: {ex.Message}");
            }
        }
    }
}
