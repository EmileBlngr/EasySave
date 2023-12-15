
namespace Backend.Settings
{
    /// <summary>
    /// Singleton class responsible for managing application settings.
    /// </summary>
    public class Settings
    {
        // Private static instance of the singleton
        private static Settings _instance;
        public Language LanguageSettings { get; set; }
        public Logs LogSettings { get; set; }

        public string IgnoredFile { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        private Settings()
        {
            // Initialize the default language, can be changed later
            LanguageSettings = new Language();
            LogSettings = new Logs();

        }

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static Settings GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Settings();
            }
            return _instance;
        }

        /// <summary>
        /// Gets the current language.
        /// </summary>
        /// <returns>The current language.</returns>
        public EnumLanguages GetLanguage()
        {
            return LanguageSettings.CurrentLanguage;
        }

        /// <summary>
        /// Sets the application language.
        /// </summary>
        /// <param name="newLanguage">The new language to set.</param>
        public void SetLanguage(EnumLanguages newLanguage)
        {
            LanguageSettings.CurrentLanguage = newLanguage;
            string languageCode = LanguageSettings.ConvertEnumToLanguageCode(newLanguage);
            LanguageSettings.LanguageFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "languages", $"{languageCode}.json");
            LanguageSettings.loadFileLocal();
            LanguageSettings.CreateLanguageFile();
        }

        public void SetIgnoreFile(string fileName)
        {
            IgnoredFile = fileName;
        }

        public string GetIgnoredFile()
        { 
            return IgnoredFile; 
        }
    }
}
