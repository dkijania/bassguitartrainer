using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BassNotesMaster.FretBoard;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Settings;

namespace BassNotesMaster.VisualSettings
{
    /// <summary>
    /// Interaction logic for VisualSettings.xaml
    /// </summary>
    public partial class VisualSettings
    {
        private Settings _settings;
        private readonly BorderStyleCollection _styleCollection = BorderStyleCollection.Instance;

        private readonly Brush _correctValueBrush = new SolidColorBrush(Colors.White);
        private readonly Brush _wrongValueBrush = new SolidColorBrush(Colors.Red);

        public VisualSettings()
        {
            InitializeComponent();
        }

        public void Init(Settings settings)
        {
            _settings = settings;
            DrawCorrectBorderStyles();
            DrawWrongBorderStyles();
            SetDefaultValueForWrongRectangleRadioButtons();
            SetDefaultValueForCorrectRectangleRadioButtons();
            SetDefaultValuesForExcercises();
            SetDefaultFontFamilyAndSize();
            SetShowBasedOnSettings();
        }

        private void SetDefaultValuesForExcercises()
        {
            NoOfTries.Text = _settings.AttemptsCount.Value.ToString(CultureInfo.InvariantCulture);
            ShowLastHitFor.Text = _settings.DelayTime.Value.ToString(CultureInfo.InvariantCulture);
        }

        private void DrawWrongBorderStyles()
        {
            var collection = _styleCollection.WrongBorderStyles;
            var borderAreas = NotesOnFretBoard.Children.OfType<Canvas>().Skip(3);
            DrawBordersOnAreas(collection, borderAreas);
        }

        private void DrawCorrectBorderStyles()
        {
            var collection = _styleCollection.CorrectBorderStyles;
            var borderAreas = NotesOnFretBoard.Children.OfType<Canvas>();
            DrawBordersOnAreas(collection, borderAreas);
        }

        private void DrawBordersOnAreas(IReadOnlyList<Border> bordes, IEnumerable<Canvas> canvases)
        {
            for (var i = 0; i < Math.Min(bordes.Count, canvases.Count()); ++i)
            {
                var borderStyle = bordes[i];
                SetBorderDimensions(borderStyle);
                var canvas = canvases.ElementAt(i);
                canvas.Children.Add(borderStyle);
            }
        }

        private void SetBorderDimensions(FrameworkElement borderStyle)
        {
            Canvas.SetLeft(borderStyle, 10);
            Canvas.SetTop(borderStyle, 10);
            borderStyle.Width = 50;
            borderStyle.Height = 32;
        }

        private void SetDefaultValueForWrongRectangleRadioButtons()
        {
            switch (_settings.WrongRectanglePreset.Value)
            {
                case 0:
                    WrongRadioButton0.IsChecked = true;
                    break;
                case 1:
                    WrongRadioButton1.IsChecked = true;
                    break;
                case 2:
                    WrongRadioButton2.IsChecked = true;
                    break;
            }
        }

        private void SetDefaultValueForCorrectRectangleRadioButtons()
        {
            switch (_settings.CorrectRectanglePreset.Value)
            {
                case 0:
                    CorrectRadioButton0.IsChecked = true;
                    break;
                case 1:
                    CorrectRadioButton1.IsChecked = true;
                    break;
                case 2:
                    CorrectRadioButton2.IsChecked = true;
                    break;
            }
        }

        private void SetDefaultFontFamilyAndSize()
        {
            FontSizeTextBox.Text = _settings.FontSize.Value.ToString(CultureInfo.InvariantCulture);
            FontFamilyComboBox.ItemsSource = System.Drawing.FontFamily.Families.Select(font => font.Name);
            FontFamilyComboBox.SelectedItem = _settings.FontFamilyName.Value;
        }

        public void OK_Click(object sender, RoutedEventArgs e)
        {
            ApplySettings();
         }

