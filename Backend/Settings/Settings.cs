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
        public List<string> PriorityExtensionsToBackup { get; set; }
        public List<string> ExtensionsToEncrypt { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        private Settings()
        {
            // Initialize the default language, can be changed later
            LanguageSettings = new Language();
            LogSettings = new Logs();
            ExtensionsToEncrypt = new List<string>();
            PriorityExtensionsToBackup = new List<string>();
            AddExtensionsToEncrypt(".txt");
            AddPriorityExtensionToBackup(".png");

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

        /// <summary>
        /// Adds an extension to the list of extensions to encrypt.
        /// </summary>
        /// <param name="extension">The extension to add.</param>
        public void AddExtensionsToEncrypt(string extension)
        {
            ExtensionsToEncrypt.Add(extension);
        }

        /// <summary>
        /// Removes an extension from the list of extensions to encrypt.
        /// </summary>
        /// <param name="extension">The extension to remove.</param>
        public void RemoveExtensionToEncrypt(string extension)
        {
            ExtensionsToEncrypt.Remove(extension);
        }

        /// <summary>
        /// Adds a priority extension to the list of extensions to backup.
        /// </summary>
        /// <param name="extension">The priority extension to add.</param>
        public void AddPriorityExtensionToBackup(string extension)
        {
            PriorityExtensionsToBackup.Add(extension);
        }

        /// <summary>
        /// Removes a priority extension from the list of extensions to backup.
        /// </summary>
        /// <param name="extension">The priority extension to remove.</param>
        public void RemovePriorityExtensionToBackup(string extension)
        {
            PriorityExtensionsToBackup.Remove(extension);
        }
    }
}
