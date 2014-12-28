using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BassTrainer.UI.WPF.FretBoard;

namespace BassTrainer.UI.WPF.VisualSettings
{
    /// <summary>
    /// Interaction logic for VisualSettings.xaml
    /// </summary>
    public partial class VisualSettings
    {
        private readonly BorderStyleCollection _styleCollection = BorderStyleCollection.Instance;
        
        public VisualSettings()
        {
            InitializeComponent();
            DataContext = new VisualSettingViewModel();
            DrawCorrectBorderStyles();
            DrawWrongBorderStyles();
     
        }
        
        private void DrawWrongBorderStyles()
        {
            var collection = _styleCollection.WrongBorderStyles;
            var borderAreas = NotesOnFretBoard.Children.OfType<Canvas>().Skip(3);
            DrawBordersOnAreas(collection, borderAreas);
        }

        private void DrawCorrectBorderStyles()
        {
            var collection = _styleCollection.CorrectBorderStyles;
            var borderAreas = NotesOnFretBoard.Children.OfType<Canvas>();
            DrawBordersOnAreas(collection, borderAreas);
        }

        private void DrawBordersOnAreas(IReadOnlyList<Border> bordes, IEnumerable<Canvas> canvases)
        {
            for (var i = 0; i < Math.Min(bordes.Count, canvases.Count()); ++i)
            {
                var borderStyle = bordes[i];
                SetBorderDimensions(borderStyle);
                var canvas = canvases.ElementAt(i);
                canvas.Children.Add(borderStyle);
            }
        }

        private void SetBorderDimensions(FrameworkElement borderStyle)
        {
            Canvas.SetLeft(borderStyle, 10);
            Canvas.SetTop(borderStyle, 10);
            borderStyle.Width = 50;
            borderStyle.Height = 32;
        }

        private bool IsNumber(string text)
        {
            double ignoredResult;
            return double.TryParse(text, out ignoredResult);
        }
      }
}