using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Input;
using BassTrainer.Core.Const;
using BassTrainer.Core.Settings;
using WpfExtensions;

namespace BassTrainer.UI.WPF.VisualSettings
{
    public class VisualSettingViewModel : BindingDataContextBase
    {
        private readonly Settings _settings = Settings.Instance;

        private int _noOfTries;
        private double _showLastHitFor;
        private bool _isShowCorrectAnwserSelected;
        private int _fontSize;
        private FretBoardShow _fretBoardShowStyle;
        private RadioButtonEnum _wrongPreset;

        public RadioButtonEnum WrongPreset
        {
            get { return _wrongPreset; }
            set
            {
                _wrongPreset = value; 
                OnPropertyChanged();
            }
        }

        public RadioButtonEnum CorrectPreset
        {
            get { return _correctPreset; }
            set
            {
                _correctPreset = value;
                OnPropertyChanged();
            }
        }

        private RadioButtonEnum _correctPreset;

        public ICommand ApplySettingsCommand { get; private set; }
        public ICommand OnTextChangedCommand { get; private set; }
        
        public VisualSettingViewModel()
        {
            NoOfTries = _settings.AttemptsCount.Value;
            ShowLastHitFor = _settings.DelayTime.Value;
            SetDefaultFontFamilyAndSize();
            FretBoardShowStyle = FretBoardShow.Sharps;
            ApplySettingsCommand = new DelegateCommand(ApplySettings);
            CorrectPreset = (RadioButtonEnum) _settings.CorrectRectanglePreset.Value;
            WrongPreset = (RadioButtonEnum)_settings.WrongRectanglePreset.Value;
        }

        private void SetDefaultFontFamilyAndSize()
        {
            FontSize = _settings.FontSize.Value;
            AvailableFontFamilies = new ObservableCollection<String>(FontFamily.Families.Select(font => font.Name));
            FontFamilySelectedItem = _settings.FontFamilyName.Value;
        }

        private void ApplySettings()
        {
            var newCorrectRectanglePreset = GetNewSettingsForCorrectRectanglePreset();
            var newWrongRectanglePreset = GetSettingsForWrongRectanglePreset();
            _settings.CorrectRectanglePreset.SetNewValue(newCorrectRectanglePreset);
            _settings.WrongRectanglePreset.SetNewValue(newWrongRectanglePreset);
            _settings.FontFamilyName.SetNewValue(FontFamilySelectedItem);
            _settings.FontSize.SetNewValue(FontSize);
            _settings.AttemptsCount.SetNewValue(NoOfTries);
            _settings.DelayTime.SetNewValue(ShowLastHitFor);
            _settings.FireSettingsChangedEvent();
        }
        
        private int GetNewSettingsForCorrectRectanglePreset()
        {
            return (int) CorrectPreset;
        }
        
        private int GetSettingsForWrongRectanglePreset()
        {
            return (int) WrongPreset;
        }
        
        public ObservableCollection<String> AvailableFontFamilies { get; private set; }
        public string FontFamilySelectedItem { get; private set; }

        public FretBoardShow FretBoardShowStyle
        {
            get { return _fretBoardShowStyle; }
            set
            {
                _fretBoardShowStyle = value;
                UpdateShowFretboardStyle();
                OnPropertyChanged();
            }
        }

        private void UpdateShowFretboardStyle()
        {
            var settings = Settings.Instance;
            var newFretBoardOptions = (FretBoardOptions)settings.FretBoardOptions.Value.Clone();
            newFretBoardOptions.Show = FretBoardShowStyle;
            settings.FretBoardOptions.SetNewValue(newFretBoardOptions);
            settings.FireSettingsChangedEvent();
        }

        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
                OnPropertyChanged();
            }
        }

        public int NoOfTries
        {
            get { return _noOfTries; }
            set
            {
                _noOfTries = value;
                OnPropertyChanged();
            }
        }

        public double ShowLastHitFor
        {
            get { return _showLastHitFor; }
            set
            {
                _showLastHitFor = value;
                OnPropertyChanged();
            }
        }

        public bool IsShowCorrectAnwserSelected
        {
            get { return _isShowCorrectAnwserSelected; }
            set
            {
                _isShowCorrectAnwserSelected = value;
                OnPropertyChanged();
            }
        }
    }
}
