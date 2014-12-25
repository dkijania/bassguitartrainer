using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Input;
using BassTrainer.Core.Const;
using BassTrainer.Core.Settings;
using BassTrainer.UI.WPF.SettingsManager;
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
        private bool _isApplyEnabled;
        private RadioButtonEnum _wrongPreset;
        private RadioButtonEnum _correctPreset;

        private bool _isSharpStyleShown;
        private bool _isFlatStyleShown;
        private bool _isMixedStyleShown;

        public ICommand ApplySettingsCommand { get; private set; }
        public ICommand RevertSettingsCommand { get; private set; }

        public VisualSettingViewModel()
        {
            ApplySettingsCommand = new DelegateCommand(ApplySettings);
            RevertSettingsCommand = new DelegateCommand(RevertSettings);
            ReadSettings();
        }

        private void ReadSettings()
        {
            NoOfTries = _settings.AttemptsCount.Value;
            ShowLastHitFor = _settings.DelayTime.Value;
            SetDefaultFontFamilyAndSize();
            ConvertFretBoardShowToBoolean(_settings.FretBoardOptions.Value.Show);
            CorrectPreset = (RadioButtonEnum) _settings.CorrectRectanglePreset.Value;
            WrongPreset = (RadioButtonEnum) _settings.WrongRectanglePreset.Value;
            IsShowCorrectAnwserSelected = _settings.ShowCorrectAnswer.Value;
        }

        private void RevertSettings()
        {
            ReadSettings();
        }

        private void SetDefaultFontFamilyAndSize()
        {
            FontSize = _settings.FontSize.Value;
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
            _settings.ShowCorrectAnswer.SetNewValue(IsShowCorrectAnwserSelected);
            UpdateShowFretboardStyle(_settings);
            _settings.FireSettingsChangedEvent();

            SaveSettings();
        }

        private void UpdateShowFretboardStyle(Settings settings)
        {
            var newFretBoardOptions = (FretBoardOptions)settings.FretBoardOptions.Value.Clone();
            newFretBoardOptions.Show = ConvertToFretBoardShow();
            settings.FretBoardOptions.SetNewValue(newFretBoardOptions);
        }

        private void SaveSettings()
        {
            var settingsConfigurator = new DotNetSettingsConfigurator();
            settingsConfigurator.Save(Settings.Instance);
        }

        private int GetNewSettingsForCorrectRectanglePreset()
        {
            return (int) CorrectPreset;
        }

        private int GetSettingsForWrongRectanglePreset()
        {
            return (int) WrongPreset;
        }

        public ObservableCollection<String> AvailableFontFamilies
        {
            get { return new ObservableCollection<String>(FontFamily.Families.Select(font => font.Name)); }
        }

        public string FontFamilySelectedItem { get; private set; }

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


        public bool IsSharpStyleShown
        {
            get { return _isSharpStyleShown; }
            set
            {
                _isSharpStyleShown = value;
                OnPropertyChanged();
            }
        }

        public bool IsFlatStyleShown
        {
            get { return _isFlatStyleShown; }
            set
            {
                _isFlatStyleShown = value;
                OnPropertyChanged();
            }
        }

        public bool IsMixedStyleShown
        {
            get { return _isMixedStyleShown; }
            set
            {
                _isMixedStyleShown = value;
                OnPropertyChanged();
            }
        }

        private FretBoardShow ConvertToFretBoardShow()
        {
            if (IsFlatStyleShown)
                return FretBoardShow.Flats;
            return IsSharpStyleShown ? FretBoardShow.Sharps : FretBoardShow.Mixed;
        }

        private void ConvertFretBoardShowToBoolean(FretBoardShow fretBoardShow)
        {
            switch (fretBoardShow)
            {
                case FretBoardShow.Sharps:
                    IsFlatStyleShown = false;
                    IsSharpStyleShown = true;
                    IsMixedStyleShown = false;
                    break;
                case FretBoardShow.Flats:
                    IsFlatStyleShown = true;
                    IsSharpStyleShown = false;
                    IsMixedStyleShown = false;
                    break;
                case FretBoardShow.Mixed:
                    IsFlatStyleShown = false;
                    IsSharpStyleShown = false;
                    IsMixedStyleShown = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("fretBoardShow");
            }
        }


        public bool IsApplyEnabled
        {
            get { return _isApplyEnabled; }
            set
            {
                _isApplyEnabled = value;
                OnPropertyChanged();
            }
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