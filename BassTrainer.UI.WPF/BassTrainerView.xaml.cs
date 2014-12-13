using BassTrainer.Core.Components;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Components.Fretboard.SelectionManager;
using BassTrainer.Core.Components.Interval;
using BassTrainer.Core.Components.Notation;
using BassTrainer.Core.Components.NotePlayer;
using BassTrainer.Core.Components.NotesView;
using BassTrainer.Core.Components.Statistics;
using BassTrainer.Core.Excercise;
using BassTrainer.Core.Settings;
using BassTrainer.UI.WPF.AppViewManager;
using BassTrainer.UI.WPF.Excercises;
using BassTrainer.UI.WPF.FretBoard;
using BassTrainer.UI.WPF.FretBoard.FretBoardView;
using BassTrainer.UI.WPF.FretBoard.SelectionManager;
using BassTrainer.UI.WPF.Intervals;
using BassTrainer.UI.WPF.Notation;
using BassTrainer.UI.WPF.NotePlayer;
using BassTrainer.UI.WPF.NotesView;
using BassTrainer.UI.WPF.Resources;
using BassTrainer.UI.WPF.ResultSerializer;
using BassTrainer.UI.WPF.ShowSelectViewManager;
using BassTrainer.UI.WPF.Statistics;

namespace BassTrainer.UI.WPF
{
    /// <summary>
    /// Interaction logic for BassTrainerView.xaml
    /// </summary>
    public partial class BassTrainerView : IComponentsViewModelsLocatorConfiguration,IViewControl
    {

        public void Configure(ComponentsViewModelsLocator componentsViewModelsLocator)
        {
            var componentLocator = ComponentsLocator.Instance;
            var settings = Settings.Instance;

            var intervalEventHandler = new WpfIntervalEventHandler(IntervalsPanel.intervalsTable);
            var intervalGuiBuilder = new WpfIntervalGuiBuilder(IntervalsPanel.InfoGrid, IntervalsPanel.ExcercisePanel, intervalEventHandler);
            var intervalComponent = new IntervalComponent(intervalGuiBuilder);
            var intervalViewModel = new IntervalViewModel(intervalComponent);

            componentsViewModelsLocator.IntervalViewModel = intervalViewModel;
            componentLocator.IntervalComponent = intervalComponent;

            var notationObjectBuilder = new WpfMusicNotationGraphicObjectsManager(NotationPanel.Notation);
            var musicNotationHandler = new WpfMusicNotationEventHandler(NotationPanel.Notation, NotationPanel.SharpNotation, NotationPanel.FlatNotation,
                                                                        notationObjectBuilder);
            var musicNotationComponent = new MusicNotationComponent(musicNotationHandler,
                                                                              notationObjectBuilder, settings);

            var musicNotationViewModel = new MusicNotationViewModel(musicNotationComponent);
            componentsViewModelsLocator.MusicNotationViewModel = musicNotationViewModel;
            componentLocator.MusicNotationComponent = musicNotationComponent;

            var guiBuilder = new FretBoardGuiBuilder(settings, FretboardControl.MainDrawingArena, FretboardControl.Container);
            var fretboard = new Core.Components.Fretboard.FretBoard(guiBuilder);
            var borderStyleCollection = BorderStyleCollection.Instance;
            var guiSelector = new GuiSelector(Settings.Instance, fretboard.FretBoardGuiBuilder);
            var mouseSelectionManager = new MouseSelectionManager(fretboard, borderStyleCollection);

            var selectionManager = new SelectionManager(guiSelector, mouseSelectionManager);
            var fretboardEventHandler = new WpfFretboardEventHandler(fretboard, selectionManager);

            var fretboardManager = new FretboardComponent(fretboard, fretboardEventHandler, selectionManager, settings);
            var fretboardViewModel = new FretboardViewModel(fretboardManager);
            componentsViewModelsLocator.FretboardViewModel = fretboardViewModel;
            componentLocator.FretboardComponent = fretboardManager;

            var notesViewEventHandler = new WpfNotesViewEventHandler();
            var notesViewGuiBuilder = new WpfNotesViewGuiBuilder(NotesViewControl.NotesViewPanel, notesViewEventHandler,
                                                                 fretboardManager);

            var notesViewComponents = new NotesViewComponent(notesViewGuiBuilder, notesViewEventHandler,
                                                                             settings, fretboardManager);

            var notesViewModel = new NotesViewModel(notesViewComponents);

            componentLocator.NotesViewComponent = notesViewComponents;
            componentsViewModelsLocator.NotesViewModel = notesViewModel;
            var playerManager = new BassNotesPlayer(settings);
            var playerViewModel = new BassNotesPlayerModelView(playerManager);
            
            componentLocator.PlayerManager = playerManager;
            
            componentsViewModelsLocator.PlayerViewModel = playerViewModel;
            PlayerControl.DataContext = playerViewModel;
         
            var guiOptionManager = new ExcerciseOptionGuiManager(ExcerciseControl.ExcerciseOptions);

            var showSelectComponent = new ShowSelectViewComponent(SelectionControl, fretboardManager);
            var showSelectViewModel = new ShowSelectViewModel(showSelectComponent);
            componentLocator.ShowSelectComponent = showSelectComponent;
            componentsViewModelsLocator.ShowSelectViewModel = showSelectViewModel;
            
            
            ComponentsLocator.Instance.ResultSerializer = new XmlResultSerializer();
         
            var showManager = new ShowManager();
            showManager.VisibilityDictionary.Add(ComponentId.Notation, NotationTab);
            showManager.VisibilityDictionary.Add(ComponentId.Fretboard, FretboardControl);
            showManager.VisibilityDictionary.Add(ComponentId.Intervals, IntervalsTab);
            showManager.VisibilityDictionary.Add(ComponentId.NotesView, NotesViewControl.NotesViewPanel);
            showManager.VisibilityDictionary.Add(ComponentId.Player, PlayerTab);
            showManager.VisibilityDictionary.Add(ComponentId.Statistic, Statistics);

            var excercisesDictionary = new ExcercisesDictionary(guiOptionManager, showManager);

            var statisticGuiManager = new StatisticGuiManager(Statistic);
            var statisticsManager = new StatisticsComponent(statisticGuiManager, excercisesDictionary);

            var statisticsViewModel = new StatisticsViewModel(statisticsManager);
            componentLocator.StatisticsComponent = statisticsManager;
            componentsViewModelsLocator.StatisticViewModel = statisticsViewModel;
            componentLocator.KeyboardEventComponent = new KeyboardEventComponent();
            componentsViewModelsLocator.KeyboardEventComponent = componentLocator.KeyboardEventComponent;
            componentsViewModelsLocator.Launcher = new ExcerciseLauncher(excercisesDictionary, statisticsManager);
            componentsViewModelsLocator.Startup();
        }
        
        public BassTrainerView()
        {
            new WpfGraphicResourceAdder();
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var locator = ComponentsViewModelsLocator.Instance;
            ExcerciseControl.SelectionControl = SelectionControl;
            Configure(locator);
            SelectionControl.FillSelectionControlsWithData();
            ExcerciseControl.FillExcercisesControls(locator.Launcher.ExcercisesDictionary.Keys);
            KeyDown += ((KeyboardEventComponent)locator.KeyboardEventComponent).OnKeyDown;
        }

    }
}
