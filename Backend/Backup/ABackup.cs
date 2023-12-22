using System;
using System.ComponentModel;
using System.IO;
using System.Timers;

namespace Backend.Backup
{

    /// <summary>
    /// Abstract class ABackup defines the logic for backups.
    /// </summary>
    public abstract class ABackup : IBackup, INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string SourceDirectory { get; set; }
        public string TargetDirectory { get; set; }
        public int TotalFiles { get; set; }
        public uint TotalSize { get; set; }
        public float FileTransferTime { get; set; }
        public float EncryptTime { get; set; }
        public DateTime StartTime { get; set; }
        public BackupState State { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler ProgressUpdated;
        public System.Timers.Timer ProgressDisplayTimer { get; set; }

        /// <summary>
        /// Constructor method of ABackup, initialize all the attributes and set up the timer
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sourceDirectory"></param>
        /// <param name="targetDirectory"></param>
        public ABackup(string name, string sourceDirectory, string targetDirectory)
        {
            Name = name;
            SourceDirectory = sourceDirectory;
            TargetDirectory = targetDirectory;
            ScanFiles();
            FileTransferTime = 0.0f;
            EncryptTime = 0.0f;
            StartTime = DateTime.Now;
            State = new BackupState();

            ProgressDisplayTimer = new System.Timers.Timer(100);
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
        /// Scans files for backup. Updates the total files number and total size
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
                Console.WriteLine(string.Format(Settings.Settings.GetInstance().LanguageSettings.LanguageData["error_scanning_files"], ex.Message));

            }
        }


        /// <summary>
        /// Raises the event indicating that the progress has been updated.
        /// This method is called when the progress of the backup operation is updated.
        /// </summary>
        protected virtual void OnProgressUpdated()
        {
            ProgressUpdated?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// Updates the progress of the backup operation.
        /// </summary>
        public void UpdateProgress()
        {
            // Calculate progress as a fraction of work done
            float progress = 1.0f - (float)State.RemainingSize / TotalSize;

            // Ensure progress does not exceed 100%
            State.Progress = Math.Min(progress, 1.0f);

            OnProgressUpdated(); // Raise the ProgressUpdated event
        }

        /// <summary>
        /// ProgressDisplayTimerElapsed method shows the progress of the backups, once the timer reaches the end of his interval.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ProgressDisplayTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            DisplayProgress(sender, e);
        }
        /// <summary>
        /// Displays the progress here, the remaining size and files, and the file being saved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DisplayProgress(object sender, EventArgs e)
        {
            Console.WriteLine(string.Format(Settings.Settings.GetInstance().LanguageSettings.LanguageData["backup_progress"],
                State.Progress * 100, State.RemainingFiles, State.RemainingSize, State.CurrentFileSource, State.CurrentFileTarget));


        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private float _progress;
        public float Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged(nameof(Progress));
                }
            }
        }

        /// <summary>
        /// Cancels the ongoing backup operation, setting the state to 'Cancelled'.
        /// </summary>
        public void CancelBackup()
        {
            if (State.State != EnumState.NotStarted && State.State != EnumState.Finished)
            {
                State.State = EnumState.Cancelled;
            }
        }

        /// <summary>
        /// Pauses the ongoing backup operation, setting the state to 'Paused'.
        /// </summary>
        public void PauseBackup()
        {
            if (State.State != EnumState.NotStarted && State.State != EnumState.Finished && State.State != EnumState.Cancelled)
            {
                State.State = EnumState.Paused;
            }
        }

        /// <summary>
        /// Resumes a paused backup operation, setting the state to 'InProgress'.
        /// </summary>
        public void ResumeBackup()
        {
            if (State.State != EnumState.InProgress)
            {
                State.State = EnumState.InProgress;
                PerformBackup();
            }
        }
    }
}
