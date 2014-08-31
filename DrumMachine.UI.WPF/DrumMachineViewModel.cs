using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DrumMachine.Audio;
using DrumMachine.Engine.Pattern;
using DrumMachine.TimeSignature;
using DrumMachine.UI.WPF.TimeSignature;
using WpfExtensions;

namespace DrumMachine.UI.WPF
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
                var array = Enum.GetNames(typeof(NoteTypeEnum));
                var collection = new ObservableCollection<string>();

                foreach (string value in array)
                {
                    collection.Add(value);
                }
                return collection;
            }
        }



        private bool _enableOnPropertyChangedEvent = true;

        private string _selectedNoteType;

        public string SelectedNoteType
        {
            get { return _selectedNoteType; }
            set
            {
                _selectedNoteType = value;
                OnPropertyChanged();
                if (_enableOnPropertyChangedEvent)
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
                if (_enableOnPropertyChangedEvent)
                    UpdatePatternStructure();
            }
        }

        private readonly IPatternTilesManipulator _tilesManipulator;

        public DrumMachineViewModel(IPatternTilesManipulator tilesManipulator, TimeSignatureViewModel timeSignatureViewModel)
            : this()
        {
            _tilesManipulator = tilesManipulator;
            timeSignatureViewModel.PropertyChanged += PropertyChangedHandler;
            timeSignatureViewModel.StandardTimeChangedEvent += StandardTimeChangedEventHandler;
            SelectedNoteType = NotesTypes[0];
        }

        public void StandardTimeChangedEventHandler(string signature)
        {
            _enableOnPropertyChangedEvent = false;
            var items = signature.Split('/');
            var note = (NoteTypeEnum)int.Parse(items[1]);
            UpdatePatternStructure(int.Parse(items[0]), note);
            _enableOnPropertyChangedEvent = true;
        }


        public void PropertyChangedHandler(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var timeSignatureModel = sender as TimeSignatureViewModel;
            if (e.PropertyName.Equals("CustomUpper") || e.PropertyName.Equals("CustomLower") ||
                e.PropertyName.Equals("UseCustomTimeSignature")
                )
            {
                _enableOnPropertyChangedEvent = false;
                UpdatePatternStructure((TimeSignatureOptions) timeSignatureModel.TimeSignature.CustomSignature);
                _enableOnPropertyChangedEvent = true;
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

            _enableOnPropertyChangedEvent = false;
            _noOfBars = 1;
            UpdatePatternStructure(Measures, SelectedNoteType);
            _enableOnPropertyChangedEvent = true;
        }

        private void UpdatePatternStructure(TimeSignatureOptions timeSignature)
        {
            UpdatePatternStructure((int) timeSignature.NotesPerMeasure, (NoteTypeEnum) timeSignature.NoteType);
        }

        private void UpdatePatternStructure(int measures, NoteTypeEnum note)
        {
            UpdatePatternStructure(measures, (int)note);
        }

        private void UpdatePatternStructure(int measures, string note)
        {
            UpdatePatternStructure(measures,
                (int)Enum.Parse(typeof(NoteTypeEnum), note));
        }

        private void UpdatePatternStructure(int measures = 0,int note = 0)
        {
            if (measures == 0 || note == 0)
                return;


            Measures = measures;
            SelectedNoteType = Enum.GetName(typeof(NoteTypeEnum),
                (NoteTypeEnum)note);
            const int shortestNote = (int)NoteTypeEnum.Sixteen;
            _tilesManipulator.SetColumnsCount(measures, shortestNote / note);
        }


        public DrumPattern ToDrumPattern()
        {
            var samplesCount = DefaultAudioSampler.Instance.Samples.Count();
            var noteType =
                (NoteTypeEnum)
                    Enum.Parse(typeof(NoteTypeEnum), SelectedNoteType);
            var drum = new DrumPattern(samplesCount, new TimeSignatureOptions(Measures * _noOfBars, noteType));
            _tilesManipulator.FillDrumPattern(drum);
            return drum;
        }

        private DrumMachineKit _drumMachineKit;


        public void Play()
        {
            _drumMachineKit = new DrumMachineKit(ToDrumPattern()) { Tempo = TempoValue };
            _drumMachineKit.OnBeatHitEvent += _tilesManipulator.PatternHighlighter.HighlightColumnOnBeat;
            _drumMachineKit.Play();

        }

        public void Stop()
        {
            if (_drumMachineKit != null)
            {
                _drumMachineKit.Stop();
            }
            _tilesManipulator.PatternHighlighter.CleanUpHighlight();
        }
    }
}