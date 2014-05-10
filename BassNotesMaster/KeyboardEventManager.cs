using System.Windows.Input;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Utils;
using BassNotesMasterApi.Utils.Keyboard;

namespace BassNotesMaster
{
    public class KeyboardEventManager : AbstractKeyboardEventManager
    {
        private readonly NotesInfo _notesInfo = new NotesInfo();
    
        public enum Modifier
        {
            LeftCtrl,
            LeftShift,
            None
        }

        private enum State
        {
            ListenToNote,
            ListenToOctave,
            FireCombinationPressed
        }

        private State _state = State.ListenToNote;
        private Note _pressedNote;

        public void OnKeyDown(object sender, KeyEventArgs eventArgs)
        {
            var keyboard = eventArgs.KeyboardDevice;
            var modifier = CheckAndGetModifier(keyboard);
            
            if (_state == State.ListenToNote)
            {
                if (!NoteExists(eventArgs.Key.ToString(), modifier))
                    return;

                _state = OctaveCheckEnabled ? State.ListenToOctave : State.FireCombinationPressed;
            }
            if (_state == State.ListenToOctave)
            {
                if (eventArgs.Key == Key.D1 || eventArgs.Key == Key.D2 || eventArgs.Key == Key.D3)
                {
                    _pressedNote.OctaveNumber = ConvertToInt(eventArgs.Key);
                    _state = State.FireCombinationPressed;
                }
                else
                {
                    _state = State.ListenToNote;
                }
            }
            if(_state == State.FireCombinationPressed)
            {
                _state = State.ListenToNote;
                FireOnCombinationPressedEvent(_pressedNote);
            }
        }

        
        private int ConvertToInt(Key key)
        {
            switch (key)
            {
                case Key.D1:
                    return 1;
                case Key.D2:
                    return 2;
                default:
                    return 3;
            }
        }

        public bool NoteExists(string noteName, Modifier modifier)
        {
            _pressedNote = CreateNote(noteName, modifier);
            return _notesInfo.NoteExists(_pressedNote);
        }

        public Note CreateNote(string noteName, Modifier modifier)
        {
            var accidental = ConvertToAccidentals(modifier);
            var noteWithoutAccidental = new Note(noteName);
            return new Note(noteWithoutAccidental, accidental);
        }

        private NotesInfo.Accidentals ConvertToAccidentals(Modifier modifier)
        {
            switch (modifier)
            {
                case Modifier.LeftCtrl:
                    return NotesInfo.Accidentals.Flat;
                case Modifier.LeftShift:
                    return NotesInfo.Accidentals.Sharp;
                default:
                    return NotesInfo.Accidentals.None;
            }
        }

        public Modifier CheckAndGetModifier(KeyboardDevice keyboardDevice)
        {
            switch (keyboardDevice.Modifiers)
            {
                case ModifierKeys.Control:
                    return Modifier.LeftCtrl;
                case ModifierKeys.Shift:
                    return Modifier.LeftShift;
                default:
                    return Modifier.None;
            }
        }
    }

}