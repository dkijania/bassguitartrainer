using System.Windows;
using System.Windows.Input;

namespace BassNotesMaster.WpfViews.Metronome
{
    /// <summary>
    /// Interaction logic for CounterDialog.xaml
    /// </summary>
    public partial class CounterDialog
    {
        public CounterModel Model { get;  set; }

        public CounterDialog()
        {
            InitializeComponent();
            Model = new CounterModel(this);
            MainGrid.DataContext = Model;

            KeyUp -= MainGrid_KeyDown;
            KeyUp += MainGrid_KeyDown;
        }

        public void ShowDialog(Window owner)
        {
            if (!IsVisible)
            {
                Owner = Owner ?? Application.Current.MainWindow;
                ShowDialog();
            }
            MainGrid.Focus();
        }

        private void MainGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Model.StopMetronomeAndCloseDialog();
                    break;
                case Key.Space: 
                    Model.PauseContineMetronome();
                    break;
            }
        }
    }
}