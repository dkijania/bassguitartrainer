using System.Linq;
using BassTrainer.Core.Const;
using BassTrainer.Core.Settings;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.Fretboard
{
    public class FretboardComponent : Component, ISettingListener
    {
        public FretBoard FretBoard { get; private set; }
        private FretboardEventHandler EventHandler { get; set; }
        public SelectionManager.SelectionManager SelectionManager { get; private set; }

        public StringFretPair[] CurrentlySelectedPositions
        {
            get { return SelectionManager.Selected.ToArray(); }
        }

        public FretboardComponent(FretBoard fretBoard, FretboardEventHandler eventHandler,
                                SelectionManager.SelectionManager selectionManager, Settings.Settings settings)
        {
            FretBoard = fretBoard;
            EventHandler = eventHandler;
            SelectionManager = selectionManager;
            Subscribe(settings);
        }

        public override void OnModeChanged(ComponentMode mode)
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

        public void SetMode(ComponentMode fretboardMode)
        {
            StopSelectionMode();
            StopExcerciseMode();
            StopInfoMode();
            switch (fretboardMode)
            {
                case ComponentMode.Info:
                    StartInfoMode();
                    break;
                case ComponentMode.Selection:
                    StartSelectionMode();
                    break;
                case ComponentMode.Excercise:
                    StartExcerciseMode();
                    break;
            }
            EventHandler.RaiseOnModeChangedEvent(fretboardMode);
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