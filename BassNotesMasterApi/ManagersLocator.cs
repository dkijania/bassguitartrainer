using System;
using System.Linq;
using System.Reflection;
using BassNotesMasterApi.Fretboard;
using BassNotesMasterApi.Interval;
using BassNotesMasterApi.NotePlayer;
using BassNotesMasterApi.NotesView;
using BassNotesMasterApi.Statistics;
using BassNotesMasterApi.Utils;
using BassNotesMasterApi.Utils.Keyboard;
using BassNotesMasterApi.Utils.ResultSerializer;

namespace BassNotesMasterApi
{
    public class ManagersLocator
    {
        public FretboardManager FretboardManager { get; set; }
        public IntervalManager IntervalManager { get; set; }
        public Notation.MusicNotation MusicNotationManager { get; set; }
        public NotesViewManager NotesViewManager { get; set; }
        public Manager ShowSelectManager { get; set; }
        public BassNotesPlayer PlayerManager { get; set; }
        public ExcerciseLauncher Launcher { get; set; }
        public IResultSerializer ResultSerializer { get; set; }
        public AbstractKeyboardEventManager KeyboardEventManager { get; set; }
        public StatisticsManager StatisticsManager { get; set; }

        private static ManagersLocator _locator;
        public event ModeChanged ModeChangedEvent;

        public void OnModeChangedEvent(ManagerMode mode)
        {
            var handler = ModeChangedEvent;
            if (handler != null) handler(mode);
        }

        public delegate void ModeChanged(ManagerMode mode);

        private ManagersLocator()
        {
        }

        public static ManagersLocator Instance
        {
            get { return _locator ?? (_locator = new ManagersLocator()); }
        }

        public void RemoveAllEvents()
        {
            DoForAllPropertiesDerivedFrom<Manager>(manager => manager.RemoveAllSubscribers());
        }

        private void CheckIfAllPropertiesAreNotNull()
        {
            DoForAllPropertiesDerivedFrom<Manager>(CheckIfNotNull);
        }

        private void CheckIfNotNull(Manager manager)
        {
            if (manager == null)
            {
                throw new ManagersLocatorException("property of type " + typeof (Manager) +
                                                   " wasn't set during configuration.");
            }
        }

        public ManagerMode Mode
        {
            set
            {
                _mode = value;
                StartMode(_mode);
            }
            get { return _mode; }
        }

        public bool IsCurrentExcercisePaused
        {
            get { return Launcher.CurrentExcercise.IsPaused; }
        }

        private ManagerMode _mode;

        private void StartMode(ManagerMode mode)
        {
            OnModeChangedEvent(mode);
        }

        private void DoForAllPropertiesDerivedFrom<T>(Action<T> action)
        {
            foreach (
                var propertyInfo in
                    GetType().GetProperties())
            {
                var type = propertyInfo.PropertyType;

                if (typeof (T).IsAssignableFrom(type))
                {
                    var manager = propertyInfo.GetValue(this);
                    action((T) manager);
                }
            }
        }

        public void Startup()
        {
            CheckIfAllPropertiesAreNotNull();
            DoForAllPropertiesDerivedFrom<Manager>(manager => manager.Init());
            RegisterEvents();
            Launcher.RunDefaultExcercise();
        }

        public void RegisterEvents()
        {
            DoForAllPropertiesDerivedFrom<Manager>(manager => ModeChangedEvent += manager.OnModeChanged);
        }

        public void SetNextExcercise(string name)
        {
            Launcher.SetNextExcercise(name);
        }

        public void StartNewExcercise()
        {
            var selectedItems = FretboardManager.SelectionManager.Selected.ToArray();
            Launcher.StopCurrentExcercise();
            Launcher.StartNewExcercise(selectedItems);
        }

        public void StopExcerciseAndStartDefault()
        {
            RemoveAllEvents();
            Mode = ManagerMode.Info;
            Launcher.StopExcerciseAndStartDefault();
        }

        public void ContinueCurrentExcercise()
        {
            var selectedItems = FretboardManager.SelectionManager.Selected.ToArray();
            Launcher.CurrentExcercise.Continue(selectedItems);
        }

        public void PauseCurrentExcercise()
        {
            RemoveAllEvents();
            Mode = ManagerMode.Selection;
            Launcher.CurrentExcercise.Pause();
        }
    }
}