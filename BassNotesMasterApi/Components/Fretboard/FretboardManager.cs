using BassNotesMasterApi.Settings;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Components.Fretboard
{
    public class FretboardManager : Manager, ISettingListener
    {
        public override ManagerMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                SetMode(_mode);
            }
        }

        private ManagerMode _mode;
        public FretBoard FretBoard { get; private set; }
        private FretboardEventHandler EventHandler { get; set; }
        public SelectionManager.SelectionManager SelectionManager { get; private set; }
        
        public FretboardManager(FretBoard fretBoard, FretboardEventHandler eventHandler,
                                SelectionManager.SelectionManager selectionManager, Settings.Settings settings)
        {
            FretBoard = fretBoard;
            EventHandler = eventHandler;
            SelectionManager = selectionManager;
            Subscribe(settings);
        }

        public override void OnModeChanged(ManagerMode mode)
        {
            SetMode(mode);
        }

        public override void RemoveAllSubscribers()
        {
            EventHandler.RemoveAllClickSubscribers();
        }

        public void Subscribe(Settings.Settings settings)
        {
            settings.SettingChangedEvent += OnSettingChanged;
        }

        public void OnSettingChanged(Settings.Settings settings)
        {
            if (settings.CorrectRectanglePreset.LastChangeResult
                || settings.WrongRectanglePreset.LastChangeResult
                || settings.FretBoardOptions.LastChangeResult
                || settings.FontFamilyName.LastChangeResult
                || settings.FontSize.LastChangeResult)
            {
                FretBoard.Refresh();
                SelectionManager.Reselect();
            }
        }

        public void SetMode(ManagerMode fretboardMode)
        {
            StopSelectionMode();
            StopExcerciseMode();
            StopInfoMode();
            switch (fretboardMode)
            {
                case ManagerMode.Info:
                    StartInfoMode();
                    break;
                case ManagerMode.Selection:
                    StartSelectionMode();
                    break;
                case ManagerMode.Excercise:
                    StartExcerciseMode();
                    break;
            }
            EventHandler.RaiseOnModeChangedEvent(_mode);
        }

        public void Subscribe(IFretboardListener listener)
        {
            EventHandler.Subscribe(listener);
        }

        public void Unsubscribe(IFretboardListener listener)
        {
            EventHandler.Unsubscribe(listener);
        }

        private void StartExcerciseMode()
        {
            EventHandler.SubscribeExcerciseHandlerForFretboard();
        }

        private void StopExcerciseMode()
        {
            EventHandler.UnsubscribeExcerciseHandlerForFretboard();
            FretBoard.Reset();
        }

        private void StopInfoMode()
        {
            EventHandler.UnsubscribeInfoHandlerForFretboard();
        }

        private void StartInfoMode()
        {
            EventHandler.SubscribeInfoHandlerForFretboard();
        }

        private void StartSelectionMode()
        {
            EventHandler.SubscribeSelectionHandlerForFretboard();
            var setter = ManagersLocator.Instance.Launcher.NewExcercise.SelectionSetter;
            InitSelection(setter);
        }

        public void InitSelection(ISelectionSetter setter)
        {
            if (setter != null)
            {
                setter.InitSelection(this);
            }
        }

        private void StopSelectionMode()
        {
            FretBoard.IgnoreColoring = false;
            EventHandler.UnsubscribeSelectionHandlerForFretboard();
        }

        public void ClearView()
        {
            FretBoard.ClearView();
        }
    }
}