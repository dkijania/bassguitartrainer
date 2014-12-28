using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using BassTrainer.Core.Settings;
using BassTrainer.UI.WPF;
using BassTrainer.UI.WPF.SettingsManager;
using BassTuner.UI.WPF;
using DrumMachine.UI.WPF;
using Metronome.UI.WPF;
using SimpleHelpSystem.UI.WPF;
using WpfExtensions;

namespace BassTrainer.MainApp
{
    internal class MainAppViewModel : BindingDataContextBase
    {
        private bool _isSettingsOpened;

        private readonly BassTrainerView _bassTrainerContent;
        private readonly MetronomeView _metronomeView;
        private readonly HelpView _helpView;
        private readonly DrumMachineView _drumMachineView;
                
        //Not implemented yet. Disabled in xaml
        private readonly BassTunerView _bassTunerView; 
     
        public ICommand ShowMetronome { get; private set; }
        public ICommand ShowDrumMachine { get; private set; }
        public ICommand ShowOptions { get; private set; }
        public ICommand ShowHelp { get; private set; }
        public ICommand ShowMain { get; private set; }
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
            
            ShowHelp = new DelegateCommand(OpenHelp);
            ShowMetronome = new DelegateCommand(OpenMetronome);
            ShowOptions = new DelegateCommand(OpenOptions);
            ShowDrumMachine = new DelegateCommand(OpenDrumMachine);
            ShowMain = new DelegateCommand(OpenTrainer);
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
        
        private void OpenDefaultView()
        {
            OpenTrainer();
        }


        private void OpenBassTuner()
        {
            MainContentItems.Clear();
            MainContentItems.Add(_bassTunerView);
        }
      
        private void OpenDrumMachine()
        {
            MainContentItems.Clear();
            MainContentItems.Add(_drumMachineView);
        }

        private void OpenTrainer()
        {
            MainContentItems.Clear();
            MainContentItems.Add(_bassTrainerContent);
        }
        
        private void OpenHelp()
        {
            MainContentItems.Clear();
            MainContentItems.Add(_helpView);
        }

        private void OpenOptions()
        {
            IsSettingsOpened = !IsSettingsOpened;
        }

        private void OpenMetronome()
        {
            MainContentItems.Clear();
            MainContentItems.Add(_metronomeView);
        }
    }
}