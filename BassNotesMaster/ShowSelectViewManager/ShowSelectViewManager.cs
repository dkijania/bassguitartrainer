using System.Windows;
using System.Windows.Controls;
using BassNotesMaster.FretBoard.SelectionManager;
using BassNotesMaster.WpfControls;
using BassNotesMasterApi.Fretboard;
using BassNotesMasterApi.Settings;
using BassNotesMasterApi.Utils;

namespace BassNotesMaster.ShowSelectViewManager
{
    public class ShowSelectViewManager : Manager
    {
        private readonly SelectionControl _selectionControl;
        private readonly FretboardManager _fretboardManager;

        public ShowSelectViewManager(SelectionControl selectionControl, FretboardManager fretboardManager)
        {
            _selectionControl = selectionControl;
            _selectionControl.DataContext = this;
            _fretboardManager = fretboardManager;
        }

        public override void Init()
        {
            PrepareShowPanel();
            RegisterCommonEvents();
        }

        private void RegisterCommonEvents()
        {
            _selectionControl.StringRangeButton.Click += StringRangeButtonClick;
            _selectionControl.AllNotesCheckBox.Click += AllNotesCheckBoxClick;
            _selectionControl.ByScaleAdd.Click += AddScaleSelection_OnClick;
        }

        public override ManagerMode Mode { get; set; }

        public override void RemoveAllSubscribers()
        {
            
        }

        public override void OnModeChanged(ManagerMode mode)
        {
            RemoveAllEvents();
            _selectionControl.IsEnabled = true;
            switch (mode)
            {
                case ManagerMode.Info:
                    PrepareShowPanel();
                    break;
                case ManagerMode.Selection:
                    PrepareSelectPanel();
                    break;
                default:
                    _selectionControl.IsEnabled = false;
                    break;
            }
        }

        protected void RemoveAllEvents()
        {
            _selectionControl.SelectAll.Click -= SelectAllClick;
            _selectionControl.UnselectAll.Click -= UnselectAllClick;
            _selectionControl.SelectAll.Click -= ShowAllClick;
            _selectionControl.UnselectAll.Click -= HideAllClick;
        }

        protected void PrepareSelectPanel()
        {
            ((TabItem) _selectionControl.Parent).Header = "Select";
            _selectionControl.SelectAll.Content = "SelectAll";
            _selectionControl.UnselectAll.Content = "UnselectAll";
            _fretboardManager.SelectionManager.GuiSelector = new GuiSelector(Settings.Instance,
                                                                             _fretboardManager.FretBoard.
                                                                                 FretBoardGuiBuilder);
            _selectionControl.SelectAll.Click += SelectAllClick;
            _selectionControl.UnselectAll.Click += UnselectAllClick;
        }

        protected void PrepareShowPanel()
        {
            ((TabItem) _selectionControl.Parent).Header = "Show";
            _selectionControl.SelectAll.Content = "ShowAll";
            _selectionControl.UnselectAll.Content = "HideAll";
            _selectionControl.SelectAll.Click += ShowAllClick;
            _selectionControl.UnselectAll.Click += HideAllClick;
            _fretboardManager.SelectionManager.GuiSelector =
                new GuiShowSelector(_fretboardManager.FretBoard.FretBoardGuiBuilder);
        }

        private void SelectAllClick(object sender, RoutedEventArgs e)
        {
            _fretboardManager.FretBoard.FretBoardGuiBuilder.ForceClearView();
            _fretboardManager.SelectionManager.SelectAllItems();
        }

        private void UnselectAllClick(object sender, RoutedEventArgs e)
        {
            _fretboardManager.SelectionManager.UnselectAllItems();
        }

        private void ShowAllClick(object sender, RoutedEventArgs e)
        {
            _fretboardManager.ClearView();
            _fretboardManager.SelectionManager.SelectAllItems();
        }

        private void HideAllClick(object sender, RoutedEventArgs e)
        {
            _fretboardManager.FretBoard.FretBoardGuiBuilder.ForceClearView();
        }

        private void StringRangeButtonClick(object sender, RoutedEventArgs e)
        {
            var stringName = (string) _selectionControl.SelectionStringComboBox.SelectedItem;
            var startFret = (int) _selectionControl.SelectionStartFret.SelectedItem;
            var endFret = (int) _selectionControl.SelectionEndFret.SelectedItem;

            _fretboardManager.SelectionManager.SelectItems(stringName, startFret, endFret);
        }

        private void AllNotesCheckBoxClick(object sender, RoutedEventArgs e)
        {
            if (_selectionControl.AllNotesCheckBox.IsChecked != null &&
                _selectionControl.AllNotesCheckBox.IsChecked.Value)
            {
                _selectionControl.ScaleStartFret.IsEnabled = true;
                _selectionControl.ScaleEndFret.IsEnabled = true;
                _selectionControl.SelectionScalePosition.IsEnabled = false;
                _selectionControl.SelectionFingeringStyle.IsEnabled = false;
            }
            else
            {
                _selectionControl.ScaleStartFret.IsEnabled = false;
                _selectionControl.ScaleEndFret.IsEnabled = false;
                _selectionControl.SelectionScalePosition.IsEnabled = true;
                _selectionControl.SelectionFingeringStyle.IsEnabled = true;
            }
        }

        private void AddScaleSelection_OnClick(object sender, RoutedEventArgs e)
        {
            var scaleType = _selectionControl.SelectionScaleType.SelectedItem.ToString();
            var rootNote = new Note((string) _selectionControl.SelectionRootNote.SelectedItem);

            if (_selectionControl.AllNotesCheckBox.IsChecked != null &&
                _selectionControl.AllNotesCheckBox.IsChecked.Value)
            {
                var startFret = _selectionControl.ScaleStartFret.SelectedItem;
                var endFret = _selectionControl.ScaleEndFret.SelectedItem;
                _fretboardManager.SelectionManager.SelectItems(scaleType, rootNote, (int) startFret, (int) endFret);
            }
            else
            {
                var notePosition = (int) _selectionControl.SelectionScalePosition.SelectedItem;
                var fingeringStyle = _selectionControl.SelectionFingeringStyle.SelectedItem.ToString();
                _fretboardManager.SelectionManager.SelectScale(scaleType, rootNote, notePosition, fingeringStyle);
            }
        }
    }
}