using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BassNotesMaster.WpfViews.Metronome;
using DrumMachine;
using DrumMachine.Audio;
using DrumMachine.Engine.Pattern;
using DrumMachine.TimeSignature;
using WpfExtensions;

namespace BassNotesMaster.WpfViews.DrumMachine
{
    public class DrumMachineViewModel : BindingDataContextBase
    {
        private int _tempoValue;
        private string _tempoMarker;
        public int MinimumTempo { get; private set; }
        public int MaximumTempo { get; private set; }
        public ICommand AddBarsCommand { get; private set; }
        public ICommand RemoveBarsCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        private int _noOfBars = 1;

        public ObservableCollection<string> NotesTypes
        {
            get
            {
                var array = Enum.GetNames(typeof(TimeSignatureOptions.NoteTypeEnum));
                var collection = new ObservableCollection<string>();

                foreach (string value in array)
                {
                    collection.Add(value);
                }
                return collection;
            }
        }



        private bool EnableOnPropertyChangedEvent = true;

        private string _selectedNoteType;

        public string SelectedNoteType
        {
            get { return _selectedNoteType; }
            set
            {
                _selectedNoteType = value;
                OnPropertyChanged();
                if (EnableOnPropertyChangedEvent)
                    UpdatePatternStructure();
            }
        }


        private int _measures;

        public int Measures
        {
            get { return _measures; }
            set
            {
                _measures = value;
                OnPropertyChanged();
                if (EnableOnPropertyChangedEvent)
                    UpdatePatternStructure();
            }
        }

        private readonly IPatternTilesManipulator _tilesManipulator;

        public DrumMachineViewModel(IPatternTilesManipulator tilesManipulator)
            : this()
        {
            _tilesManipulator = tilesManipulator;
            SelectedNoteType = NotesTypes[0];
        }

        public void StandardTimeChangedEventHandler(string signature)
        {
            EnableOnPropertyChangedEvent = false;
            var items = signature.Split('/');
            var note = (TimeSignatureOptions.NoteTypeEnum)int.Parse(items[1]);
            UpdatePatternStructure(int.Parse(items[0]), note);
            EnableOnPropertyChangedEvent = true;
        }


        public void PropertyChangedHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var timeSignatureModel = sender as TimeSignatureViewModel;
            if (e.PropertyName.Equals("CustomUpper") || e.PropertyName.Equals("CustomLower") ||
                e.PropertyName.Equals("UseCustomTimeSignature")
                )
            {
                EnableOnPropertyChangedEvent = false;
                UpdatePatternStructure(timeSignatureModel.CustomSignature);
                EnableOnPropertyChangedEvent = true;
            }
        }

        public int TempoValue
        {
            get { return _tempoValue; }
            set
            {
                _tempoValue = value;
                TempoMarker = "Tempo: " + _tempoValue;
                if (_drumMachineKit != null) _drumMachineKit.Tempo = _tempoValue;
                OnPropertyChanged();
            }
        }

        public string TempoMarker
        {
            get { return _tempoMarker; }
            set
            {
                _tempoMarker = value;
                OnPropertyChanged();
            }
        }

        private DrumMachineViewModel()
        {
            MinimumTempo = 20;
            MaximumTempo = 320;
            TempoValue = 120;
            AddBarsCommand = new DelegateCommand(AddBar);
            RemoveBarsCommand = new DelegateCommand(RemoveBar);
            PlayCommand = new DelegateCommand(Play);
            StopCommand = new DelegateCommand(Stop);
        }

        public void AddBar()
        {
            _tilesManipulator.AddBar();
            _noOfBars++;
        }

        public void RemoveBar()
        {
            if (_noOfBars == 1) return;
            _tilesManipulator.RemoveBar();
            --_noOfBars;
        }


        private void UpdatePatternStructure()
        {
            if (Measures == null || SelectedNoteType == null)
                return;

            EnableOnPropertyChangedEvent = false;
            _noOfBars = 1;
            UpdatePatternStructure(Measures, SelectedNoteType);
            EnableOnPropertyChangedEvent = true;
        }

        private void UpdatePatternStructure(TimeSignatureOptions timeSignature)
        {
            UpdatePatternStructure(timeSignature.NotesPerMeasure, timeSignature.NoteType);
        }

        private void UpdatePatternStructure(int measures, TimeSignatureOptions.NoteTypeEnum note)
        {
            UpdatePatternStructure(measures, (int)note);
        }

        private void UpdatePatternStructure(int measures, string note)
        {
            UpdatePatternStructure(measures,
                (int)Enum.Parse(typeof(TimeSignatureOptions.NoteTypeEnum), note));
        }

        private void UpdatePatternStructure(int measures = 0,
            int note = 0)
        {
            if (measures == 0 || note == 0)
                return;


            Measures = measures;
            SelectedNoteType = Enum.GetName(typeof(TimeSignatureOptions.NoteTypeEnum),
                (TimeSignatureOptions.NoteTypeEnum)note);
            const int shortestNote = (int)TimeSignatureOptions.NoteTypeEnum.Sixteen;
            _tilesManipulator.SetColumnsCount(measures, shortestNote / note);
        }


        public DrumPattern ToDrumPattern()
        {
            var samplesCount = DefaultAudioSampler.Instance.Samples.Count();
            var noteType =
                (TimeSignatureOptions.NoteTypeEnum)
                    Enum.Parse(typeof(TimeSignatureOptions.NoteTypeEnum), SelectedNoteType);
            var drum = new DrumPattern(samplesCount, new TimeSignatureOptions(Measures * _noOfBars, noteType));
            _tilesManipulator.FillDrumPattern(drum);
            return drum;
        }

        private DrumMachineKit _drumMachineKit;


        public void Play()
        {
            _drumMachineKit = new DrumMachineKit(ToDrumPattern()) { Tempo = TempoValue };
            _drumMachineKit.Play();
        }

        public void Stop()
        {
            if (_drumMachineKit != null)
            {
                _drumMachineKit.Stop();
            }
        }
    }
}