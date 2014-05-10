using System.Windows.Input;
using BassNotesMaster.WpfViews;
using DrumMachineView = BassNotesMaster.WpfViews.DrumMachine.DrumMachineView;
using MetronomeView = BassNotesMaster.WpfViews.Metronome.MetronomeView;
using Settings = BassNotesMasterApi.Settings.Settings;


namespace BassNotesMaster
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ICommand ShowMetronome { get; private set; }
        public ICommand ShowDrumMachine { get; private set; }
        public ICommand ShowOptions { get; private set; }
        public ICommand ShowHelp { get; private set; }
        public ICommand ShowMain { get; private set; }

        private readonly MainView _mainContent;
        private readonly MetronomeView _metronomeView;
        private readonly HelpView _helpView;
        private readonly DrumMachineView _drumMachineView;

        public MainWindow()
        {
            InitializeComponent();
            WindowsCommands.DataContext = this;
            _mainContent = new MainView {MetroWindow = this};
            _mainContent.Init();
            _metronomeView = new MetronomeView(this);
            _helpView = new HelpView(this);
            _drumMachineView = new DrumMachineView {MetroWindow = this};

            VisualSettingsControl.Init(Settings.Instance);

            ShowHelp = new DelegateCommand(OpenHelp);
            ShowMetronome = new DelegateCommand(OpenMetronome);
            ShowOptions = new DelegateCommand(OpenOptions);
            ShowDrumMachine = new DelegateCommand(OpenDrumMachine);
            ShowMain = new DelegateCommand(GotoMain);

            GotoMain();
        }

        private void OpenDrumMachine()
        {
            Main.Children.Clear();
            Main.Children.Add(_drumMachineView);
        }

        private void GotoMain()
        {
            Main.Children.Clear();
            Main.Children.Add(_mainContent);
        }

        private void GoToMetronome()
        {
            Main.Children.Clear();
            Main.Children.Add(_metronomeView);
        }

        private void GoToHelp()
        {
            Main.Children.Clear();
            Main.Children.Add(_helpView);
        }

        private void OpenOptions()
        {
            Sett.IsOpen = !Sett.IsOpen;
        }

        private void OpenMetronome()
        {
            GoToMetronome();
        }

        private void OpenHelp()
        {
            GoToHelp();
        }
    }
}