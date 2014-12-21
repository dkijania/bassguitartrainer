using BassTrainer.Core.Components;
using BassTrainer.Core.Excercise;
using BassTrainer.Core.Utils.Keyboard;
using BassTrainer.UI.WPF.FretBoard;
using BassTrainer.UI.WPF.Intervals;
using BassTrainer.UI.WPF.Notation;
using BassTrainer.UI.WPF.NotePlayer;
using BassTrainer.UI.WPF.NotesView;
using BassTrainer.UI.WPF.ShowSelectViewManager;
using BassTrainer.UI.WPF.Statistics;

namespace BassTrainer.UI.WPF
{
    public class ComponentsViewModelsLocator : IComponentModeManager
    {
        public IntervalViewModel IntervalViewModel { get; set; }
        public MusicNotationViewModel MusicNotationViewModel { get; set; }
        public FretboardViewModel FretboardViewModel { get; set; }
        public NotesViewModel NotesViewModel { get; set; }
        public BassNotesPlayerModelView PlayerViewModel { get; set; }
        public ShowSelectViewModel ShowSelectViewModel { get; set; }
        public StatisticsViewModel StatisticViewModel { get; set; }
        public ExcerciseLauncher Launcher { get; set; }
        public AbstractKeyboardEventComponent KeyboardEventComponent { get; set; }

        private readonly ComponentsLocator _componentsLocator = ComponentsLocator.Instance;
        private static ComponentsViewModelsLocator _locator;

        public static ComponentsViewModelsLocator Instance
        {
            get { return _locator ?? (_locator = new ComponentsViewModelsLocator()); }
        }

        public bool IsCurrentExcercisePaused
        {
            get { return Launcher.CurrentExcercise.IsPaused; }
        }

        public void Startup()
        {
            _componentsLocator.Init();
            RegisterEvents();
            Launcher.RunDefaultExcercise();
        }

        public void RegisterEvents()
        {
            _componentsLocator.DoForAllPropertiesDerivedFrom<Component>(
                manager => ModeChangedEvent += manager.OnModeChanged);
        }

        public void SetNextExcercise(string name)
        {
            Launcher.SetNextExcercise(name);
        }

        public void StartNewExcercise()
        {
            var selectedItems = _componentsLocator.FretboardComponent.CurrentlySelectedPositions;
            Launcher.StopCurrentExcercise();
            Launcher.StartNewExcercise(selectedItems);
        }

        public void StopExcerciseAndStartDefault()
        {
            _componentsLocator.RemoveAllEvents();
            Mode = ComponentMode.Info;
            Launcher.StopExcerciseAndStartDefault();
        }

        public void ContinueCurrentExcercise()
        {
            var selectedItems = _componentsLocator.FretboardComponent.CurrentlySelectedPositions;
            Launcher.CurrentExcercise.Continue(selectedItems);
        }

        public void PauseCurrentExcercise()
        {
            _componentsLocator.RemoveAllEvents();
            Mode = ComponentMode.Selection;
            Launcher.CurrentExcercise.Pause();
        }

        public void InitSelection()
        {
            var setter = Instance.Launcher.NewExcercise.SelectionSetter;
            if (setter != null)
            {
                setter.InitSelection(_componentsLocator.FretboardComponent);
            }
        }

        public event ModeChanged ModeChangedEvent;

        public void OnModeChangedEvent(ComponentMode mode)
        {
            var handler = ModeChangedEvent;
            if (handler != null) handler(mode);
        }

        public delegate void ModeChanged(ComponentMode mode);


        private ComponentMode Mode
        {
            set
            {
                _mode = value;
                StartMode(_mode);
            }
            get { return _mode; }
        }
        private ComponentMode _mode;

        private void StartMode(ComponentMode mode)
        {
            OnModeChangedEvent(mode);
            if (mode == ComponentMode.Selection)
                InitSelection();
         
        }

        public void ApplyMode(ComponentMode mode)
        {
            Mode = mode;
        }

        public bool IsMode(ComponentMode mode)
        {
            return Mode.Equals(mode);
        }
    }
}