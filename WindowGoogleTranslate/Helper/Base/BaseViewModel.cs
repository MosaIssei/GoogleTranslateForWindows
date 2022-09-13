using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WindowGoogleTranslate.Helper.Base
{
    public abstract class BaseViewModel : BaseModel
    {
        private WindowState windowState;

        public WindowState WindowState
        {
            get { return windowState; }
            set
            {
                windowState = value;
                OnPropertyChanged();
            }
        }

        public ICommand CloseWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand {
            get;
            set;
        }
        public ICommand MaximizeWindowCommand { get; set; }

        public BaseViewModel()
        {
            CloseWindowCommand = new RealeyCommand(CloseWindow);
            MinimizeWindowCommand = new RealeyCommand(MinimizeWindow);
            MaximizeWindowCommand = new RealeyCommand(MaximizeNormalWindow);
        }

        public virtual void CloseWindow()
        {
            App.Current.Shutdown();
        }

        public virtual void MaximizeNormalWindow()
        {
            WindowState ^= WindowState.Maximized;
        }

        public virtual void MinimizeWindow()
        {
            WindowState = WindowState.Minimized;
        }

    }
}
