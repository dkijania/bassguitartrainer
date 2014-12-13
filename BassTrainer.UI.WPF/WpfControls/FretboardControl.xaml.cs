using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BassTrainer.Core.Resources;

namespace BassTrainer.UI.WPF.WpfControls
{
    /// <summary>
    /// Interaction logic for FretboardControl.xaml
    /// </summary>
    public partial class FretboardControl : UserControl
    {
        public FretboardControl()
        {
            InitializeComponent();
            FretBoard.Source =  new BitmapImage( new Uri(ResourcesManager.Instance[ResourcesManager.ResourceId.FretboardImage].ToString()));
        }
    }
}