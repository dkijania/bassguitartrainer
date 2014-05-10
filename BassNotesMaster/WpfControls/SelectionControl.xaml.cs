using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using BassNotesMasterApi;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Fretboard.SelectionManager;
using BassNotesMasterApi.Settings;

namespace BassNotesMaster.WpfControls
{
    /// <summary>
    /// Interaction logic for SelectionControl.xaml
    /// </summary>
    public partial class SelectionControl : UserControl
    {
        public SelectionControl()
        {
            InitializeComponent();
        }

        public void FillSelectionControlsWithData()
        {
            AllNotesCheckBox.IsChecked = false;

            SelectionScaleType.ItemsSource = Enum.GetNames(typeof(ScaleType));
            SelectionScaleType.SelectedIndex = 0;

            var notesInfo = new NotesInfo();
            SelectionRootNote.ItemsSource = notesInfo.OrderWithAccidentals.Select(x => x.ToString());
            SelectionRootNote.SelectedIndex = 0;

            var fretBoardOptions = Settings.Instance.FretBoardOptions.Value;
            SelectionStringComboBox.ItemsSource = fretBoardOptions.Strings;
            SelectionStringComboBox.SelectedIndex = 0;

            SelectionStartFret.ItemsSource = Enumerable.Range(0, FretBoardOptions.NoOfFret);
            SelectionStartFret.SelectedIndex = 0;

            SelectionEndFret.ItemsSource = Enumerable.Range(0, FretBoardOptions.NoOfFret);
            SelectionEndFret.SelectedIndex = 0;

            SelectionScalePosition.ItemsSource = Enumerable.Range(1, 2);
            SelectionScalePosition.SelectedIndex = 0;

            ScaleStartFret.ItemsSource = Enumerable.Range(0, FretBoardOptions.NoOfFret);
            ScaleStartFret.SelectedIndex = 0;
            ScaleStartFret.IsEnabled = false;

            ScaleEndFret.ItemsSource = Enumerable.Range(0, FretBoardOptions.NoOfFret);
            ScaleEndFret.SelectedIndex = FretBoardOptions.NoOfFret - 1;
            ScaleEndFret.IsEnabled = false;

            SelectionFingeringStyle.ItemsSource = Enum.GetNames(typeof(ScaleSelectionManager.ScaleFingering));
            SelectionFingeringStyle.SelectedIndex = 0;
        }

        public void ColorOctaveButton(object sender, System.Windows.RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton) sender;
            ManagersLocator.Instance.FretboardManager.FretBoard.FretBoardGuiBuilder.ApplyColorForOctaves = toggleButton.IsChecked.Value;
        }

        public void ColorEquivalentNotes(object sender, System.Windows.RoutedEventArgs e)
        {
            var toggleButton = (ToggleButton)sender;
            ManagersLocator.Instance.FretboardManager.FretBoard.FretBoardGuiBuilder.ApplyColorForNotes= toggleButton.IsChecked.Value;
        }
    }
}
