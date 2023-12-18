using System.Configuration;
using System.Data;
using System.Windows;

namespace FrontendWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string CurrentLanguage { get; set; } = "fr-FR";

        public static event Action<string> LanguageChanged;

        public static void OnLanguageChanged(string newLanguage)
        {
            CurrentLanguage = newLanguage;
            LanguageChanged?.Invoke(newLanguage);
        }
    }



}
