
using System.Text.Json;

namespace Backend.Settings
{
    public class Language
    {
        public string LanguageFile { get; set; }
        public EnumLanguages CurrentLanguage { get; set; } // used already defined enum
        public Dictionary<string, string> LanguageData { get; set; }

        public Language()
        {
            CurrentLanguage = EnumLanguages.FR;
            string basePath = AppDomain.CurrentDomain.BaseDirectory; // get the local path of the app

            string languageCode = ConvertEnumToLanguageCode(CurrentLanguage);
            //Console.WriteLine($"the local path catched is :{basePath}");

            LanguageFile = Path.Combine(basePath, "Data", "languages", $"{languageCode}.json"); // Create a local path to the json file language
            //Console.WriteLine($"the local path created is :{LanguageFile}");
            CreateLanguageFile();
        }

        public string ConvertEnumToLanguageCode(EnumLanguages language)
        {
            string code = language.ToString().ToLower();
            return $"{code}-{code.ToUpper()}";
        }
        public void CreateLanguageFile()
        {
            if (LanguageData == null || !LanguageData.Any())
            {
                loadFileLocal();
            }
        }
        public void loadFileLocal()
        {
            //Console.WriteLine($"Trying to load language file at: {LanguageFile}");

            if (File.Exists(LanguageFile)) //vérifier si le fichier existe
            {
                string jsonString = File.ReadAllText(LanguageFile);
                
                try
                {
                    LanguageData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString); //affect deserialized data to the LanguageData property
                }
                catch (JsonException e)
                {
                    Console.WriteLine($"An error occurred during JSON deserialization: {e.Message}");
                }

            }
            else
            { 
                Console.WriteLine("Language file not found");}

        }
        /// <summary>
        /// 
        /// </summary>
        public void ShowFirstValue()
        {


            Console.WriteLine($"Attempting to load file at: {LanguageFile}");
            // create the language folder if not already created
            if (LanguageData == null || LanguageData.Count==0)
            {
                loadFileLocal();
            }

            // veryfying the key "new_backup_access" exist
            if (LanguageData != null && LanguageData.TryGetValue("new_backup_access", out string value))

                Console.WriteLine(value);

            else

                Console.WriteLine("La clé spécifiée est introuvable dans les données de langue.");

        }
    }
}
