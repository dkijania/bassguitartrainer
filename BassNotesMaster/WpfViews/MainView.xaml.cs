using BassNotesMaster.AppViewManager;
using BassNotesMaster.Excercises;
using BassNotesMaster.FretBoard;
using BassNotesMaster.FretBoard.FretBoardView;
using BassNotesMaster.FretBoard.SelectionManager;
using BassNotesMaster.Intervals;
using BassNotesMaster.Notation;
using BassNotesMaster.NotePlayer;
using BassNotesMaster.NotesView;
using BassNotesMaster.Resources;
using BassNotesMaster.ResultSerializer;
using BassNotesMaster.SettingsManager;
using BassNotesMaster.Statistics;
using BassNotesMasterApi;
using BassNotesMasterApi.Excercise;
using BassNotesMasterApi.Fretboard;
using BassNotesMasterApi.Fretboard.SelectionManager;
using BassNotesMasterApi.Interval;
using BassNotesMasterApi.Notation;
using BassNotesMasterApi.NotesView;
using BassNotesMasterApi.Settings;
using BassNotesMasterApi.Statistics;
using BassNotesMasterApi.Utils;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace BassNotesMaster.WpfViews
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : IManagersLocatorConfiguration,IViewControl
    {
        public MetroWindow MetroWindow { get; set; }
    
     
        public void Configure(ManagersLocator managersLocator)
        {
            var window = MetroWindow;
            ExcerciseControl.Window = window;
            Statistic.Window = window;
            var settings = Settings.Instance;

            var intervalEventHandler = new WpfIntervalEventHandler(IntervalsPanel.intervalsTable, window);
            var intervalGuiBuilder = new WpfIntervalGuiBuilder(IntervalsPanel.InfoGrid, IntervalsPanel.ExcercisePanel, intervalEventHandler);
            ManagersLocator.Instance.IntervalManager = new IntervalManager(intervalGuiBuilder);

            var notationObjectBuilder = new WpfMusicNotationGraphicObjectsManager(NotationPanel.Notation);
            var musicNotationHandler = new WpfMusicNotationEventHandler(NotationPanel.Notation, NotationPanel.SharpNotation, NotationPanel.FlatNotation,
                                                                        notationObjectBuilder);
            ManagersLocator.Instance.MusicNotationManager = new MusicNotation(musicNotationHandler,
                                                                              notationObjectBuilder, settings);

            var fretBoardModel = new FretBoardModel();
            var guiBuilder = new FretBoardGuiBuilder(settings, FretboardControl.MainDrawingArena, FretboardControl.Container, fretBoardModel);
            var fretboard = new BassNotesMasterApi.Fretboard.FretBoard(guiBuilder);
            var borderStyleCollection = BorderStyleCollection.Instance;
            var guiSelector = new GuiSelector(Settings.Instance, fretboard.FretBoardGuiBuilder);
            var mouseSelectionManager = new MouseSelectionManager(fretboard, borderStyleCollection);

            var selectionManager = new SelectionManager(guiSelector, mouseSelectionManager);
            var fretboardEventHandler = new WpfFretboardEventHandler(fretboard, selectionManager);

            var fretboardManager = new FretboardManager(fretboard, fretboardEventHandler, selectionManager, settings);
            ManagersLocator.Instance.FretboardManager = fretboardManager;

            var notesViewEventHandler = new WpfNotesViewEventHandler();
            var notesViewGuiBuilder = new WpfNotesViewGuiBuilder(NotesViewControl.NotesViewPanel, notesViewEventHandler,
                                                                 fretboardManager);

            ManagersLocator.Instance.NotesViewManager = new NotesViewManager(notesViewGuiBuilder, notesViewEventHandler,
                                                                             settings,
                                                                             fretboardManager);
            ManagersLocator.Instance.PlayerManager = new BassNotesPlayer(PlayerControl.PlayAgain, PlayerControl.Volume, PlayerControl.Mute, settings);
            var guiOptionManager = new ExcerciseOptionGuiManager(ExcerciseControl.ExcerciseOptions);

            ManagersLocator.Instance.ShowSelectManager = new ShowSelectViewManager.ShowSelectViewManager(SelectionControl, fretboardManager);
            ManagersLocator.Instance.ResultSerializer = new XmlResultSerializer();
         
            var showManager = new ShowManager();
            showManager.VisibilityDictionary.Add(ComponentId.Notation, NotationTab);
            showManager.VisibilityDictionary.Add(ComponentId.Fretboard, FretboardControl);
            showManager.VisibilityDictionary.Add(ComponentId.Intervals, IntervalsTab);
            showManager.VisibilityDictionary.Add(ComponentId.NotesView, NotesViewControl.NotesViewPanel);
            showManager.VisibilityDictionary.Add(ComponentId.Player, PlayerTab);
            showManager.VisibilityDictionary.Add(ComponentId.Statistic, Statistics);

            var excercisesDictionary = new ExcercisesDictionary(guiOptionManager, showManager);

            var statisticGuiManager = new StatisticGuiManager(Statistic);
            var statisticsManager = new StatisticsManager(statisticGuiManager, excercisesDictionary);
            ManagersLocator.Instance.StatisticsManager = statisticsManager;
            ManagersLocator.Instance.KeyboardEventManager = new KeyboardEventManager();
            ManagersLocator.Instance.Launcher = new ExcerciseLauncher(excercisesDictionary, statisticsManager);
            ManagersLocator.Instance.Startup();
        }

        public void Init()
        {
            ExcerciseControl.SelectionControl = SelectionControl;
            var settingsConfigurator = new DotNetSettingsConfigurator();
            settingsConfigurator.Read(Settings.Instance);
            Configure(ManagersLocator.Instance);
            SelectionControl.FillSelectionControlsWithData();
            ExcerciseControl.FillExcercisesControls(ManagersLocator.Instance.Launcher.ExcercisesDictionary.Keys);
            KeyDown += ((KeyboardEventManager)ManagersLocator.Instance.KeyboardEventManager).OnKeyDown;
        }

        public MainView()
        {
            new WpfGraphicResourceAdder();
            InitializeComponent();
         }
    }
}
