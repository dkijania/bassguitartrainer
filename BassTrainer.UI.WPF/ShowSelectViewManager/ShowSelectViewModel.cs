using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BassTrainer.Core.Components;
using BassTrainer.Core.Components.Fretboard.SelectionManager;
using BassTrainer.Core.Const;
using BassTrainer.Core.Settings;
using BassTrainer.UI.WPF.WpfControls;
using WpfExtensions;

namespace BassTrainer.UI.WPF.ShowSelectViewManager
{
    public class ShowSelectViewModel : BindingDataContextBase
    {
        private const string ShowHeader = "Show";
        private const string SelectHeader = "Select";
        private const string ShowLabel = "ShowAll";
        private const string HideLabel = "HideAll";
        private const string SelectLabel = "SelectAll";
        private const string UnselectLabel = "UnselectAll";
        
        private readonly ShowSelectViewComponent _selectViewComponent;
        private readonly SelectionControl _selectionControl;
        private bool _isEnabled;
        private bool _enableAllNotes;
        private string _headerLabel;
        
        private string _currentShowSelectLabel;
        private string _currentHideUnselectLabel;
        
        private string _selectedStringName;
        private int _selectedStartFret;
        private int _selectedEndFret;

        private bool _isScaleStartFretEnabled;
        private bool _isScaleEndFretEnabled;
        private bool _isScalePositionEnabled;
        private bool _isScaleFingeringEnabled;

        private string _selectedScaleType;
        private string _selectedRootNote;
        private int _selectedScaleStartFret;
        private int _selectedScaleEndFret;
        private int _selectedScalePosition;
        private string _selectedScaleFingeringStyle;

        private bool _isColorOctaveEnabled;
        private bool _isColorEquivalentEnabled;

        public ICommand SelectStringRangeCommand { get; private set; }
        public ICommand AddScaleSelectionCommand { get; private set; }
        
        public ShowSelectViewModel(ShowSelectViewComponent selectViewComponent, SelectionControl selectionControl)
        {
            _selectViewComponent = selectViewComponent;
            _selectionControl = selectionControl;
            _selectViewComponent.ComponentModeChangedEvent += ComponentModeChangedEvent;
            SelectStringRangeCommand = new DelegateCommand(SelectStringRange);
            AddScaleSelectionCommand = new DelegateCommand(AddScaleSelection);
            FillSelectionControlsWithData();
        }

        public void FillSelectionControlsWithData()
        {
            EnableAllNotes = false;
            SelectedStringName = Strings.First();
            SelectedScalePosition = ScalePositions.First();
            SelectedScaleFingeringStyle = SelectionFingeringStyles.First();
            SelectedScaleType = ScaleTypes.First();
            SelectedRootNote = RootNotes.First();
        }


        private void ComponentModeChangedEvent(ComponentMode mode)
        {
            RemoveAllEvents();
            switch (mode)
            {
                case ComponentMode.Info:
                   PrepareShowPanel();
                    break;
                case ComponentMode.Selection:
                   PrepareSelectPanel();
                     break;
                case ComponentMode.Excercise:
                    IsEnabled = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("mode");
            }
        }

        protected void PrepareSelectPanel()
        {
            IsEnabled = true;
            SetSelectTabHeader();
            SelectShowAllLabel = SelectLabel;
            UnSelectHideAllLabel = UnselectLabel;
            _selectionControl.SelectAll.Click += SelectAllClick;
            _selectionControl.UnselectAll.Click += UnselectAllClick;
        }

        protected void PrepareShowPanel()
        {
            IsEnabled = true;
            SetShowTabHeader();
            SelectShowAllLabel = ShowLabel;
            UnSelectHideAllLabel = HideLabel;
            _selectionControl.SelectAll.Click += ShowAllClick;
            _selectionControl.UnselectAll.Click += HideAllClick;
        }


        protected void RemoveAllEvents()
        {
            _selectionControl.SelectAll.Click -= SelectAllClick;
            _selectionControl.UnselectAll.Click -= UnselectAllClick;
            _selectionControl.SelectAll.Click -= ShowAllClick;
            _selectionControl.UnselectAll.Click -= HideAllClick;
        }

        private void SelectAllClick(object sender, RoutedEventArgs e)
        {
            _selectViewComponent.SelectAll();
        }

        private void UnselectAllClick(object sender, RoutedEventArgs e)
        {
            _selectViewComponent.UnselectAll();
        }

        private void ShowAllClick(object sender, RoutedEventArgs e)
        {
            _selectViewComponent.ReselectAll();
        }

        private void HideAllClick(object sender, RoutedEventArgs e)
        {
            _selectViewComponent.Clear();
        }

       

        private void SetShowTabHeader()
        {
            SetParentTabHeader(ShowHeader);
        }

        private void SetSelectTabHeader()
        {
            SetParentTabHeader(SelectHeader);
        }

        private void SetParentTabHeader(string label)
        {
            ((TabItem) _selectionControl.Parent).Header = label;
        }

        private void SelectStringRange()
        {
            _selectViewComponent.SelectItems(SelectedStringName, SelectedStartFret, SelectedEndFret);
        }

        private void EableAllNotes()
        {
            if (EnableAllNotes)
            {
                IsScaleStartFretEnabled = true;
                IsScaleEndFretEnabled = true;
                IsScalePositionEnabled = false;
                IsScaleFingeringEnabled = false;
            }
            else
            {
                IsScaleStartFretEnabled = false;
                IsScaleEndFretEnabled = false;
                IsScalePositionEnabled = true;
                IsScaleFingeringEnabled = true;
            }
        }

