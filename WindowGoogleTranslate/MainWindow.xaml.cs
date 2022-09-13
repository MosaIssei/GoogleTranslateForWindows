using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

using WindowGoogleTranslate.Thems;

using static System.Net.Mime.MediaTypeNames;
using static WindowGoogleTranslate.Thems.ThemesController;

namespace WindowGoogleTranslate
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class RbTabModel
        {
            public string LanguageName { get; set; }
            public string LanguageCode { get; set; }
            public bool IsChecked { get; set; } = false;
        }

        bool isTrasnlating = false;
        string sourceLan = "auto";
        string TranslateLan = "fa";
        string sourc = "";
        List<string> rightToLeftLan;
        Dictionary<string, string> LanCode;
        public ObservableCollection<RbTabModel> Sourcelan { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            if (Properties.Settings.Default.DarkMode)
            {
                tbDark.IsChecked = true;
            }
            else
            {
                ThemesController.SetTheme(ThemeType.Light);
            }
            string path = AppDomain.CurrentDomain.BaseDirectory;
            LanCode = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText($"{path}Asset\\LanguageCode.json"));
            rightToLeftLan = JsonSerializer.Deserialize<List<string>>(File.ReadAllText($"{path}Asset\\RTLLanguage.json"));
            foreach (var item in LanCode.Take(3))
            {
                ItemsLanSourc.Items.Add(new { LanguageName = item.Key.ToUpper(), LanguageCode = item.Value});
            }
            txtResult.Document.Blocks.Clear();
            txtResult.Document.Blocks.Add(new Paragraph(new Run("ترجمه")));
            txtSource.Focus();

        }

        private void txtSource_TextChanged(object sender, TextChangedEventArgs e)
        {
            SendTranslate();

        }

        private async void SendTranslate()
        {
            sourc = ConvertRichTextBoxContentsToString(txtSource).Trim();
            
            if (string.IsNullOrEmpty(sourc))
            {
                txtResult.Document.Blocks.Clear();
                txtResult.Document.Blocks.Add(new Paragraph(new Run("ترجمه")));
                TranslateBack.Visibility = Visibility.Collapsed;
                rbAuto.Content = "DETECT LANGUAGE";
                return;
            }

            isTrasnlating = true;
            var client = new HttpClient();
            try
            {
                var result = await client.GetAsync(new Uri($"https://translate.googleapis.com/translate_a/single?client=gtx&sl={sourceLan}&tl={TranslateLan}&dt=t&q={sourc}"));
                var json = await result.Content.ReadAsStringAsync();
                var ee = JsonSerializer.Deserialize<object[]>(json);
                var rr = JsonSerializer.Deserialize<object[]>(ee[0].ToString());
                if (sourceLan == "auto")
                {
                    switch (ee[2].ToString())
                    {
                        case "fa":
                            rbAuto.Content = "PERSIAN - DETECTED";
                            txtSource.FlowDirection = FlowDirection.RightToLeft;
                            break;
                        case "ar":
                            rbAuto.Content = "ARABIC - DETECTED";
                            txtSource.FlowDirection = FlowDirection.RightToLeft;
                            break;
                        case "en":
                            rbAuto.Content = "ENGLISH - DETECTED";
                            txtSource.FlowDirection = FlowDirection.LeftToRight;
                            break;
                        case "ja":
                            rbAuto.Content = "JAPANESE - DETECTED";
                            txtSource.FlowDirection = FlowDirection.LeftToRight;
                            break;

                        default:
                            break;
                    }
                }

                txtResult.Document.Blocks.Clear();

                string text = string.Empty;
                for (int i = 0; i < rr.Length; i++)
                {
                    text += JsonSerializer.Deserialize<object[]>(rr[i].ToString())[0].ToString();
                }

                
                TranslateBack.Visibility = Visibility.Visible;
                txtResult.Document.Blocks.Add(new Paragraph(new Run(text)));
                isTrasnlating = false;
                if (string.IsNullOrEmpty(ConvertRichTextBoxContentsToString(txtSource).Trim()))
                {
                    txtResult.Document.Blocks.Clear();
                    txtResult.Document.Blocks.Add(new Paragraph(new Run("ترجمه")));
                    TranslateBack.Visibility = Visibility.Collapsed;
                    rbAuto.Content = "DETECT LANGUAGE";
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        string ConvertRichTextBoxContentsToString(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text;
        }

        private void Source_Checked(object sender, RoutedEventArgs e)
        {
            var rad = sender as RadioButton;
            RadioSourceChanged(rad);
        }

        private void RadioSourceChanged(RadioButton rad)
        {
            if (txtSource == null)
            {
                return;
            }

            sourceLan = rad.Tag.ToString();

            if (rightToLeftLan.Any(s => s == sourceLan))
            {
                txtSource.FlowDirection = FlowDirection.RightToLeft;
            }
            else
            {
                txtSource.FlowDirection = FlowDirection.LeftToRight;
            }
            
            if (sourceLan == TranslateLan)
            {

            }
            SendTranslate();
        }

        private void RadioTranslateChanged(RadioButton rad)
        {
            if (txtSource == null)
            {
                return;
            }

            switch (rad.Content.ToString().ToLower())
            {
                case "persian":
                    txtResult.FlowDirection = FlowDirection.RightToLeft;
                    TranslateLan = "fa";
                    break;
                case "english":
                    txtResult.FlowDirection = FlowDirection.LeftToRight;
                    TranslateLan = "en";
                    break;
                default:
                    break;
            }
            if (TranslateLan == sourceLan)
            {

            }
            SendTranslate();
        }

        private void Translate_Checked(object sender, RoutedEventArgs e)
        {
            var rad = sender as RadioButton;
            RadioTranslateChanged(rad);
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ThemesController.SetTheme(ThemeType.Dark);
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ThemesController.SetTheme(ThemeType.Light);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.DarkMode = tbDark.IsChecked.Value;
            Properties.Settings.Default.Save();
        }
    }
}
