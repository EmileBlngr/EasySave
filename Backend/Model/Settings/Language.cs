using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Backend.Model.Settings;

namespace Backend.Model.Settings
{
    public class Language
    {
        public string LanguageFile {get; set; }
        public EnumLanguages CurrentLanguage { get; set; } // Utilisation de l'enum déjà défini
        public Dictionary<string, string> LanguageData { get; private set; }

        public Language(EnumLanguages language = EnumLanguages.English)
        {
            CurrentLanguage = language;
            LanguageFile = Path.Combine(
                @"C:\Users\romeo\OneDrive\Bureau\A3 INFO\Projet Programmation systeme\Projet EasySave groupe 2\Frontend console\bin\Debug\net6.0\languages",
                $"{CurrentLanguage.ToString().ToLower()}.json"); //Construit le nom de fichier basé sur la langue sélectionnée 
        }

        public void loadFileLocal()
        {
            if (File.Exists(LanguageFile)) //vérifier si le fichier existe
            {
                string jsonString = File.ReadAllText(LanguageFile);
                try
                {
                    LanguageData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString); //Affecte les données déserialisées à la propriété LanguageData
                }
                catch (JsonException e)
                {
                    Console.WriteLine($"An error occurred: {e.Message}");
                }

            }
            else 
            {
                Console.WriteLine("Language file not found");
            }
        }
        public void ShowFirstValue()
        {


            Console.WriteLine($"Attempting to load file at: {LanguageFile}");
            // Chargement des données de langue si ce n'est pas déjà fait
            if (LanguageData == null || !LanguageData.Any())
            {
                loadFileLocal();
            }

            // Vérification de la présence de la clé "new_backup_access"
            if (LanguageData != null && LanguageData.TryGetValue("new_backup_access", out string value))
            {
                Console.WriteLine(value);
            }
            else
            {
                Console.WriteLine("La clé spécifiée est introuvable dans les données de langue.");
            }
        }
    }
}
