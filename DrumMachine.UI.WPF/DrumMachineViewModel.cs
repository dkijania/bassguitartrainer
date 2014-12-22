using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using DrumMachine.Engine.Pattern;
using DrumMachine.TimeSignature;
using DrumMachine.UI.WPF.Pattern;
using DrumMachine.UI.WPF.TimeSignature;
using Microsoft.Win32;
using WpfExtensions;

namespace DrumMachine.UI.WPF
{
    public class DrumMachineViewModel : BindingDataContextBase
    {
        private readonly DrumMachineModel _model;
        private bool _enableOnPropertyChangedEvent = true;
        private bool _isSplitModeEnabled;
        private bool _isJoinModeEnabled;
        private int _measures;
        private string _selectedNoteType;
        private string _tempoMarker;
        private int _tempoValue;

        public DrumMachineViewModel(IPatternTilesManipulator tilesManipulator,
            TimeSignatureViewModel timeSignatureViewModel)
        {
            tilesManipulator.OnSelectEvent += drumMachineTile_OnSelectEvent;
            tilesManipulator.IgnoreMouseEvent += _tilesManipulator_IgnoreMouseEvent;
            _model = new DrumMachineModel(tilesManipulator);
            timeSignatureViewModel.PropertyChanged += PropertyChangedHandler;
            timeSignatureViewModel.StandardTimeChangedEvent += StandardTimeChangedEventHandler;
            SelectedNoteType = NotesTypes[0];

            MinimumTempo = 20;
            MaximumTempo = 320;
            TempoValue = 120;
            AddBarsCommand = new DelegateCommand(AddBar);
            RemoveBarsCommand = new DelegateCommand(RemoveBar);
            PlayCommand = new DelegateCommand(Play);
            StopCommand = new DelegateCommand(Stop);
            ImportDrumPattern = new DelegateCommand(ImportDrumPatternFromFile);
            ExportDrumPattern = new DelegateCommand(ExportDrumPatternToFile);
            ClearCommand = new DelegateCommand(Clear);
        }

        private void Clear()
        {
            _model.Clear();
        }

        public int MinimumTempo { get; private set; }
        public int MaximumTempo { get; private set; }
        public ICommand AddBarsCommand { get; private set; }
        public ICommand RemoveBarsCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand PlayCommand { get; private set; }
        public ICommand ImportDrumPattern { get; private set; }
        public ICommand ExportDrumPattern { get; private set; }
        public ICommand ClearCommand { get; private set; }

        public ObservableCollection<string> NotesTypes
        {
            get
            {
                string[] array = Enum.GetNames(typeof (NoteTypeEnum));
                var collection = new ObservableCollection<string>();

                foreach (string value in array)
                {
                    collection.Add(value);
                }
                return collection;
            }
        }

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

        public bool IsSplitModeEnabled
        {
            get { return _isSplitModeEnabled; }
            set
            {
                _isSplitModeEnabled = value;
                if (_isSplitModeEnabled) IsJoinModeEnabled = false;
                OnPropertyChanged();
            }
        }

        public bool IsJoinModeEnabled
        {
            get { return _isJoinModeEnabled; }
            set
            {
                _isJoinModeEnabled = value;
                if (_isJoinModeEnabled) IsSplitModeEnabled = false;
                OnPropertyChanged();
            }
        }

        public int TempoValue
        {
            get { return _tempoValue; }
            set
            {
                _tempoValue = value;
                TempoMarker = "Tempo: " + _tempoValue;
                _model.TryToSetTempo(_tempoValue);
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

        private void drumMachineTile_OnSelectEvent(int row, int column, bool isSelected)
        {
            if (IsSplitModeEnabled)
                _model.SplitCell(row, column);
            else if (IsJoinModeEnabled)
                _model.JoinCell(row, column);
            else if (isSelected)
                _model.PlaySound(row);
        }

        private bool _tilesManipulator_IgnoreMouseEvent()
        {
            return IsSplitModeEnabled || IsJoinModeEnabled;
        }

        public void StandardTimeChangedEventHandler(string signature)
        {
            _enableOnPropertyChangedEvent = false;
            string[] items = signature.Split('/');
            var note = (NoteTypeEnum) int.Parse(items[1]);
            UpdatePatternStructure(int.Parse(items[0]), note);
            _enableOnPropertyChangedEvent = true;
        }


        public void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            var timeSignatureModel = sender as TimeSignatureViewModel;
            if (e.PropertyName.Equals("CustomUpper") || e.PropertyName.Equals("CustomLower") ||
                e.PropertyName.Equals("UseCustomTimeSignature")
                )
            {
                _enableOnPropertyChangedEvent = false;
                UpdatePatternStructure(timeSignatureModel.TimeSignature.CustomSignature);
                _enableOnPropertyChangedEvent = true;
            }
        }

        public void AddBar()
        {
            _model.AddBar();
        }

        public void RemoveBar()
        {
            _model.RemoveBar();
        }

        private void UpdatePatternStructure()
        {
            if (Measures == 0 || SelectedNoteType == null)
                return;

            _enableOnPropertyChangedEvent = false;
            UpdatePatternStructure(Measures, SelectedNoteType);
            _enableOnPropertyChangedEvent = true;
        }

        private void UpdatePatternStructure(TimeSignatureOptions timeSignature)
        {
            UpdatePatternStructure(timeSignature.NotesPerMeasure, timeSignature.NoteType);
        }

        private void UpdatePatternStructure(int measures, NoteTypeEnum note)
        {
            UpdatePatternStructure(measures, (int) note);
        }

        private void UpdatePatternStructure(int measures, string note)
        {
            UpdatePatternStructure(measures,
                (int) Enum.Parse(typeof (NoteTypeEnum), note));
        }

        private void UpdatePatternStructure(int measures = 0, int note = 0)
        {
            if (measures == 0 || note == 0)
                return;

            Measures = measures;
            SelectedNoteType = Enum.GetName(typeof (NoteTypeEnum),
                (NoteTypeEnum) note);
         _model.UpdatePatternStructure(measures,note);
        }

        public void Play()
        {
            _model.Play(TempoValue, SelectedNoteType, Measures);
        }

        public void Stop()
        {
            _model.Stop();
        }

        private void ExportDrumPatternToFile()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (!dialog.ShowDialog().GetValueOrDefault())
                return;

            _model.ExportDrumPattern(dialog.FileName, SelectedNoteType, Measures, TempoValue);
        }

        private void ImportDrumPatternFromFile()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (!dialog.ShowDialog().GetValueOrDefault())
                return;

            
            var drumSettings = _model.ImportDrumPatternAndGetNewSettings(dialog.FileName);
            UpdateControlPanel(drumSettings);
        }

        private void UpdateControlPanel(DrumPatternSettings drumPatternSettings)
        {
            _enableOnPropertyChangedEvent = false;
            Measures = drumPatternSettings.Measures;
            TempoValue = drumPatternSettings.Tempo;
            SelectedNoteType = drumPatternSettings.NoteType;
            _enableOnPropertyChangedEvent = true;
        }
    }
}