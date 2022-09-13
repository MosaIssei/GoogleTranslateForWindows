using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WindowGoogleTranslate.Thems
{
    public static class ThemesController
    {
        public static ThemeType CurrentTheme { get; set; }

        public enum ThemeType
        {
            Light,
            Dark,
        }

        public static ResourceDictionary ThemeDictionary
        {
            get { return Application.Current.Resources.MergedDictionaries[0]; }
            set { Application.Current.Resources.MergedDictionaries[0] = value; }
        }

        public static void ChangeTheme(string ThemeName)
        {
            ThemeDictionary = new ResourceDictionary() { Source = new Uri($"Themes/{ThemeName}Theme.xaml", UriKind.Relative) };
        }

        public static void SetTheme(ThemeType Theme)
        {
            string themeName = string.Empty;
            CurrentTheme = Theme;
            switch (Theme)
            {
                case ThemeType.Light:
                    themeName = "Light";
                    Properties.Settings.Default.DarkMode = false;
                    break;
                case ThemeType.Dark:
                    themeName = "Dark";
                    Properties.Settings.Default.DarkMode = true;
                    break;
                default:
                    break;
            }

            try
            {
                if (string.IsNullOrEmpty(themeName))
                {
                    return;
                }
                ChangeTheme(themeName);
                Properties.Settings.Default.Save();
            }
            catch (Exception)
            {

            }
        }
        public static void SetTheme()
        {
            switch (CurrentTheme)
            {
                case ThemeType.Light:
                    SetTheme(ThemeType.Dark);
                    break;
                case ThemeType.Dark:
                    SetTheme(ThemeType.Light);
                    break;
                default:
                    break;
            }

        }
    }
}
