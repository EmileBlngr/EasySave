﻿using System.Diagnostics;
using Timer = System.Timers.Timer;

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
            ScanFiles();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ProgressDisplayTimer.Start();
            try
            {
                // Gets the list of files in the source directory
                string[] sourceFiles = Directory.GetFiles(SourceDirectory);
                State.RemainingFiles = sourceFiles.Length;
                State.RemainingSize = TotalSize;
                // Copy each file to the target directory
                foreach (string sourceFilePath in sourceFiles)
                {
                    string fileName = Path.GetFileName(sourceFilePath);
                    string targetFilePath = Path.Combine(TargetDirectory, fileName);
                    State.CurrentFileSource = sourceFilePath;
                    State.CurrentFileTarget = targetFilePath;
                    File.Copy(sourceFilePath, targetFilePath, true); // true to overwrite existing files

                    State.RemainingFiles--;
                    FileInfo fileInfo = new FileInfo(sourceFilePath);
                    State.RemainingSize -= fileInfo.Length;
                    UpdateProgress();
                    Thread.Sleep(500);
                }

            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                ProgressDisplayTimer.Stop();
                State.State = EnumState.Failed;
                Console.WriteLine($"Error copying files: {ex.Message}");
            }
            finally
            {
                stopwatch.Stop();
                ProgressDisplayTimer.Stop();
                // Assigning the total backup time to FileTransferTime
                FileTransferTime = (float)stopwatch.Elapsed.TotalSeconds;
                State.State = EnumState.Finished;
                Console.WriteLine($"Differential Backup finished successfully in {FileTransferTime} seconds");
            }
        }

    }
}