        private void ApplySettings()
        {
            var newCorrectRectanglePreset = GetNewSettingsForCorrectRectanglePreset();
            var newWrongRectanglePreset = GetSettingsForWrongRectanglePreset();
            var newFontSize = GetNewFontSize();
            var newFontFamilyFont = GetNewFontFamilyFont();
            _settings.CorrectRectanglePreset.SetNewValue(newCorrectRectanglePreset);
            _settings.WrongRectanglePreset.SetNewValue(newWrongRectanglePreset);
            _settings.FontFamilyName.SetNewValue(newFontFamilyFont);
            _settings.FontSize.SetNewValue(newFontSize);
            _settings.AttemptsCount.SetNewValue(GetAttempsCount());
            _settings.DelayTime.SetNewValue(GetDelayValue());
            _settings.FireSettingsChangedEvent();
        }

        private int GetNewFontSize()
        {
            return Convert.ToInt32(FontSizeTextBox.Text);
        }

        private int GetAttempsCount()
        {
            return Convert.ToInt32(NoOfTries.Text);
        }

        private double GetDelayValue()
        {
            return Convert.ToDouble(ShowLastHitFor.Text);
        }

        private string GetNewFontFamilyFont()
        {
            return FontFamilyComboBox.SelectedItem.ToString();
        }

        private int GetNewSettingsForCorrectRectanglePreset()
        {
            if (IsRadioButtonChecked(CorrectRadioButton0))
            {
                return 0;
            }
            return IsRadioButtonChecked(CorrectRadioButton1) ? 1 : 2;
        }

        private bool IsRadioButtonChecked(RadioButton radioButton)
        {
            return radioButton.IsChecked != null && radioButton.IsChecked.Value;
        }

        private int GetSettingsForWrongRectanglePreset()
        {
            if (IsRadioButtonChecked(WrongRadioButton0))
            {
                return 0;
            }
            return IsRadioButtonChecked(WrongRadioButton1) ? 1 : 2;
        }

        private void FontSizeTextBox_Changed(object sender, RoutedEventArgs routedEventArgs)
        {
            var tb = sender as TextBox;
            if (tb == null)
            {
                throw new BadControlTypeException(String.Format("expected Textbox type of {0}", sender.GetType()));
            }
            if (IsNumber(tb.Text.Trim()))
            {
                FontSizeTextBox.BorderBrush = _correctValueBrush;
                Apply.IsEnabled = true;
            }
            else
            {
                tb.BorderBrush = _wrongValueBrush;
                Apply.IsEnabled = false;
            }
        }

        private bool IsNumber(string text)
        {
            double ignoredResult;
            return double.TryParse(text, out ignoredResult);
        }

        private void SetShowBasedOnSettings()
        {
            switch (Settings.Instance.FretBoardOptions.Value.Show)
            {
                case FretBoardShow.Sharps:
                    ShowSharps.IsChecked = true;
                    break;
                case FretBoardShow.Bemols:
                    ShowBemol.IsChecked = true;
                    break;
                case FretBoardShow.Mixed:
                    ShowMixed.IsChecked = true;
                    break;
            }
        }

        private void ShowFretboardTypeClick(object sender, RoutedEventArgs e)
        {
            var settings = Settings.Instance;
            var newFretBoardOptions = (FretBoardOptions) settings.FretBoardOptions.Value.Clone();
            if (ShowSharps.IsChecked != null && ShowSharps.IsChecked.Value)
            {
                newFretBoardOptions.Show = FretBoardShow.Sharps;
            }
            if (ShowBemol.IsChecked != null && ShowBemol.IsChecked.Value)
            {
                newFretBoardOptions.Show = FretBoardShow.Bemols;
            }
            if (ShowMixed.IsChecked != null && ShowMixed.IsChecked.Value)
            {
                newFretBoardOptions.Show = FretBoardShow.Mixed;
            }
            settings.FretBoardOptions.SetNewValue(newFretBoardOptions);
            settings.FireSettingsChangedEvent();
        }
    }
}