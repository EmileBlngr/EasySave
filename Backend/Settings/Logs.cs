
using Backend.Backup;
using System.Text.Json;

namespace Backend.Settings
{
    public class Logs
    {
        public bool WriteToJson { get; set; }
        public bool WriteToTxt { get; set; }
        public EnumLogFormat LogFormat { get; set; }

        public Logs(EnumLogFormat logFormat)
        {
            LogFormat = logFormat;
            WriteToJson = true;
            WriteToTxt = false;
        }

        public void WriteLog(BackupLogEntry logEntry)
        {
            string logMessage = FormatLogMessage(logEntry);
            SaveLogMessage(logMessage);
        }

        private string FormatLogMessage(BackupLogEntry logEntry)
        {
            switch (LogFormat)
            {
                case EnumLogFormat.Json:
                    // formate le message avec JsonSerializer
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    return JsonSerializer.Serialize(logEntry, options) + ",";
                case EnumLogFormat.Txt:
                default:
                    // Format pour un fichier texte simple
                    return $"{logEntry.Time}: Backup '{logEntry.Name}' from '{logEntry.FileSource}' to '{logEntry.FileTarget}' " +
                           $"size {logEntry.FileSize} bytes took {logEntry.FileTransferTime}ms";
            }
        }


        private void SaveLogMessage(string message)
        {
            string fileName = GetLogFileName();
            File.AppendAllText(fileName, message + Environment.NewLine);
        }

        private string GetLogFileName()
        {
            string logsDirectory = "Logs"; //Nom du répertoire pour les fichiers de log
            if (!Directory.Exists(logsDirectory))
            {
                // Crée le répertoire s'il n'existe pas déjà
                Directory.CreateDirectory(logsDirectory);
            }

            // Construit le nom de fichier en fonction de la date et du format de log
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string extension = LogFormat == EnumLogFormat.Json ? "json" : "txt";
            string fileName = $"log_{date}.{extension}";

            // Retourne le chemin complet du fichier de log
            return Path.Combine(logsDirectory, fileName);
        }
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

            Console.WriteLine("Les fichiers de logs ont été créés.");
        }
    }
}
