using System.Collections;
using System.Windows;
using System.Windows.Controls;
using BassTrainer.Core.Components;

namespace BassTrainer.UI.WPF.WpfControls
{
    /// <summary>
    /// Interaction logic for ExcerciseControl.xaml
    /// </summary>
    public partial class ExcerciseControl : UserControl
    {
        public SelectionControl SelectionControl { get; set; }
        private ComponentsViewModelsLocator _locator = ComponentsViewModelsLocator.Instance;

        public ExcerciseControl()
        {
            InitializeComponent();
        }

        public void FillExcercisesControls(IEnumerable collection)
        {
            ExcercisesTypes.ItemsSource = collection;
            ExcercisesTypes.SelectedIndex = 0;

            ExcerciseEnd.IsEnabled = false;
            ExcercisePauseCont.IsEnabled = false;
        }


        private void ChooseSelection_Click(object sender, RoutedEventArgs e)
        {
            if (!_locator.IsMode(ComponentMode.Selection))
            {
                ExcerciseChooseSelection.Content = "Cancel";
                ExcercisesTypes.IsEnabled = false;
                _locator.ApplyMode(ComponentMode.Selection); 
            }
            else
            {
                ExcerciseChooseSelection.Content = "Choose Selection";
                ExcercisesTypes.IsEnabled = true;
                  _locator.ApplyMode(ComponentMode.Info);
            }
        }

        private void ExcercisePause_OnClick(object sender, RoutedEventArgs e)
        {
            SelectionControl.IsEnabled = false;
            ExcerciseEnd.IsEnabled = true;
            ExcerciseStart.IsEnabled = false;

            if (_locator.IsCurrentExcercisePaused)
            {
                _locator.ContinueCurrentExcercise();
                ExcercisePauseCont.Content = "Pause";
            }
            else
            {
                ExcercisePauseCont.Content = "Continue";
                _locator.PauseCurrentExcercise();
            }
        }

        private void ExcercisesTypes_Selected(object sender, RoutedEventArgs e)
        {
            _locator.SetNextExcercise(ExcercisesTypes.SelectedItem.ToString());
        }

        private void ExcerciseStop_OnClick(object sender, RoutedEventArgs e)
        {
            SelectionControl.IsEnabled = true;
            ExcerciseStart.IsEnabled = true;
            ExcerciseEnd.IsEnabled = false;
            ExcercisePauseCont.IsEnabled = false;
            ExcerciseChooseSelection.IsEnabled = true;
            ExcercisePauseCont.Content = "Pause";
            ExcerciseStart.Content = "Start";
            ExcercisesTypes.IsEnabled = true;
            _locator.StopExcerciseAndStartDefault();
        }

        private void ExcerciseStart_OnClick(object sender, RoutedEventArgs e)
        {
            if (_locator.IsMode(ComponentMode.Excercise))
            {
                _locator.Launcher.CurrentExcercise.Skip();
                return;
            }
            _locator.StartNewExcercise();
            SelectionControl.IsEnabled = false;
            ExcercisePauseCont.IsEnabled = true;
            ExcerciseEnd.IsEnabled = true;
            ExcerciseChooseSelection.IsEnabled = false;
            ExcerciseChooseSelection.Content = "Choose Selection";
            ExcercisesTypes.IsEnabled = false;
            ExcerciseStart.Content = "Skip";
        }
    }
}