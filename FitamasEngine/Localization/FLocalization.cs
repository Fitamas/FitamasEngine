using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitamas.Localization
{
    public static class FLocalization
    {
        //private static Dictionary<string, Dictionary<string, string>> _translations = new();
        private static string _currentLanguage = "en";
        private static event Action OnLanguageChanged;

        // Поддерживаемые языки
        public static readonly Dictionary<string, string> SupportedLanguages = new()
        {
            ["en"] = "English",
        };

        public static string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value && SupportedLanguages.ContainsKey(value))
                {
                    _currentLanguage = value;
                    OnLanguageChanged?.Invoke();
                }
            }
        }
    }
}
