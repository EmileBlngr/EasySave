
using System.Text.Json;

namespace Backend.Model.Settings
{
    public class Logs
    {
        public EnumLogFormat LogFormat { get; set; }

        public Logs(EnumLogFormat logFormat)
        {
            LogFormat = logFormat;
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

        public static void DemonstrateLogging(bool writeToJson, bool writeToTxt)
        {
            var logEntries = new List<BackupLogEntry>
    {
        new BackupLogEntry
        {
            Name = "Save1",
            FileSource = "D:\\Data\\source.txt",
            FileTarget = "E:\\Backup\\source.txt",
            FileSize = 1024,
            FileTransferTime = 100
        },
        new BackupLogEntry
        {
            Name = "Save2",
            FileSource = "D:\\Data\\image.png",
            FileTarget = "E:\\Backup\\image.png",
            FileSize = 2048,
            FileTransferTime = 200
        }
    };

            if (writeToJson)
            {
                Logs jsonLogger = new (EnumLogFormat.Json);
                logEntries.ForEach(jsonLogger.WriteLog);
            }

            // Vérifiez si TXT est sélectionné
            if (writeToTxt)
            {
                Logs txtLogger = new (EnumLogFormat.Txt);
                logEntries.ForEach(txtLogger.WriteLog);
            }

            Console.WriteLine("Les fichiers de logs ont été créés.");
        }
    }
}
