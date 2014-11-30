using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BassNotesMasterApi.Components.Fretboard;
using BassNotesMasterApi.Components.NotesView;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Utils;
using Settings = BassNotesMasterApi.Settings.Settings;

namespace BassNotesMaster.NotesView
{
    public class WpfNotesViewGuiBuilder : INotesViewGuiBuilder
    {
        private readonly Dictionary<Note, Button> _notesButtons = new Dictionary<Note, Button>();

        public WpfNotesViewGuiBuilder(Panel container, NotesViewEventHandler eventHandler,
                                      FretboardManager fretboardManager)
        {
            eventHandler.Builder = this;
            eventHandler.FretboardManager = fretboardManager;
            AddAllButtonsFromContainerToDictionary(container, eventHandler);
        }

        private void AddAllButtonsFromContainerToDictionary(Panel container, NotesViewEventHandler eventHandler)
        {
            var buttons = container.Children.OfType<Button>();
            foreach (var button in buttons)
            {
                var noteName = button.Content.ToString();
                var note = new Note(noteName);
                _notesButtons.Add(note, button);
                button.Click += ((WpfNotesViewEventHandler) eventHandler).OnClickEventHandler;
            }
        }

        public void EnableAllButtons()
        {
            foreach (var notesButton in _notesButtons.Values)
            {
                notesButton.IsEnabled = true;
            }
        }

        public void EnableButtonsExclusive(Note[] notes)
        {
            DisableAllButtons();
            EnableButtons(notes);
        }

        public void EnableButtons(Note[] notes)
        {
            foreach (
                var buttonNotPair in
                    notes.Select(note => _notesButtons.First(x => x.Key.EqualsWithoutOctaveNumber(note))))
            {
                buttonNotPair.Value.IsEnabled = true;
            }
        }

        public void DisableAllButtons()
        {
            foreach (var notesButton in _notesButtons.Values)
            {
                notesButton.IsEnabled = false;
            }
        }

        public Note GetSenderAsNote(object sender)
        {
            var clickedButton = (Button) sender;
            return _notesButtons.FirstOrDefault(x => x.Value.Name.Equals(clickedButton.Name)).Key;
        }

        public void HandleShowChange(FretBoardOptions fretboardOptions)
        {
            switch (fretboardOptions.Show)
            {
                case FretBoardShow.Sharps:
                    foreach (var notes in _notesButtons.Keys)
                    {
                        var button = _notesButtons[notes];
                        button.Content = notes.SharpOrRegularRepresenation;
                    }
                    break;
                case FretBoardShow.Bemols:
                    foreach (var notes in _notesButtons.Keys)
                    {
                        var button = _notesButtons[notes];
                        button.Content = notes.BemolRepresenation;
                    }

                    break;
                case FretBoardShow.Mixed:
                    foreach (var notes in _notesButtons.Keys)
                    {
                        var button = _notesButtons[notes];
                        button.Content = notes.ToString();
                    }
                    break;
            }
        }

        public void ShowButtonExclusive(Note note)
        {
            HideAllButtons();
            var button = _notesButtons.First(x => x.Key.EqualsWithoutOctaveNumber(note)).Value;
            button.Visibility = Visibility.Visible;
        }

        public void HideAllButtons()
        {
            foreach (var button in _notesButtons.Values)
                button.Visibility = Visibility.Hidden;
        }

        public void ShowAllButtons()
        {
            foreach (var button in _notesButtons.Values)
                button.Visibility = Visibility.Visible;
        }

        public void SetTextForButton(Note note, Note text, bool withOctaveNumber)
        {
            var button = _notesButtons.First(x => x.Key.EqualsWithoutOctaveNumber(note)).Value;
            button.Content = FormatDependingOnShowSettings(text, withOctaveNumber);
            button.FontSize = Settings.Instance.FontSize.Value + 2;
        }

        public void RevertGui()
        {
            ShowAllButtons();
            RevertTextForAllButtons();
        }

        private void RevertTextForAllButtons()
        {
            foreach (var notesButtonPair in _notesButtons)
            {
                notesButtonPair.Value.Content = FormatDependingOnShowSettings(notesButtonPair.Key, false);
                notesButtonPair.Value.FontSize = Settings.Instance.FontSize.Value;
            }
        }

        private string FormatDependingOnShowSettings(Note text, bool withOctaveNumber)
        {
            var octaveNumberAsString = string.Empty;
            if (withOctaveNumber)
                octaveNumberAsString = text.OctaveNumber == 0
                                           ? string.Empty
                                           : text.OctaveNumber.ToString(CultureInfo.InvariantCulture);

            switch (Settings.Instance.FretBoardOptions.Value.Show)
            {
                case FretBoardShow.Sharps:
                    return text.SharpOrRegularRepresenation + octaveNumberAsString;
                case FretBoardShow.Bemols:
                    return text.BemolRepresenation + octaveNumberAsString;
                default:
                    return text.SharpOrRegularRepresenation + octaveNumberAsString + Note.PrintDelimeter +
                           text.BemolRepresenation + octaveNumberAsString;
            }
        }
    }
}
