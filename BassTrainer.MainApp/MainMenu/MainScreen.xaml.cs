using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BassTrainer.Core.Resources;
using MahApps.Metro.Controls;

namespace BassTrainer.MainApp.MainMenu
{
    /// <summary>
    ///     Interaction logic for MainScreen.xaml
    /// </summary>
    public partial class MainScreen : UserControl
    {
        public MainScreen()
        {
            InitializeComponent();
            SetImage(Trainer, ResourcesManager.ResourceId.TileTrainer);
            SetImage(Drums, ResourcesManager.ResourceId.TileDrums);
            SetImage(Metronome, ResourcesManager.ResourceId.TileMetronome);
            SetImage(Tuning, ResourcesManager.ResourceId.TileTuning);
        }

        private static void SetImage(Control element, ResourcesManager.ResourceId resourceId)
        {
            var imageBrush = new ImageBrush();
            var image = new Image
            {
                Source = new BitmapImage(
                    new Uri(ResourcesManager.Instance[resourceId].ToString()))
            };
            imageBrush.ImageSource = image.Source;
            element.Background = imageBrush;
        }
    }
}