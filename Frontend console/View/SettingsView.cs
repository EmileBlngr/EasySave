using Backend.Backup;
using Backend.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend_console.View
{
    public class SettingsView
    {

        public static void AccessSettings(BackupManager backupManager)
        {
            string inputLanguage;
            string inputlogs;

            Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["enter_settings"]);
            Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["go_back"]);
            Console.WriteLine("2. " + backupManager.Settings.LanguageSettings.LanguageData["logs_format_title"]);
            Console.WriteLine("3. " + backupManager.Settings.LanguageSettings.LanguageData["language_title"]);
            string userInput = Console.ReadLine();
            switch (userInput)
            {
                case "1":
                    break;
                case "2":
                    Console.WriteLine("Choisissez le ou les formats de logs à activer/désactiver :");

                    // Afficher tous les formats de logs disponibles
                    foreach (var enum_log in Enum.GetValues(typeof(EnumLogFormat)))
                    {
                        Console.WriteLine(enum_log);
                    }
                    inputlogs = Console.ReadLine();

                    if (Enum.TryParse(inputlogs, true, out EnumLogFormat selectedLogFormat))
                    {
                        bool currentState = backupManager.Settings.LogSettings.GetLogFormatState(selectedLogFormat);
                        Console.WriteLine($"Le format de log '{selectedLogFormat}' est actuellement {(currentState ? "activé" : "désactivé")}.");

                        // Demander à l'utilisateur de modifier l'état
                        Console.WriteLine("Entrez 'true' pour activer ou 'false' pour désactiver ce format de log:");
                        string newStateInput = Console.ReadLine();
                        if (bool.TryParse(newStateInput, out bool newState))
                        {
                            if (currentState == newState)
                            {
                                Console.WriteLine($"Ce format de log est déjà sur '{newState}'.");
                            }
                            else
                            {
                                backupManager.Settings.LogSettings.SetLogFormatState(selectedLogFormat, newState);
                                Console.WriteLine($"Le format de log '{selectedLogFormat}' a été {(newState ? "activé" : "désactivé")}.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Entrée invalide. Veuillez entrer 'true' ou 'false'.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Sélection de logs invalide.");
                    }
                    break;
                case "3":
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["choose_language"]);

                    // Afficher toutes les langues disponibles
                    foreach (var enum_language in Enum.GetValues(typeof(EnumLanguages)))
                    {
                        Console.WriteLine(enum_language);
                    }

                    // Récupérer l'entrée de l'utilisateur
                    inputLanguage = Console.ReadLine();

                    // Essayer de convertir l'entrée en EnumLanguages
                    if (Enum.TryParse(inputLanguage, true, out EnumLanguages selectedLanguage))
                    {
                        // Appeler SetLanguage avec la langue sélectionnée
                        backupManager.Settings.SetLanguage(selectedLanguage);
                        Console.WriteLine("Language changed successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid language selection.");
                    }
                    break;

                default:
                    Console.WriteLine(backupManager.Settings.LanguageSettings.LanguageData["invalid_choice"]);
                    break;
            }





        }

    }
}
