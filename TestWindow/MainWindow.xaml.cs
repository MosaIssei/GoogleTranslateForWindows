using CefSharp;
using CefSharp.Wpf;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace TestWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int count = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cefs.Address = $"https://translate.google.com/?sl=en&tl=fa&text={Search.Text}&op=translate";
            
        }

        private async void cefs_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
                await Task.Delay(TimeSpan.FromSeconds(1));
                JavascriptResponse jr = await cefs.EvaluateScriptAsync("document.getElementsByClassName(\"Q4iAWc\")[0].innerHTML");
                App.Current.Dispatcher.Invoke(() =>
                {
                    Result.Text = jr.Result.ToString();
                });
        }
    }
}
