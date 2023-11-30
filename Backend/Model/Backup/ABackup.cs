using Backend.Model.Backup;

namespace Backend
{

    /// <summary>
    /// Abstract class ABackup defines the logic for backups.
    /// </summary>
    public abstract class ABackup : IBackup
    {
        protected string Name { get; set; }
        protected string SourceDirectory { get; set; }
        protected string TargetDirectory { get; set; }
        protected uint TotalSize { get; set; }
        protected float FileTransferTime { get; set; }
        protected DateTime StartTime { get; set; }
        protected BackupState State { get; set; }
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
            
        }
        /// <summary>
        /// Updates the progress of the backup operation.
        /// </summary>
        public void UpdateProgress()
        {
            
        }
    }
}
