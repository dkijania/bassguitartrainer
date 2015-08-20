using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BassTrainer.MainApp.MainMenu;
using BassTrainer.UI.WPF;
using BassTuner.UI.WPF;
using DrumMachine.UI.WPF;
using Metronome.UI.WPF;
using SimpleHelpSystem.UI.WPF;
using WpfExtensions;

namespace BassTrainer.MainApp.MainContent
{
    internal class MainAppViewModel : BindingDataContextBase, IMainAppViewModel
    {
        private bool _isSettingsOpened;

        private readonly BassTrainerView _bassTrainerContent;
        private readonly MetronomeView _metronomeView;
        private readonly HelpView _helpView;
        private readonly DrumMachineView _drumMachineView;
        private readonly MainScreen _mainScreenView;
        private readonly BassTunerView _bassTunerView; 
     
        public ICommand ShowMetronome { get; private set; }
        public ICommand ShowDrumMachine { get; private set; }
        public ICommand ShowOptions { get; private set; }
        public ICommand ShowHelp { get; private set; }
        public ICommand ShowTrainer { get; private set; }
        public ICommand ShowMainMenu { get; private set; }
        public ICommand ShowBassTuner { get; private set; }
    
        public ObservableCollection<FrameworkElement> MainContentItems { get; private set; }
        
        public MainAppViewModel()
        {
             MainContentItems = new ObservableCollection<FrameworkElement>();
            _bassTrainerContent = new BassTrainerView();
            _metronomeView = new MetronomeView();
            _helpView = new HelpView();
            _drumMachineView = new DrumMachineView();
            _bassTunerView = new BassTunerView();
            _mainScreenView = new MainScreen {DataContext = new MainScreenViewModel(this)};

            ShowHelp = new DelegateCommand(OpenHelp);
            ShowMetronome = new DelegateCommand(OpenMetronome);
            ShowOptions = new DelegateCommand(OpenOptions);
            ShowDrumMachine = new DelegateCommand(OpenDrumMachine);
            ShowTrainer = new DelegateCommand(OpenTrainer);
            ShowMainMenu = new DelegateCommand(OpenMainMenu);
            ShowBassTuner = new DelegateCommand(OpenBassTuner);
            OpenDefaultView();
        }
        
        public bool IsSettingsOpened
        {
            get { return _isSettingsOpened; }
            set
            {
                _isSettingsOpened = value;
                OnPropertyChanged();
            }
        }
        
        public void OpenDefaultView()
        {
            OpenMainMenu();
        }

        public void OpenMainMenu()
        {
            MainContentItems.Clear();
            MainContentItems.Add(_mainScreenView);
        }

        public void OpenBassTuner()
        {
            MainContentItems.Clear();
            MainContentItems.Add(_bassTunerView);
        }

        public void OpenDrumMachine()
        {
            MainContentItems.Clear();
            MainContentItems.Add(_drumMachineView);
        }

        public void OpenTrainer()
        {
            MainContentItems.Clear();
            MainContentItems.Add(_bassTrainerContent);
        }

        public void OpenHelp()
        {
            MainContentItems.Clear();
            MainContentItems.Add(_helpView);
        }

        public void OpenOptions()
        {
            IsSettingsOpened = !IsSettingsOpened;
        }

        public void OpenMetronome()
        {
            MainContentItems.Clear();
            MainContentItems.Add(_metronomeView);
        }
    }
}