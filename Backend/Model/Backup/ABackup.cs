using Backend.Model.Backup;
using System;

namespace Backend
{

    /// <summary>
    /// Abstract class ABackup defines the logic for backups.
    /// </summary>
    public abstract class ABackup : IBackup
    {
        public string Name { get; set; }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public int TotalFiles { get; set; }
        public uint TotalSize { get; set; }
        public float FileTransferTime { get; set; }
        public DateTime StartTime { get; set; }
        public BackupState State { get; set; }
        public ABackup(string name, string sourceDirectory, string targetDirectory)
        {
            Name = name;
            SourceDirectory = sourceDirectory;
            TargetDirectory = targetDirectory;
            TotalFiles = 0;
            TotalSize = 0;
            FileTransferTime = 0.0f;
            StartTime = DateTime.Now;
            State = new BackupState();
        }
        /// <summary>
        /// Performs the backup operation. Must be implemented by subclasses.
        /// </summary>
        public virtual void PerformBackup()
        {
            
        }

        /// <summary>
        /// Scans files for backup.
        /// </summary>
        public void ScanFiles()
        {
            try
            {
                string[] files = Directory.GetFiles(SourceDirectory, "*", SearchOption.AllDirectories);

                TotalFiles = files.Length;
                long totalSize = 0;

                foreach (string filePath in files)
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    totalSize += fileInfo.Length;
                }

                TotalSize = (uint)totalSize;
                Console.WriteLine($"Scanned {TotalFiles} files. Total size: {totalSize} bytes.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error scanning files: {ex.Message}");
            }
        }
        /// <summary>
        /// Updates the progress of the backup operation.
        /// </summary>
        public void UpdateProgress()
        {
            
        }
    }
}
