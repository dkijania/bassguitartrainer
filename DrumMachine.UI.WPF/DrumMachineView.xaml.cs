using System.Windows.Controls;
using DrumMachine.UI.WPF.TimeSignature;

namespace DrumMachine.UI.WPF
{
    /// <summary>
    /// Interaction logic for DrumMachineView.xaml
    /// </summary>
    public partial class DrumMachineView : UserControl
    {
        public DrumMachineView()
        {
            InitializeComponent();
            
            var timeSignatureViewModel = new TimeSignatureViewModel();
            var patternTilesManipulator = new WpfDrumMachinePatternTilesManipulator(PatternGrid);
            var drumMachineViewModel = new DrumMachineViewModel(patternTilesManipulator,timeSignatureViewModel);
            TimeSignaturePanel.DataContext = timeSignatureViewModel;
            MainGrid.DataContext = drumMachineViewModel;
        }
    }
}