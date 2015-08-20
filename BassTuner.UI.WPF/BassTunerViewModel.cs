using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BassTrainer.Core.Components.NotePlayer;
using BassTrainer.Core.Const;
using BassTrainer.Core.Const.BassGuitar.Parameters;
using BassTrainer.Core.Const.BassGuitar.Parameters.Tuning;
using BassTrainer.Core.Settings;
using WpfExtensions;

namespace BassTuner.UI.WPF
{
    public class BassTunerViewModel : BindingDataContextBase
    {
        private TuningSounds _currentlyActiveInstrumentTuning;
        private readonly BassGuitarTypes _bassGuitarTypes = new BassGuitarTypes();
        private readonly BassNotesPlayer _player = new BassNotesPlayer(Settings.Instance);
        private Note _lastPlayedNote;

        private String _currentlyActiveInstrumentType;
        private bool _instrumentTuningVisible;
        private bool _playerButtonsVisibility;
        private String _startStopContent;

        public BassGuitar4StringTuning InstrumentTunings { get; private set; }
        public IList<String> InstrumentsTypes { get; private set; }

        public ICommand SetActiveInstrumentTypeCommand { get; private set; }
        public ICommand SetActiveInstrumentTuningCommand { get; private set; }
        public ICommand PlayNoteCommand { get; private set; }
        public ICommand PlayStopSoundCommand { get; private set; }
        public ICommand PlayPrevSoundCommand { get; private set; }
        public ICommand PlayNextSoundCommand { get; private set; }

        public BassTunerViewModel()
        {
            PlayerButtonsVisibility = false;
            InstrumentsTypes = new List<string>(_bassGuitarTypes.Values);
            SetActiveInstrumentTypeCommand = new DelegateCommand(SetActiveInstrumentType);
            SetActiveInstrumentTuningCommand = new DelegateCommand(SetActiveInstrumentTuning);
            PlayStopSoundCommand = new RelayCommand(PlayStopNote);
            PlayPrevSoundCommand = new DelegateCommand(PlayPrevNote);
            PlayNextSoundCommand = new DelegateCommand(PlayNextNote);
            PlayNoteCommand = new RelayCommand(PlayNote);
            InstrumentTunings = new BassGuitar4StringTuning();
            StopStartContent = Properties.Resources.Start;
        }

        private void SetActiveInstrumentType()
        {
            CurrentlyActiveInstrumentType = InstrumentsTypes[0];
            SetActiveInstrumentTuning();
            InstrumentTuningVisible = true;
            PlayerButtonsVisibility = true;
        }

        private void SetActiveInstrumentTuning()
        {
            CurrentlyActiveInstrumentTuning = InstrumentTunings[0];
        }

        private void PlayStopNote(object note)
        {
            if (_player.IsPlaying())
            {
                _player.Stop();
                StopStartContent = Properties.Resources.Start;
                return;
            }
            RepeatLastNote();
        }

        private void PlayNote(object note)
        {
            StopStartContent = Properties.Resources.Stop;
            var noteToPlay = note as Note;
            SaveLastPlayedNote(noteToPlay);
            _player.PlayNoteContinuosly(UpdateAnimation, noteToPlay);
        }

        private void UpdateAnimation()
        {
            SaveLastPlayedNote(LastPlayedNote);
        }

        private void SaveLastPlayedNote(Note note)
        {
            LastPlayedNote = note;
        }

        private void RepeatLastNote()
        {
            var noteToPlay = _lastPlayedNote ?? CurrentlyActiveInstrumentTuning.Notes.First();
            PlayNote(noteToPlay);
        }

        private void PlayNextNote()
        {
            var note = CurrentlyActiveInstrumentTuning.GetNextTo(_lastPlayedNote);
            PlayNote(note);
        }

        private void PlayPrevNote()
        {
            var note = CurrentlyActiveInstrumentTuning.GetPrevTo(_lastPlayedNote);
            PlayNote(note);
        }
        
        public bool PlayerButtonsVisibility
        {
            get { return _playerButtonsVisibility; }
            set
            {
                _playerButtonsVisibility = value;
                OnPropertyChanged();
            }
        }

        public string CurrentlyActiveInstrumentType
        {
            get { return _currentlyActiveInstrumentType; }
            set
            {
                _currentlyActiveInstrumentType = value;
                OnPropertyChanged();
            }
        }

        public TuningSounds CurrentlyActiveInstrumentTuning
        {
            get { return _currentlyActiveInstrumentTuning; }
            set
            {
                _currentlyActiveInstrumentTuning = value;
                OnPropertyChanged();
            }
        }

        public bool InstrumentTuningVisible
        {
            get { return _instrumentTuningVisible; }
            set
            {
                _instrumentTuningVisible = value;
                OnPropertyChanged();
            }
        }

        public string PrevContent
        {
            get { return Properties.Resources.Previous; }
        }

        public string NextContent
        {
            get { return Properties.Resources.Next; }
        }

        public string StopStartContent
        {
            get { return _startStopContent; }
            set
            {
                _startStopContent = value;
                OnPropertyChanged();
            }
        }

        public Note LastPlayedNote
        {
            get { return _lastPlayedNote; }
            set
            {
                _lastPlayedNote = value;
                OnPropertyChanged();
            }
        }
    }
}