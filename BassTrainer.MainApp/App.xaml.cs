using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace BassTrainer.MainApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var ex = e.Exception;
            var metroWindow = (MetroWindow)Current.MainWindow;
            if(metroWindow != null)
            metroWindow.ShowMessageAsync(ex.Message, "Error");
            e.Handled = true;
        }
    
    }


}
