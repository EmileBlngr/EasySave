

namespace Backend.Settings
{
    public class Settings
    {
        // Instance privée statique du singleton
        private static Settings _instance;
        public Language LanguageSettings { get; set; }
        public Logs LogSettings { get; set; }

        public Settings()
        {
            // Initialiser la langue par défaut, peut être changée plus tard
            LanguageSettings = new Language();
            LogSettings = new Logs(EnumLogFormat.Json);

        }
        // Méthode publique statique pour accéder à l'instance
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
        }
        public EnumLanguages GetLanguage()
        {
            return LanguageSettings.CurrentLanguage;
        }

        public void SetLanguage(EnumLanguages newLanguage)
        {
            LanguageSettings.CurrentLanguage = newLanguage;
            string languageCode = LanguageSettings.ConvertEnumToLanguageCode(newLanguage);
            LanguageSettings.LanguageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "languages", $"{languageCode}.json");
            LanguageSettings.loadFileLocal();
            LanguageSettings.CreateLanguageFile();           
        }
        // méthodes pour gérer les logs
    }


}
