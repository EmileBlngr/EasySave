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
            if (BackupManager.IsBusinessSoftwareRunning())
            {
                Console.WriteLine("Le logiciel métier est en cours d'exécution. La sauvegarde ne peut pas être lancée.");
                return;
            }
            ScanFiles();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ProgressDisplayTimer.Start();

            try
            {
                string[] sourceFiles = Directory.GetFiles(SourceDirectory);
                if (State.CurrentFileIndex == 0)
                {
                    State.RemainingFiles = sourceFiles.Length;
                    State.RemainingSize = TotalSize;
                }

                //create an array with the priority files
                var priorityFiles = sourceFiles.Where(file =>
                    Settings.Settings.GetInstance().PriorityExtensionsToBackup.Any(ext =>
                        file.EndsWith(ext, StringComparison.OrdinalIgnoreCase))).ToArray();

                //create an array with the remaining files

                var remainingFiles = sourceFiles.Except(priorityFiles).ToArray();

                //merge both array into one with priority files in first
                string[] allFiles = priorityFiles.Concat(remainingFiles).ToArray();

                // browse all files starting with the priority ones
                for (int i = State.CurrentFileIndex; i < allFiles.Length; i++)
                {
                    if (State.State == EnumState.Cancelled)
                    {                        
                        break;
                    }

                    else if (State.State == EnumState.Paused || BackupManager.IsBusinessSoftwareRunning())
                    {
                        State.State = EnumState.Paused;
                        // save current index and leave without breaking the loop
                        State.CurrentFileIndex = i;
                        return;
                    }

                    string sourceFilePath = allFiles[i];
                    ProcessFile(sourceFilePath);
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
                    if (State.State != EnumState.Cancelled)
                        Encrypt();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    if (State.State != EnumState.Cancelled && State.State != EnumState.Paused)
                    {
                        //set progress to 100%, mark as finished, and log
                        this.State.Progress = 1.0f; // Set to 100%
                        OnProgressUpdated();
                        State.State = EnumState.Finished;
                        Console.WriteLine(string.Format(Settings.Settings.GetInstance().LanguageSettings.LanguageData["full_backup_finished"], FileTransferTime));
                        Console.WriteLine("\n\n\n");
                        Settings.Settings.GetInstance().LogSettings.Createlogs(this);
                    }
                    else
                    {
                        Console.WriteLine("Backup was cancelled.");
                    }

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

            cryptosoftProcess.StartInfo.CreateNoWindow = true;
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

            int fileSizeKB = (int)(new FileInfo(sourceFilePath).Length / 1024);
            if (Settings.Settings.GetInstance().CumulativeTransferSizeKB <= 0)
                Settings.Settings.GetInstance().CumulativeTransferSizeKB = 0;
            int currentCumulativeSize = Settings.Settings.GetInstance().CumulativeTransferSizeKB;

            try
            {
                // Copy files if cumulative currently copying size is less than the maximum
                if (currentCumulativeSize <= Settings.Settings.GetInstance().MaxParallelTransferSizeKB)
                {
                    if (!File.Exists(targetFilePath) || File.GetLastWriteTimeUtc(sourceFilePath) > File.GetLastWriteTimeUtc(targetFilePath))
                    {
                        Settings.Settings.GetInstance().CumulativeTransferSizeKB += fileSizeKB;
                        File.Copy(sourceFilePath, targetFilePath, true);

                        State.RemainingFiles--;
                        State.RemainingSize -= (int)(new FileInfo(sourceFilePath).Length);
                        UpdateProgress();
                        Thread.Sleep(250);
                        Settings.Settings.GetInstance().CumulativeTransferSizeKB -= fileSizeKB;
                        // Update cumulative transfer size
                    }
                }
                else
                {
                    Thread.Sleep(250);
                    ProcessFile(sourceFilePath);
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"Error while copying files : {sourceFilePath} is used by another process.");
                Settings.Settings.GetInstance().CumulativeTransferSizeKB -= fileSizeKB;
                Thread.Sleep(250);
                ProcessFile(sourceFilePath);
            }
        }
    }
}
