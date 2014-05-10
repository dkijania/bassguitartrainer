using BassNotesMasterApi.Fretboard;
using BassNotesMasterApi.Settings;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.NotesView
{
    public class NotesViewManager : Manager, ISettingListener
    {
        private readonly NotesViewGuiBuilder _guiBuilder;
        private readonly NotesViewEventHandler _eventHandler;
        public FretboardManager FretboardManager { get; set; }

        public NotesViewManager(NotesViewGuiBuilder guiBuilder, NotesViewEventHandler eventHandler,
                                Settings.Settings settings,
                                FretboardManager fretboardManager)
        {
            _guiBuilder = guiBuilder;
            _eventHandler = eventHandler;
            FretboardManager = fretboardManager;
            Subscribe(settings);
        }

        public void Subscribe(INotesViewListener listener)
        {
            _eventHandler.Subscribe(listener);
        }

        public void Unsubscribe(INotesViewListener listener)
        {
            _eventHandler.Unsubscribe(listener);
        }

        public void Subscribe(Settings.Settings settings)
        {
            settings.SettingChangedEvent += OnSettingChanged;
        }

        public void OnSettingChanged(Settings.Settings settings)
        {
            if(!settings.FretBoardOptions.LastChangeResult)
                return;
            var fretboardOptions = settings.FretBoardOptions.Value;
            _guiBuilder.HandleShowChange(fretboardOptions);
        }

        public void EnableNotesButtons()
        {
            _guiBuilder.EnableAllButtons();
        }

        public void EnableNotesButtonsExlusive(Note[] notes)
        {
            _guiBuilder.EnableButtonsExclusive(notes);
        }

        public override ManagerMode Mode { get; set; }

        public override void RemoveAllSubscribers()
        {
            _eventHandler.ResetEvents();
        }

        public override void OnModeChanged(ManagerMode mode)
        {
            if (mode == ManagerMode.Excercise)
            {
                RemoveAllSubscribers();
            }else 
            {
                EnableNotesButtons();
            }
        }

        public void ShowButtonExclusive(Note note)
        {
            _guiBuilder.ShowButtonExclusive(note);
        }

        public void ShowAllButtons()
        {
            _guiBuilder.ShowAllButtons();
        }

        public void SetTextForButton(Note note, Note text, bool withOctaveNumber)
        {
         _guiBuilder.SetTextForButton(note, text,withOctaveNumber);
        }

        public void RevertGui()
        {
            _guiBuilder.RevertGui();
        }

       
    }
}