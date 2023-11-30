using System.Xml.Linq;
using System;
using Timer = System.Timers.Timer;
using System.Diagnostics;

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
                // Browse all files in the source directory
                foreach (string sourceFilePath in sourceFiles)
                {
                    string fileName = Path.GetFileName(sourceFilePath);
                    string targetFilePath = Path.Combine(TargetDirectory, fileName);
                    State.CurrentFileSource = sourceFilePath;
                    State.CurrentFileTarget = targetFilePath;

                    // If the file does not exist in the target directory or if it has been modified
                    if (!File.Exists(targetFilePath) || File.GetLastWriteTimeUtc(sourceFilePath) > File.GetLastWriteTimeUtc(targetFilePath))
                    {
                        File.Copy(sourceFilePath, targetFilePath, true); // true to overwrite existing files
                    }

                    State.RemainingFiles--;
                    FileInfo fileInfo = new FileInfo(sourceFilePath);
                    State.RemainingSize -= fileInfo.Length;
                    UpdateProgress();
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                ProgressDisplayTimer.Stop();
                State.State = EnumState.Failed;
                Console.WriteLine($"Error copying modified or new files: {ex.Message}");
            }
            finally
            {
                stopwatch.Stop();
                ProgressDisplayTimer.Stop();
                // Assigning the total backup time to FileTransferTime
                FileTransferTime = (float)stopwatch.Elapsed.TotalSeconds;
                Console.WriteLine($"Differential Backup finished successfully in {FileTransferTime} seconds");
            }
        }
    }
}
