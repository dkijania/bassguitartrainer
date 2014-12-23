using BassTrainer.Core.Components;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Const;
using BassTrainer.Core.Settings;
using BassTrainer.UI.WPF.FretBoard.SelectionManager;

namespace BassTrainer.UI.WPF.ShowSelectViewManager
{
    public class ShowSelectViewComponent : Component
    {
        private readonly FretboardComponent _fretboardComponent;

        public event ShowSelectViewComponentModeChangedEvent ComponentModeChangedEvent;

        public delegate void ShowSelectViewComponentModeChangedEvent(ComponentMode mode);

        protected virtual void OnShowSelectViewComponentModeChangedEvent(ComponentMode mode)
        {
            var handler = ComponentModeChangedEvent;
            if (handler != null) handler(mode);
        }

        public ShowSelectViewComponent(FretboardComponent fretboardComponent)
        {
            _fretboardComponent = fretboardComponent;
        }

        public override void Init()
        {
            OnInfo();
        }

        public override void RemoveAllSubscribers()
        {
        }

        public override void OnModeChanged(ComponentMode mode)
        {
            switch (mode)
            {
                case ComponentMode.Info:
                    OnInfo();
                    break;
                case ComponentMode.Selection:
                    OnSelect();
                    break;
            }
            OnShowSelectViewComponentModeChangedEvent(mode);
        }
        
        protected void OnSelect()
        {
            _fretboardComponent.SelectionManager.GuiSelector = new GuiSelector(Settings.Instance,
                _fretboardComponent.FretBoard.
                    FretBoardGuiBuilder);
        }

        protected void OnInfo()
        {
            _fretboardComponent.SelectionManager.GuiSelector =
                new GuiShowSelector(_fretboardComponent.FretBoard.FretBoardGuiBuilder);
        }

        public void SelectItems(string stringName, int startFret, int endFret)
        {
            _fretboardComponent.SelectionManager.SelectItems(stringName, startFret, endFret);
        }

        public void SelectItems(string scaleType, Note rootNote, int startFret, int endFret)
        {
            _fretboardComponent.SelectionManager.SelectItems(scaleType, rootNote, (int) startFret, (int) endFret);
        }

        public void SelectScale(string scaleType, Note rootNote, int notePosition, string fingeringStyle)
        {
            _fretboardComponent.SelectionManager.SelectScale(scaleType, rootNote, notePosition, fingeringStyle);
        }

        public void ReselectAll()
        {
            Clear();
            SelectAll();
        }

        public void Clear()
        {
            _fretboardComponent.FretBoard.ForceClearView();
        }

        public void UnselectAll()
        {
            _fretboardComponent.SelectionManager.UnselectAllItems();
        }

        public void SelectAll()
        {
            _fretboardComponent.SelectionManager.SelectAllItems();
        }
    }
}