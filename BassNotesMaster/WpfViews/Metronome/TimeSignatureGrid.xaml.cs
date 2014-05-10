namespace BassNotesMaster.WpfViews.Metronome
{
    /// <summary>
    /// Interaction logic for TimeSignatureGrid.xaml
    /// </summary>
    public partial class TimeSignatureGrid : ITimeSignatureView
    {
        public TimeSignatureGrid()
        {
            InitializeComponent();
        }

        public void OnUseCustomTimeSignatureChange(bool value)
        {
            if (value)
            {
                CustomGrid.IsEnabled = true;
                StandardGrid.IsEnabled = false;
            }
            else
            {
                CustomGrid.IsEnabled = false;
                StandardGrid.IsEnabled = true;
            }
        }

        public void OnUseTimeSignature(bool value)
        {
            if (value)
            {
                CustomGroupBox.IsEnabled = StandardGroupBox.IsEnabled = true;
                if (EnableCustom.IsChecked.HasValue && EnableCustom.IsChecked.Value)
                {
                    CustomGrid.IsEnabled = true;
                    StandardGrid.IsEnabled = false;
                }
                else
                {
                    CustomGrid.IsEnabled = false;
                    StandardGrid.IsEnabled = true;
                }
            }
            else
            {
                CustomGroupBox.IsEnabled = StandardGroupBox.IsEnabled = false;
            }
        }
    }
}