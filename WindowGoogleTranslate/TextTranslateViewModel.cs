using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using System.Windows;

using WindowGoogleTranslate.Helper.Base;
using System.Windows.Input;
using WindowGoogleTranslate.Thems;

namespace WindowGoogleTranslate
{
    public class TextTranslateViewModel : BaseViewModel
    {
        #region Properties

        private bool darkModeChecked = true;

        public bool DarkModeChecked
        {
            get { return darkModeChecked; }
            set
            {
                darkModeChecked = value;
                OnPropertyChanged();
                changeSkin(value);
            }
        }

        private void changeSkin(bool value)
        {
            if (value)
            {
                ThemesController.SetTheme(ThemesController.ThemeType.Dark);
            }
            else
            {
                ThemesController.SetTheme(ThemesController.ThemeType.Light);
            }

        }

        private string sourceText;

        public string SourceText
        {
            get { return sourceText; }
            set
            {
                sourceText = value;
                OnPropertyChanged();
                SendTranslate(value);
            }
        }

        private string translateText;

        public string TranslateText
        {
            get { return translateText; }
            set
            {
                translateText = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands


        #endregion

        #region Constractor

        public TextTranslateViewModel()
        {
            if (Properties.Settings.Default.DarkMode)
            {
                ThemesController.SetTheme(ThemesController.ThemeType.Dark);
                darkModeChecked = true;
            }
            else
            {
                ThemesController.SetTheme(ThemesController.ThemeType.Light);
                darkModeChecked = false;
            }

           
        }

        #endregion

        

        private async void SendTranslate(string value)
        {
            Clipboard.SetText("Hello");
            if (string.IsNullOrEmpty(value.Trim()))
            {
                TranslateText = string.Empty;
                return;
            }
            using (var client = new HttpClient())
            {
                try
                {
                    var result = await client.GetAsync(new Uri($"https://translate.googleapis.com/translate_a/single?client=gtx&sl=en&tl=fa&dt=t&q={value}"));
                    var json = await result.Content.ReadAsStringAsync();
                    var trLines = JsonSerializer.Deserialize<object[]>(json);
                    var trLinesText = JsonSerializer.Deserialize<object[]>(trLines[0].ToString());

                    string text = string.Empty;
                    for (int i = 0; i < trLinesText.Length; i++)
                    {
                        text += JsonSerializer.Deserialize<object[]>(trLinesText[i].ToString())[0].ToString();
                    }
                    if (string.IsNullOrEmpty(SourceText.Trim()))
                    {
                        TranslateText = string.Empty;
                        return;
                    }
                    TranslateText = text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }


        }
    }
}
