using Backend.Settings;
using System.Diagnostics;

namespace Backend.Backup
{
    /// <summary>
    /// BackupFull class represents a full backup strategy 
    /// and inherits from the ABackup abstract class.
    /// </summary>
    public class BackupFull : ABackup
    {
        /// <summary>
        /// Constructor method of BackupFull, based on the constructor of ABackup.
        /// It is empty because it only implements the basic constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sourceDirectory"></param>
        /// <param name="targetDirectory"></param>
        public BackupFull(string name, string sourceDirectory, string targetDirectory) : base(name, sourceDirectory, targetDirectory)
        {

        }

        /// <summary>
        /// Performs a full backup operation.
        /// Copies all files from the source directory to the target directory.
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
                    File.Copy(sourceFilePath, targetFilePath, true);

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
                Console.WriteLine(string.Format(Settings.Settings.Instance.LanguageSettings.LanguageData["error_copying_files"], ex.Message));

            }
            finally
            {
                stopwatch.Stop();
                ProgressDisplayTimer.Stop();
                FileTransferTime = (float)stopwatch.Elapsed.TotalSeconds;
                State.State = EnumState.Finished;
                Console.WriteLine(string.Format(Settings.Settings.Instance.LanguageSettings.LanguageData["differential_backup_success"], FileTransferTime));
            }
        }

    }
}