        private void AddScaleSelection()
        {
            var scaleType = SelectedScaleType;
            var rootNote = new Note(SelectedRootNote);

            if (EnableAllNotes)
            {
                var startFret = SelectedScaleStartFret;
                var endFret = SelectedScaleEndFret;
                _selectViewComponent.SelectItems(scaleType, rootNote, startFret, endFret);
            }
            else
            {
                var notePosition = SelectedScalePosition;
                var fingeringStyle = SelectedScaleFingeringStyle;
                _selectViewComponent.SelectScale(scaleType, rootNote, notePosition, fingeringStyle);
            }
        }

        private void ColorOctaveButton(bool value)
        {
            ComponentsLocator.Instance.FretboardComponent.FretBoard.FretBoardGuiBuilder.ApplyColorForOctaves = value;
        }

        private void ColorEquivalentNotes(bool value)
        {
            ComponentsLocator.Instance.FretboardComponent.FretBoard.FretBoardGuiBuilder.ApplyColorForNotes = value;
        }

        public IEnumerable<String> ScaleTypes
        {
            get { return Enum.GetNames(typeof(ScaleType)); }
        }

        public IEnumerable<String> RootNotes
        {
            get { return new NotesInfo().OrderWithAccidentals.Select(x => x.ToString()); }
        }

        public IEnumerable<String> Strings
        {
            get
            {
                var fretBoardOptions = Settings.Instance.FretBoardOptions.Value;
                return fretBoardOptions.Strings;
            }
        }

        public IEnumerable<int> Frets
        {
            get { return Enumerable.Range(0, FretBoardOptions.NoOfFret); }
        }

        public IEnumerable<int> ScalePositions
        {
            get { return Enumerable.Range(1, 2); }
        }

        public IEnumerable<string> SelectionFingeringStyles
        {
            get { return Enum.GetNames(typeof(ScaleSelectionManager.ScaleFingering)); }
        }

        public bool IsScaleStartFretEnabled
        {
            get { return _isScaleStartFretEnabled; }
            set
            {
                _isScaleStartFretEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsScaleEndFretEnabled
        {
            get { return _isScaleEndFretEnabled; }
            set
            {
                _isScaleEndFretEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsScalePositionEnabled
        {
            get { return _isScalePositionEnabled; }
            set
            {
                _isScalePositionEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsScaleFingeringEnabled
        {
            get { return _isScaleFingeringEnabled; }
            set
            {
                _isScaleFingeringEnabled = value;
                OnPropertyChanged();
            }
        }

        public string SelectedScaleType
        {
            get { return _selectedScaleType; }
            set
            {
                _selectedScaleType = value;
                OnPropertyChanged();
            }
        }

        public string SelectedRootNote
        {
            get { return _selectedRootNote; }
            set
            {
                _selectedRootNote = value;
                OnPropertyChanged();
            }
        }

        public bool IsColorOctaveEnabled
        {
            get { return _isColorOctaveEnabled; }
            set
            {
                _isColorOctaveEnabled = value;
                ColorOctaveButton(_isColorEquivalentEnabled);
                OnPropertyChanged();
            }
        }

        public bool IsColorEquivalentEnabled
        {
            get { return _isColorEquivalentEnabled; }
            set
            {
                _isColorEquivalentEnabled = value;
                ColorEquivalentNotes(_isColorEquivalentEnabled);
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool EnableAllNotes
        {
            get { return _enableAllNotes; }
            set
            {
                _enableAllNotes = value;
                EableAllNotes();
                OnPropertyChanged();
            }
        }

        public string HeaderLabel
        {
            get { return _headerLabel; }
            set
            {
                _headerLabel = value;
                OnPropertyChanged();
            }
        }

        public string SelectShowAllLabel
        {
            get { return _currentShowSelectLabel; }
            set
            {
                _currentShowSelectLabel = value;
                OnPropertyChanged();
            }
        }

        public string UnSelectHideAllLabel
        {
            get { return _currentHideUnselectLabel; }
            set
            {
                _currentHideUnselectLabel = value;
                OnPropertyChanged();
            }
        }

        public string SelectedStringName
        {
            get { return _selectedStringName; }
            set
            {
                _selectedStringName = value;
                OnPropertyChanged();
            }
        }

        public int SelectedStartFret
        {
            get { return _selectedStartFret; }
            set
            {
                _selectedStartFret = value;
                OnPropertyChanged();
            }
        }

        public int SelectedEndFret
        {
            get { return _selectedEndFret; }
            set
            {
                _selectedEndFret = value;
                OnPropertyChanged();
            }
        }

        public int SelectedScalePosition
        {
            get { return _selectedScalePosition; }
            set
            {
                _selectedScalePosition = value;
                OnPropertyChanged();
            }
        }

        public string SelectedScaleFingeringStyle
        {
            get { return _selectedScaleFingeringStyle; }
            set
            {
                _selectedScaleFingeringStyle = value;
                OnPropertyChanged();
            }
        }

        public int SelectedScaleStartFret
        {
            get { return _selectedScaleStartFret; }
            set
            {
                _selectedScaleStartFret = value;
                OnPropertyChanged();
            }
        }

        public int SelectedScaleEndFret
        {
            get { return _selectedScaleEndFret; }
            set
            {
                _selectedScaleEndFret = value;
                OnPropertyChanged();
            }
        }
    }
}