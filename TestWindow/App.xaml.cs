using CefSharp.Wpf;
using CefSharp;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

namespace TestWindow
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
#if ANYCPU
            CefRuntime.SubscribeAnyCpuAssemblyResolver();
#endif
            var settings = new CefSettings()
            {
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache")
            };

            
            settings.CefCommandLineArgs.Add("enable-media-stream");
            
            settings.CefCommandLineArgs.Add("use-fake-ui-for-media-stream");
            
            settings.CefCommandLineArgs.Add("enable-usermedia-screen-capturing");

            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
            }
        }
    }
}
