using System.Collections;
using System.Windows;
using System.Windows.Controls;
using BassNotesMasterApi;
using BassNotesMasterApi.Excercise;
using BassNotesMasterApi.Utils;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace BassNotesMaster.WpfControls
{
    /// <summary>
    /// Interaction logic for ExcerciseControl.xaml
    /// </summary>
    public partial class ExcerciseControl : UserControl
    {
        public SelectionControl SelectionControl { get; set; }
        public MetroWindow Window { get; set; }
 
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
            if (ManagersLocator.Instance.Mode != ManagerMode.Selection)
            {
                ExcerciseChooseSelection.Content = "Cancel";
                ExcercisesTypes.IsEnabled = false;
                ManagersLocator.Instance.Mode = ManagerMode.Selection;
            }
            else
            {
                ExcerciseChooseSelection.Content = "Choose Selection";
                ExcercisesTypes.IsEnabled = true; 
                ManagersLocator.Instance.Mode = ManagerMode.Info;
            }
        }

        private void ExcercisePause_OnClick(object sender, RoutedEventArgs e)
        {
            SelectionControl.IsEnabled = false;
            ExcerciseEnd.IsEnabled = true;
            ExcerciseStart.IsEnabled = false;

            if (ManagersLocator.Instance.IsCurrentExcercisePaused)
            {
                ManagersLocator.Instance.ContinueCurrentExcercise();
                ExcercisePauseCont.Content = "Pause";
                
            }
            else
            {
                ExcercisePauseCont.Content = "Continue";
                ManagersLocator.Instance.PauseCurrentExcercise();
            }
        }

        private void ExcercisesTypes_Selected(object sender, RoutedEventArgs e)
        {
            ManagersLocator.Instance.SetNextExcercise(ExcercisesTypes.SelectedItem.ToString());
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
            ManagersLocator.Instance.StopExcerciseAndStartDefault();
            
        }

        private void ExcerciseStart_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if(ManagersLocator.Instance.Mode == ManagerMode.Excercise)
                {
                    ManagersLocator.Instance.Launcher.CurrentExcercise.Skip();
                    return;
                }
                ManagersLocator.Instance.StartNewExcercise();
                SelectionControl.IsEnabled = false;
                ExcercisePauseCont.IsEnabled = true;
                ExcerciseEnd.IsEnabled = true;
                ExcerciseChooseSelection.IsEnabled = false;
                ExcerciseChooseSelection.Content = "Choose Selection";
                ExcercisesTypes.IsEnabled = false;
                ExcerciseStart.Content = "Skip";
            }
            catch (ExcerciseException exception)
            {
                Window.ShowMessageAsync(exception.Message, "Excercise error");
            }
        }

    }
}
