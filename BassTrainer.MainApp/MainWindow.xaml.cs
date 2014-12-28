using System;
using System.Windows;
using BassTrainer.Core.Settings;
using BassTrainer.UI.WPF.SettingsManager;
using MahApps.Metro.Controls;

namespace BassTrainer.MainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeConfiguration();
            InitializeComponent();
            try
            {
                DataContext = new MainAppViewModel();
            }
            catch (Exception ex)
            {
                ShowErrorAndExit(ex);
            }
        }

        private void InitializeConfiguration()
        {
            var settingsConfigurator = new DotNetSettingsConfigurator();
            settingsConfigurator.Read(Settings.Instance);
            
        }

        private void ShowErrorAndExit(Exception ex)
        {
            MessageBox.Show(ex.Message, "Error during initialization");
            Close();
        }
    }
}