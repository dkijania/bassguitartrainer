using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace BassNotesMaster.FretBoard
{
    public class BorderStyleCollection
    {
        private static BorderStyleCollection _instance;

        public static BorderStyleCollection Instance
        {
            get { return _instance ?? (_instance = new BorderStyleCollection()); }
        }

        private BorderStyleCollection(){}

        public Brush BackgroundBrushAfterSelection =  new SolidColorBrush(Colors.CornflowerBlue);
        
        public Border GetBorderStyle(bool isCorrect,int presetNo)
        {
            return isCorrect ? GetCorrectBorderStyle(presetNo) : GetWrongBorderStyle(presetNo);
        }
        
        public List<Border> CorrectBorderStyles
        {
            get { return GetCorrectBorderStyles(); }
        }

        public List<Border> WrongBorderStyles
        {
            get { return GetWrongBorderStyles(); }
        }

        public Border SelectedBorderStyle
        {
            get { return GetSelectedBorderStyle(); }
        }

        private Border GetSelectedBorderStyle()
        {
            return  new Border
                             {
                                 BorderBrush = new SolidColorBrush(Colors.Blue),
                                 BorderThickness = new Thickness(1),
                                 Background = new SolidColorBrush(Colors.LightBlue),
                                 CornerRadius = new CornerRadius(1),
                                 Opacity = 0.5,
                             };
        }

        

        private List<Border> GetCorrectBorderStyles()
        {
            var simpleborder = new Border { Background = new SolidColorBrush { Color = Colors.WhiteSmoke } };
            var shadingBorder = new Border
            {
                Background = new SolidColorBrush { Color = Colors.WhiteSmoke },
                CornerRadius = new CornerRadius(10),
                BorderBrush = new SolidColorBrush { Color = Colors.Black },
                Effect = new DropShadowEffect
                {
                    Color = Colors.Gray,
                    Opacity = 0.5,
                    ShadowDepth = 16,
                }
            };
            var gradientBorder = new Border
            {
                Background = new LinearGradientBrush
                {
                    StartPoint = new Point(0.5, 0),
                    EndPoint = new Point(0.5, 1),
                    GradientStops = new GradientStopCollection
                                                                          {
                                                                              new GradientStop
                                                                                  {
                                                                                      Color = Colors.Azure,
                                                                                      Offset = 0.5
                                                                                  },
                                                                              new GradientStop
                                                                                  {
                                                                                      Color = Colors.DodgerBlue,
                                                                                      Offset = 1
                                                                                  },
                                                                          }
                },
                Effect = new DropShadowEffect
                {
                    Color = Colors.Gray,
                    Opacity = 0.7,
                    ShadowDepth = 10,
                }
            };
           return new List<Border>{simpleborder,shadingBorder,gradientBorder};      
        } 
        
        public List<Border> GetWrongBorderStyles()
        {
            var redsimpleborder = new Border { Background = new SolidColorBrush { Color = Colors.Coral } };
          
            var redShadingBorder = new Border
            {
                Background = new SolidColorBrush { Color = Colors.Coral },
                CornerRadius = new CornerRadius(10),
                BorderBrush = new SolidColorBrush { Color = Colors.Black },
                Effect = new DropShadowEffect
                {
                    Color = Colors.Gray,
                    Opacity = 0.5,
                    ShadowDepth = 16,
                }
            };

            var redGradientBorder = new Border
            {
                Background = new LinearGradientBrush
                {
                    StartPoint = new Point(0.5, 0),
                    EndPoint = new Point(0.5, 1),
                    GradientStops = new GradientStopCollection
                                                                          {
                                                                              new GradientStop
                                                                                  {
                                                                                      Color = Colors.Azure,
                                                                                      Offset = 0.5
                                                                                  },
                                                                              new GradientStop
                                                                                  {
                                                                                      Color = Colors.Crimson,
                                                                                      Offset = 1
                                                                                  },
                                                                          }
                },
                Effect = new DropShadowEffect
                {
                    Color = Colors.Gray,
                    Opacity = 0.7,
                    ShadowDepth = 10,
                }
            };
            return new List<Border> { redsimpleborder, redShadingBorder, redGradientBorder };      
      
        }

        public Border GetWrongBorderStyle(int presetNo)
        {
            var wrongBorderStyles = WrongBorderStyles;
            if (presetNo < 0 || presetNo >= wrongBorderStyles.Count)
            {
                throw new IndexOutOfRangeException(String.Format("Available wrong styles : {0} , {1} but chosen ", wrongBorderStyles.Count, presetNo));
            }
            return wrongBorderStyles[presetNo];
        }

        public Border GetCorrectBorderStyle(int presetNo)
        {
            var correctBorderStyles = CorrectBorderStyles;
            if (presetNo < 0 || presetNo >= correctBorderStyles.Count)
            {
                throw new IndexOutOfRangeException(String.Format("Available correct styles : {0} , {1} but chosen ", correctBorderStyles.Count, presetNo));
            }
            return correctBorderStyles[presetNo];
        }
    }
}
