using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Notation;
using BassNotesMasterApi.Resources;
using BassNotesMasterApi.Utils;
using WpfExtensions;
using Image = System.Windows.Controls.Image;


namespace BassNotesMaster.Notation
{
    public class WpfMusicNotationGraphicObjectsManager : MusicNotationGraphicObjectsManager
    {
         public readonly Panel Canvas;
       
        public WpfMusicNotationGraphicObjectsManager(Panel canvas) 
        {
            Canvas = canvas;
            CanvasHeight = canvas.Height;
            CanvasWidth = canvas.Width;
            LineOffset = CanvasHeight / 10;
            HeightCenter = CanvasHeight / 2;
            WidthCenter = CanvasWidth / 2;
            FourthSteveLineYPosition = HeightCenter - (2 * LineOffset);
        }
        
        public override void DrawBassStave()
        {
            DrawStave();
            DrawBassClef();
        }


        public override double GetDistanceFromBottom(double height)
        {
            var collection = GetIntervals();
            var min = double.MaxValue;
            var index = -1;
            for (var i = 0; i < collection.Count(); ++i)
            {
                var currentMin = Math.Abs(collection[i] - height) / LineOffset;
                if (!(currentMin < min)) continue;
                min = currentMin;
                index = i;
            }
            return index;
        }

        private List<double> GetIntervals()
        {
            var botomLine = CalculateLedgerLineYPosition(3);
            var topLine = CalculateLedgerLineYPosition(-4);
            var outputList = new List<double>();
            for (var i = botomLine; i >= (topLine - LineOffset / 2); i -= LineOffset / 2)
            {
                outputList.Add(i);
            }
            return outputList;
        }

        public override void ClearView()
        {
            Canvas.Children.Clear();
        }

        private void DrawStave()
        {
            foreach (var line in GetStave())
            {
                Canvas.Children.Add(line);
            }
        }

        private IEnumerable<Line> GetStave()
        {
            const int strokeThickness = MusicNotationGraphicProperties.StrokeThickness;
            var colorBrush = MusicNotationGraphicProperties.ColorBrush;
            var staveLines = new List<Line>();
            for (var i = -2; i <= 2; ++i)
            {
                var staveline = new Line
                                    {
                                        X1 = 0,
                                        Y1 = HeightCenter + i * LineOffset,
                                        X2 = CanvasWidth,
                                        Y2 = HeightCenter + i * LineOffset,
                                        StrokeThickness = strokeThickness,
                                        Stroke = colorBrush
                                    };
                staveLines.Add(staveline);
            }
            return staveLines.ToArray();
        }

        private void DrawBassClef()
        {
            var bassClef = GetBassClefImage();
            Canvas.Children.Add(bassClef);
        }

        private void DrawNote(IEnumerable<UIElement> noteComponentsToDraw)
        {
            foreach (var noteElement in noteComponentsToDraw)
            {
                Canvas.Children.Add(noteElement);
            }
        }

        private void DrawElement(FrameworkElement element)
        {
            Canvas.Children.Add(element);
        }

        private Line GetBottomLedgerLine(bool cutLength = true)
        {
            return GetLedgerLine(lineOffsetDistanceFromCenter: 3, cutLength: cutLength);
        }

        private Line GetFirstUpperLedgerLine(bool cutLength = true)
        {
            return GetLedgerLine(lineOffsetDistanceFromCenter: -3, cutLength: cutLength);
        }

        private Line GetSecondUpperLedgerLine(bool cutLength = true)
        {
            return GetLedgerLine(lineOffsetDistanceFromCenter: -4, cutLength: cutLength);
        }


        private Line GetLedgerLine(int lineOffsetDistanceFromCenter, bool cutLength = true)
        {
            return new Line
                       {
                           X1 = cutLength ? 140 : 0,
                           Y1 = CalculateLedgerLineYPosition(lineOffsetDistanceFromCenter),
                           X2 = cutLength ? 190 : CanvasWidth,
                           Y2 = CalculateLedgerLineYPosition(lineOffsetDistanceFromCenter),
                           StrokeThickness = MusicNotationGraphicProperties.StrokeThickness,
                           Stroke = MusicNotationGraphicProperties.ColorBrush
                       };
        }

