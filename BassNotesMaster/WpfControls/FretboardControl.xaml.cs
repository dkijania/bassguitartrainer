using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BassNotesMasterApi.Resources;

namespace BassNotesMaster.WpfControls
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