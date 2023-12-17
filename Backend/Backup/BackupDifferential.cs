using System.Diagnostics;
using Backend.Settings;

namespace Backend.Backup
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

                // Process priority extensions first
                foreach (string sourceFilePath in sourceFiles)
                {
                    string extension = Path.GetExtension(sourceFilePath);
                    string fileName = Path.GetFileName(sourceFilePath);
                    if (fileName.Equals(Settings.Settings.GetInstance().GetIgnoredFile()))
                    {
                        continue; 
                    }

                    string targetFilePath = Path.Combine(TargetDirectory, fileName);
                    State.CurrentFileSource = sourceFilePath;
                    State.CurrentFileTarget = targetFilePath;

                    if (Settings.Settings.GetInstance().PriorityExtensionsToBackup.Any(ext => extension.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                    {
                        ProcessFile(sourceFilePath);
                    }
                }

                // Process other extensions
                foreach (string sourceFilePath in sourceFiles)
                {
                    string extension = Path.GetExtension(sourceFilePath);

                    if (!Settings.Settings.GetInstance().PriorityExtensionsToBackup.Any(ext => extension.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                    {
                        ProcessFile(sourceFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                ProgressDisplayTimer.Stop();
                State.State = EnumState.Failed;
                Console.WriteLine(string.Format(Settings.Settings.GetInstance().LanguageSettings.LanguageData["error_copying_modified_files"], ex.Message));

            }
            finally
            {
                stopwatch.Stop();
                ProgressDisplayTimer.Stop();
                FileTransferTime = (float)stopwatch.Elapsed.TotalSeconds;

                try
                {
                    Encrypt();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    State.State = EnumState.Finished;
                    Console.WriteLine(string.Format(Settings.Settings.GetInstance().LanguageSettings.LanguageData["full_backup_finished"], FileTransferTime));
                    Console.WriteLine("\n\n\n");
                    Settings.Settings.GetInstance().LogSettings.Createlogs(this);
                }
            }
        }
        /// <summary>
        /// Encrypts files using the Cryptosoft utility.
        /// </summary>
        private void Encrypt()
        {
            // Create a new process to execute Cryptosoft
            Process cryptosoftProcess = new Process();
            string[] targetFiles = Directory.GetFiles(TargetDirectory);

            string backendDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\..\\Backend\\Data\\Cryptosoft"));
            cryptosoftProcess.StartInfo.WorkingDirectory = backendDirectory;
            string cryptosoftPath = Path.Combine(backendDirectory, "Cryptosoft.exe");
            cryptosoftProcess.StartInfo.FileName = cryptosoftPath;

            cryptosoftProcess.StartInfo.RedirectStandardOutput = true;
            cryptosoftProcess.StartInfo.RedirectStandardError = true;
            Stopwatch encryptionStopwatch = new Stopwatch();
            encryptionStopwatch.Start();

            foreach (string targetFile in targetFiles)
            {
                string fileName = Path.GetFileName(targetFile);
                string extension = Path.GetExtension(targetFile);
                if (Settings.Settings.GetInstance().ExtensionsToEncrypt.Contains(extension))
                {
                    string targetFilePath = Path.Combine(TargetDirectory, fileName);
                    cryptosoftProcess.StartInfo.Arguments = $"-d \"{targetFilePath}\"";

                    cryptosoftProcess.Start();
                    cryptosoftProcess.WaitForExit();
                }
            }

            encryptionStopwatch.Stop();
            EncryptTime = (float)encryptionStopwatch.Elapsed.TotalSeconds * 1000;
            Console.WriteLine($"Encrypt time: {EncryptTime}");

            string output = cryptosoftProcess.StandardOutput.ReadToEnd();
            string error = cryptosoftProcess.StandardError.ReadToEnd();
        }

        /// <summary>
        /// Processes a file, copying it to the target directory if necessary.
        /// </summary>
        /// <param name="sourceFilePath">The path of the source file to be processed.</param>
        private void ProcessFile(string sourceFilePath)
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
}
