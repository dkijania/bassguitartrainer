using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.Notation
{
    public class MusicNotationComponent : Component
    {
        private readonly MusicNotationEventHandler _musicNotationEventHandler;
        private readonly MusicNotation _musicNotation;
        
        public MusicNotationComponent(MusicNotationEventHandler musicNotationEventHandler,
                             MusicNotationGraphicObjectsManager wpfMusicNotationGraphicObjectsManager,
                             Settings.Settings settings)
        {
            _musicNotationEventHandler = musicNotationEventHandler;
            _musicNotationEventHandler.MusicNotation = _musicNotation;
            _musicNotation = new MusicNotation(wpfMusicNotationGraphicObjectsManager,settings);
        }

        public void Subscribe(IMusicNotationListener listener)
        {
            _musicNotation.OnMouseClickEvent += listener.OnMouseClick;
        }

        public void Unsubscribe(IMusicNotationListener listener)
        {
            _musicNotation.OnMouseClickEvent -= listener.OnMouseClick;
        }
        
        public override void OnModeChanged(ComponentMode mode)
        {
            _musicNotationEventHandler.UnregisterEventsForButtons();
            _musicNotationEventHandler.UnregisterEventsForCanvas();
            switch (mode)
            {
                case ComponentMode.Info:
                    _musicNotationEventHandler.RegisterEventsForButtons();
                    _musicNotationEventHandler.RegisterEventsForCanvas();
                    break;
            }
        }

        public override void Init()
        {
            _musicNotation.ResetGui();
        }

        public override void RemoveAllSubscribers()
        {
            _musicNotationEventHandler.ClearAllEvents();
            _musicNotation.ResetMouseEvent();
        }

        public void ResetGui()
        {
            _musicNotation.ResetGui();
        }

        public void RedrawNote(StringFretPair stringFretPair, bool result)
        {
            _musicNotation.RedrawNote(stringFretPair,result);
        }

        public void RedrawNote(StringFretPair stringFretToFind)
        {
            _musicNotation.RedrawNote(stringFretToFind);
        }
    }
}