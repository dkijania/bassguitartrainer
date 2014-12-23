using System;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Const;
using BassTrainer.Core.Settings;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.NotesView
{
    public class NotesViewComponent : Component, ISettingListener
    {
        private readonly NotesViewEventHandler _eventHandler;
        private readonly NotesView _notesView;

        public NotesViewComponent(INotesViewGuiBuilder guiBuilder, NotesViewEventHandler eventHandler,
            Settings.Settings settings, FretboardComponent fretboardComponent)
        {
            _notesView = new NotesView(guiBuilder);
            _eventHandler = eventHandler;
            _eventHandler.FretboardComponent = fretboardComponent;
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
            if (!settings.FretBoardOptions.LastChangeResult)
                return;
            var fretboardOptions = settings.FretBoardOptions.Value;
            _notesView.HandleShowChange(fretboardOptions);
        }

        public void EnableNotesButtons()
        {
            _notesView.EnableAllButtons();
        }

        public void EnableNotesButtonsExlusive(Note[] notes)
        {
            _notesView.EnableButtonsExclusive(notes);
        }

        public override void RemoveAllSubscribers()
        {
            _eventHandler.ResetEvents();
        }

        public override void OnModeChanged(ComponentMode mode)
        {
            _eventHandler.Mode = mode;

            switch (mode)
            {
                case ComponentMode.Info:
                    EnableNotesButtons();
                    break;
                case ComponentMode.Selection:
                    DisableNotesButtons();
                    break;
                case ComponentMode.Excercise:
                    RemoveAllSubscribers();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        }

        public void DisableNotesButtons()
        {
            _notesView.DisableAllButtons();
        }

        public void ShowButtonExclusive(Note note)
        {
            _notesView.ShowButtonExclusive(note);
        }

        public void ShowAllButtons()
        {
            _notesView.ShowAllButtons();
        }

        public void SetTextForButton(Note note, Note text, bool withOctaveNumber)
        {
            _notesView.SetTextForButton(note, text, withOctaveNumber);
        }

        public void RevertGui()
        {
            _notesView.RevertGui();
        }
    }
}