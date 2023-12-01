using System.Diagnostics;

namespace Backend.Model.Backup
{
    /// <summary>
    /// BackupDifferential class represents a differential backup strategy 
    /// and inherits from the ABackup abstract class.
    /// </summary>
    public class BackupDifferential : ABackup
    {
        /// <summary>
        ///  Constructor method of BackupDifferential, based on the constructor of ABackup.
        ///  It is empty because it only implements the basic constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sourceDirectory"></param>
        /// <param name="targetDirectory"></param>
        public BackupDifferential(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        {

        }
        /// <summary>
        /// Performs a differential backup operation.
        /// Copies files from the source directory to the target directory only if they don't exist 
        /// in the target directory or if they have been modified.
        /// Updates the progress, remaining files, and remaining size during the process.
        /// </summary>
        public override void PerformBackup()
        {
            ScanFiles();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ProgressDisplayTimer.Start();

            try
            {
                string[] sourceFiles = Directory.GetFiles(SourceDirectory);
                State.RemainingFiles = sourceFiles.Length;
                State.RemainingSize = TotalSize;
                foreach (string sourceFilePath in sourceFiles)
                {
                    string fileName = Path.GetFileName(sourceFilePath);
                    string targetFilePath = Path.Combine(TargetDirectory, fileName);
                    State.CurrentFileSource = sourceFilePath;
                    State.CurrentFileTarget = targetFilePath;

                    if (!File.Exists(targetFilePath) || File.GetLastWriteTimeUtc(sourceFilePath) > File.GetLastWriteTimeUtc(targetFilePath))
                    {
                        File.Copy(sourceFilePath, targetFilePath, true);
                        Thread.Sleep(500);
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
                FileTransferTime = (float)stopwatch.Elapsed.TotalSeconds;
                Console.WriteLine($"Differential Backup finished successfully in {FileTransferTime} seconds\n");
            }
        }
    }
}
