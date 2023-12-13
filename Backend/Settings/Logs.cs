
using Backend.Backup;
using System.Text.Json;

namespace Backend.Settings
{
    /// <summary>
    /// Class representing log settings and functionality.
    /// </summary>
    public class Logs
    {
        public bool WriteToJson { get; set; }
        public bool WriteToTxt { get; set; }
        public EnumLogFormat LogFormat { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Logs"/> class with the specified log format.
        /// </summary>
        /// <param name="logFormat">The log format.</param>
        public Logs(EnumLogFormat logFormat)
        {
            LogFormat = logFormat;
            WriteToJson = true;
            WriteToTxt = false;
        }

        /// <summary>
        /// Writes a log entry using the specified log format.
        /// </summary>
        /// <param name="logEntry">The log entry to be written.</param>
        public void WriteLog(BackupLogEntry logEntry)
        {
            string logMessage = FormatLogMessage(logEntry);
            SaveLogMessage(logMessage);
        }

        /// <summary>
        /// Formats a log message based on the specified log format.
        /// </summary>
        /// <param name="logEntry">The log entry to be formatted.</param>
        /// <returns>The formatted log message.</returns>
        private string FormatLogMessage(BackupLogEntry logEntry)
        {
            switch (LogFormat)
            {
                case EnumLogFormat.Json:
                    // Format the message using JsonSerializer
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    return JsonSerializer.Serialize(logEntry, options) + ",";
                case EnumLogFormat.Txt:
                default:
                    // Format for a simple text file
                    return $"{logEntry.Time}: Backup '{logEntry.Name}' from '{logEntry.FileSource}' to '{logEntry.FileTarget}' " +
                           $"size {logEntry.FileSize} bytes took {logEntry.FileTransferTime}ms";
            }
        }

        /// <summary>
        /// Saves a log message to the log file.
        /// </summary>
        /// <param name="message">The log message to be saved.</param>
        private void SaveLogMessage(string message)
        {
            string fileName = GetLogFileName();
            File.AppendAllText(fileName, message + Environment.NewLine);
        }

        /// <summary>
        /// Gets the log file name based on the current date and log format.
        /// </summary>
        /// <returns>The full path of the log file.</returns>
        private string GetLogFileName()
        {
            string logsDirectory = "Logs"; // Name of the directory for log files
            if (!Directory.Exists(logsDirectory))
            {
                // Create the directory if it does not already exist
                Directory.CreateDirectory(logsDirectory);
            }

            // Build the file name based on the date and log format
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string extension = LogFormat == EnumLogFormat.Json ? "json" : "txt";
            string fileName = $"log_{date}.{extension}";

            // Return the full path of the log file
            return Path.Combine(logsDirectory, fileName);
        }

        /// <summary>
        /// Gets the state of the log format (enabled or disabled) based on the specified log format.
        /// </summary>
        /// <param name="logFormat">The log format.</param>
        /// <returns>The state of the log format (enabled or disabled).</returns>
        public bool GetLogFormatState(EnumLogFormat logFormat)
        {
            switch (logFormat)
            {
                case EnumLogFormat.Json:
                    return WriteToJson;
                case EnumLogFormat.Txt:
                    return WriteToTxt;
                default:
                    throw new ArgumentException("Format de log non pris en charge");
            }
        }

        /// <summary>
        /// Sets the state of the log format (enabled or disabled) based on the specified log format.
        /// </summary>
        /// <param name="logFormat">The log format.</param>
        /// <param name="newState">The new state to set.</param>
        public void SetLogFormatState(EnumLogFormat logFormat, bool newState)
        {
            switch (logFormat)
            {
                case EnumLogFormat.Json:
                    WriteToJson = newState;
                    break;
                case EnumLogFormat.Txt:
                    WriteToTxt = newState;
                    break;
                default:
                    throw new ArgumentException("Format de log non pris en charge");
            }
        }

        /// <summary>
        /// Creates log files for the specified backup.
        /// </summary>
        /// <param name="backup">The backup for which log files are created.</param>
        public void Createlogs(ABackup backup)
        {
            var logEntries = new List<BackupLogEntry>
    {
        new BackupLogEntry
        {
            Name = backup.Name,
            FileSource = backup.SourceDirectory,
            FileTarget = backup.TargetDirectory,
            FileSize = backup.TotalSize,
            FileTransferTime = backup.FileTransferTime
        },

    };

            if (WriteToJson)
            {
                Logs jsonLogger = new(EnumLogFormat.Json);
                logEntries.ForEach(jsonLogger.WriteLog);
            }

            if (WriteToTxt)
            {
                Logs txtLogger = new(EnumLogFormat.Txt);
                logEntries.ForEach(txtLogger.WriteLog);
            }
        }
    }
}
