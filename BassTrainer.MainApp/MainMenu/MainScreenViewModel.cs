using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BassTrainer.MainApp.MainContent;
using WpfExtensions;

namespace BassTrainer.MainApp.MainMenu
{
    public class MainScreenViewModel : BindingDataContextBase
    {
        public ICommand OpenTrainerCommand { get; private set; }
        public ICommand OpenDrumMachineCommand { get; private set; }
        public ICommand OpenMetronomeCommand { get; private set; }
        public ICommand OpenSettingsCommand { get; private set; }
        public ICommand OpenHelpCommand { get; private set; }
        public ICommand OpenTuningCommand { get; private set; }
        
        public MainScreenViewModel(IMainAppViewModel appViewModel)
        {
            OpenTrainerCommand = new DelegateCommand(appViewModel.OpenTrainer);
            OpenDrumMachineCommand = new DelegateCommand(appViewModel.OpenDrumMachine);
            OpenMetronomeCommand = new DelegateCommand(appViewModel.OpenMetronome);
            OpenSettingsCommand = new DelegateCommand(appViewModel.OpenOptions);
            OpenHelpCommand = new DelegateCommand(appViewModel.OpenHelp);
            OpenTuningCommand = new DelegateCommand(appViewModel.OpenBassTuner);
        }
    }
}