using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace BassTrainer.MainApp.MainContent
{
    public interface IMainAppViewModel
    {
        ICommand ShowMetronome { get; }
        ICommand ShowDrumMachine { get; }
        ICommand ShowOptions { get; }
        ICommand ShowHelp { get; }
        ICommand ShowTrainer { get; }
        ICommand ShowBassTuner { get; }

        ObservableCollection<FrameworkElement> MainContentItems { get; }

        bool IsSettingsOpened { get; set; }
        void OpenDefaultView();
        void OpenMainMenu();
        void OpenBassTuner();
        void OpenDrumMachine();
        void OpenTrainer();
        void OpenHelp();
        void OpenOptions();
        void OpenMetronome();

        event PropertyChangedEventHandler PropertyChanged;
    }
}