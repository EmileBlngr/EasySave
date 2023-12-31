﻿namespace Backend.Backup
{

    /// <summary>
    /// Represents the state of a backup operation, including the current state, progress, remaining files, remaining size,
    /// and information about the current source and target files.
    /// </summary>
    public class BackupState
    {
        public EnumState State { get; set; }
        public float Progress { get; set; }
        public int RemainingFiles { get; set; }

        public long RemainingSize { get; set; }

        public string CurrentFileSource { get; set; }

        public string CurrentFileTarget { get; set; }

        public int CurrentFileIndex { get; set; }

        /// <summary>
        /// Initializes a new instance of the BackupState class.
        /// The constructor sets the initial state of the backup, progress, remaining files, remaining size,
        /// current source file, and current target file.
        /// </summary>
        public BackupState()
        {
            State = EnumState.NotStarted;
            Progress = 0.0f;
            RemainingFiles = 0;
            RemainingSize = 0;
            CurrentFileSource = "";
            CurrentFileTarget = "";
            CurrentFileIndex = 0;
        }
    }
}
