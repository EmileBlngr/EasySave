using System;
using System.ComponentModel;

namespace Backend.Backup
{

    /// <summary>
    /// Represents the interface for a backup operation.
    /// </summary>
    public interface IBackup : INotifyPropertyChanged
    {
        /// <summary>
        /// Updates the progress of the backup operation.
        /// </summary>
        public void UpdateProgress();

        /// <summary>
        /// Scans files as part of the backup operation.
        /// </summary>
        public void ScanFiles();

        /// <summary>
        /// Performs the backup operation.
        /// </summary>
        public void PerformBackup();
    }
}
