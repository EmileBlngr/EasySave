using Backend.Model.Backup;
using System;
using Timer = System.Timers.Timer;
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
        public event EventHandler ProgressUpdated;
        public Timer ProgressDisplayTimer { get; set; }

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
            //Set the progress display timer
            ProgressDisplayTimer = new Timer(500); // 0.5 seconds
            ProgressDisplayTimer.Elapsed += ProgressDisplayTimerElapsed;
            ProgressDisplayTimer.AutoReset = true;
            ProgressDisplayTimer.Enabled = false;
            ProgressUpdated += DisplayProgress;
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
            State.Progress = 1.0f - (float)State.RemainingSize / TotalSize;
        }
        public void ProgressDisplayTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            // Timer has started, show progress
            DisplayProgress(sender, e);
        }

        public void DisplayProgress(object sender, EventArgs e)
        {
            // Display progress here
            Console.WriteLine($"Progression : {State.Progress * 100}% | Fichiers restants : {State.RemainingFiles} | " +
                $"Taille restante : {State.RemainingSize} octets | Fichier source actuel : {State.CurrentFileSource} | " +
                $"Fichier destination actuel : {State.CurrentFileTarget}");

        }
        protected virtual void OnProgressUpdated()
        {
            ProgressUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