        private double CalculateLedgerLineYPosition(int lineOffsetDistanceFromCenter)
        {
            return HeightCenter + lineOffsetDistanceFromCenter * LineOffset;
        }

        private Image GetBassClefImage()
        {
            var clef = new Image
                           {
                               Height = 3 * LineOffset,
                               Margin = new Thickness(0, FourthSteveLineYPosition, 0, 0),
                               Source = new BitmapImage(new Uri(ResourceManager[ResourcesManager.ResourceId.BassClefImage].ToString()))
                           };
            return clef;
        }

        private Image GetNoteWithStemUpward(int distanceFromBottom, bool isCorrect)
        {
            var img = new Image
                          {
                              Height = 4 * LineOffset,
                              Margin =
                                  new Thickness(WidthCenter,
                                                GetXBasedOnDistanceFromBottom(distanceFromBottom) - 3.5 * LineOffset, 0,
                                                0),
                              Source =
                                  new BitmapImage(new Uri(
                                  ResourceManager[isCorrect
                                                      ? ResourcesManager.ResourceId.QuarterNoteStemUpwardImage
                                                      : ResourcesManager.ResourceId.WrongQuarterNoteStemUpwardImage].ToString()))
                          };
            return img;
        }

        private Image GetNoteWithStemDownward(int distanceFromBottom, bool isCorrect)
        {
            var img = new Image
                          {
                              Height = 4 * LineOffset,
                              Margin =
                                  new Thickness(WidthCenter,
                                                GetXBasedOnDistanceFromBottom(distanceFromBottom) - (0.5 * LineOffset), 0,
                                                0),
                              Source =
                                  new BitmapImage(new Uri(
                                  ResourceManager[isCorrect
                                                      ? ResourcesManager.ResourceId.QuarterNoteStemDownwardImage
                                                      : ResourcesManager.ResourceId.WrongQuarterNoteStemDownwardImage].ToString())),
                          };
            return img;
        }

        private double GetXBasedOnDistanceFromBottom(int distance)
        {
            return HeightCenter + (3 * LineOffset) - distance * 0.5 * LineOffset;
        }

        private Image GetSharpForNote(Image note, NotesInfo.NoteStem stem, bool isCorrect)
        {
            var img = new Image
                          {
                              Height = LineOffset * 3,
                              Source = new BitmapImage(new Uri(ResourceManager[isCorrect
                                                                                   ? ResourcesManager.ResourceId.
                                                                                         SharpNoteImage
                                                                                   : ResourcesManager.ResourceId.
                                                                                         WrongSharpNoteImage].ToString())),
                              Margin = SetAnchorForSharpDependingOnNoteType(note, stem)
                          };
            return img;
        }

        private Image GetFlatForNote(Image note, NotesInfo.NoteStem stem, bool isCorrect)
        {
            var img = new Image
                          {
                              Height = LineOffset * 2,
                              Source =
                                  new BitmapImage(new Uri(
                                  ResourceManager[isCorrect
                                                      ? ResourcesManager.ResourceId.FlatNoteImage
                                                      : ResourcesManager.ResourceId.WrongFlatNoteImage].ToString())),
                              Margin = SetAnchorForFlatDependingOnNoteType(note, stem)
                          };
            return img;
        }

        private Thickness SetAnchorForSharpDependingOnNoteType(Image note, NotesInfo.NoteStem stem)
        {
            var margin = note.Margin;
            margin.Left -= LineOffset * 2;
            switch (stem)
            {
                case NotesInfo.NoteStem.Upward:
                    margin.Top += LineOffset * 2;
                    break;
                default:
                    margin.Top -= LineOffset;
                    break;
            }
            return margin;
        }

        private Thickness SetAnchorForFlatDependingOnNoteType(Image note, NotesInfo.NoteStem stem)
        {
            var margin = note.Margin;
            margin.Left -= LineOffset;
            switch (stem)
            {
                case NotesInfo.NoteStem.Upward:
                    margin.Top += LineOffset * 2.2;
                    break;
                case NotesInfo.NoteStem.Downward:
                    margin.Top -= 0.8 * LineOffset;
                    break;
            }
            return margin;
        }


        public override void DrawTransparentLedgerLines()
        {
            var doubleCollection = new DoubleCollection {10, 4};
            const double opacity = 0.3;
            var bottomLedgerLine = GetBottomLedgerLine(cutLength: false);
            var firstUpperLedgerLine = GetFirstUpperLedgerLine(cutLength: false);
            var secondUpperLedgerLine = GetSecondUpperLedgerLine(cutLength: false);
            bottomLedgerLine.StrokeDashArray = doubleCollection;
            bottomLedgerLine.Opacity = opacity;
            firstUpperLedgerLine.StrokeDashArray = doubleCollection;
            firstUpperLedgerLine.Opacity = opacity;
            secondUpperLedgerLine.StrokeDashArray = doubleCollection;
            secondUpperLedgerLine.Opacity = opacity;
            DrawElement(bottomLedgerLine);
            DrawElement(firstUpperLedgerLine);
            DrawElement(secondUpperLedgerLine);
        }

        public override void RemoveTransparentLedgerLines()
        {
            Canvas.RemoveVisual<Line>((x => x.Opacity.Equals(0.3)));
        }
        public override void DrawNote(int distanceFromBottom, NotesInfo.Accidentals accidentals, NotesInfo.NoteStem stemDirection, Note note, StringFretPair stringFretPair, bool isCorrect)
        {
            var graphicNote = GetImageOfNote(stemDirection, distanceFromBottom, isCorrect);
            var accidental = GetAccidental(note, accidentals, graphicNote, stemDirection, isCorrect);
            var ledgerLines = DrawLedgerLinesIfNeeded(stringFretPair, accidentals);
            var itemsToDraw = GetItemsToDraw(graphicNote, ledgerLines, accidental);
            DrawNote(itemsToDraw);
        }

        public Image GetImageOfNote(NotesInfo.NoteStem stemDirection, int distanceFromBottom, bool isCorrect)
        {
            switch (stemDirection)
            {
                case NotesInfo.NoteStem.Downward:
                    return GetNoteWithStemDownward(distanceFromBottom, isCorrect);
                default:
                    return GetNoteWithStemUpward(distanceFromBottom, isCorrect);
            }
        }

        public FrameworkElement GetAccidental(Note note, NotesInfo.Accidentals accidentals, Image graphicNote,
                                              NotesInfo.NoteStem stemDirection, bool isCorrect)
        {
            if (note.IsNatural())
            {
                return null;
            }
            switch (accidentals)
            {
                case NotesInfo.Accidentals.Flat:
                    return GetFlatForNote(graphicNote, stemDirection, isCorrect);
                default:
                    return GetSharpForNote(graphicNote, stemDirection, isCorrect);
            }
        }

        public IEnumerable<FrameworkElement> DrawLedgerLinesIfNeeded(StringFretPair stringFretPair,
                                                                     NotesInfo.Accidentals accidentals)
        {
            var listOfLedgerLines = new List<FrameworkElement>();
            if (stringFretPair.StringName == FretBoardOptions.StringName.E && stringFretPair.FretNo == 0)
            {
                listOfLedgerLines.Add(GetBottomLedgerLine());
            }
            if (stringFretPair.StringName == FretBoardOptions.StringName.G && stringFretPair.FretNo > 4)
            {
                listOfLedgerLines.Add(GetFirstUpperLedgerLine());
            }

            if (accidentals == NotesInfo.Accidentals.Flat)
            {
                if (stringFretPair.StringName == FretBoardOptions.StringName.G && stringFretPair.FretNo >= 8)
                {
                    listOfLedgerLines.Add(GetSecondUpperLedgerLine());
                }
            }
            else
            {
                if (stringFretPair.StringName == FretBoardOptions.StringName.G && stringFretPair.FretNo > 8)
                {
                    listOfLedgerLines.Add(GetSecondUpperLedgerLine());
                }
            }
            return listOfLedgerLines;
        }

        public IEnumerable<FrameworkElement> GetItemsToDraw(Image graphicNote, IEnumerable<FrameworkElement> ledgerLines, FrameworkElement accidental)
        {
            var itemsToDraw = new List<FrameworkElement>();
            itemsToDraw.AddRange(ledgerLines);

            itemsToDraw.Add(graphicNote);
            if (accidental != null)
                itemsToDraw.Add(accidental);
            return itemsToDraw;
        }


       
    }
}