using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using BassNotesMasterApi.Components.Fretboard;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Settings;
using BassNotesMasterApi.Utils;
using WpfExtensions;

namespace BassNotesMaster.FretBoard.FretBoardView
{
    public class FretBoardGuiBuilder : IFretBoardGuiBuilder
    {
        private static Settings _settings;
        public readonly Canvas DrawArea;
        public readonly Panel Container;
        private readonly BorderStyleCollection _borderStyleCollection = BorderStyleCollection.Instance;
        private readonly FretBoardGuiCalculator _boardGuiCalculator;

        public AlwaysRedrawCollection AlwaysRedrawCollection { get; set; }
        public bool HideNoteLabel { get; set; }

        public FretBoardGuiBuilder(Settings settings, Canvas mainDrawingArea, Panel container)
        {
            Container = container;
            DrawArea = mainDrawingArea;
            _settings = settings;
            _boardGuiCalculator =
                new FretBoardGuiCalculator(DrawArea);
            AlwaysRedrawCollection = new AlwaysRedrawCollection();
        }

        public void Reset()
        {
            HideNoteLabel = false;
        }

        public void DrawAllGraphicNoteRepresentation()
        {
            foreach (var uiElement in GetAllGraphicNoteRepresentation())
            {
                DrawArea.Children.Add(uiElement);
            }
        }

        public void DrawNoteWithQuestionMark(StringFretPair stringFretToFind)
        {
            var border = CreateAndGetRectangleToDrawBasedOnStringFretPair(stringFretToFind, "?");
            DrawNote(border);
        }

        public void RedrawNoteWithQuestionMark(StringFretPair stringFretToFind, bool result)
        {
            ClearView();
            var border = CreateAndGetRectangleToDrawBasedOnStringFretPair(stringFretToFind, "?",
                                                                          result);
            DrawNote(border);
        }

        public void DrawNotesIfNotExist(StringFretPair[] collectionOfStringFretPair)
        {
            foreach (var stringFretPair in collectionOfStringFretPair.Where(x => !IsAlreadyDrawn(x)))
            {
                DrawNote(stringFretPair);
            }
        }

        public bool IsAlreadyDrawn(StringFretPair stringFretPair)
        {
            return GetBorderForCords(stringFretPair) != null;
        }


        public void DrawNotes(StringFretPair[] collection, double transparency = 1.0,bool drawLabels = true)
        {
            foreach (var stringFretPair in collection)
            {
                DrawNote(stringFretPair, true, drawLabels,transparency);
            }
        }

        public void DrawNote(Point clikedPoint)
        {
            var rect = CreateAndGetGraphicNoteRepresentation(clikedPoint);
            DrawNote(rect);
        }

        public void DrawNote(StringFretPair cords, double transparency = 1.0, bool drawLabels = true)
        {
            DrawNote(cords, isCorrect: true, transparency: transparency,drawLabels: drawLabels);
        }

        public void DrawNote(StringFretPair cords, bool isCorrect, bool drawLabels = true,double transparency = 1.0 )
        {
            var rect = CreateAndGetGraphicNoteRepresentation(cords, isCorrect, transparency,drawLabels);
            DrawNote(rect);
        }

        public void RedrawNote(StringFretPair pair, bool isCorrect, bool drawLabels = true)
        {
            ClearView();
            DrawNote(pair, isCorrect, drawLabels);
        }

        public void RedrawNote(Point clikedPoint)
        {
            ClearView();
            DrawNote(clikedPoint);
        }

        public void RedrawNotes(StringFretPair[] collection)
        {
            ClearView();
            DrawNotes(collection);
        }

        private IEnumerable<UIElement> GetAllGraphicNoteRepresentation()
        {
            var mapping = NotesToStringFretBoardMapping.Instance; 
            var keys = mapping.Keys;
            return (from stringFretPair in keys
                    let note = mapping.GetNote(stringFretPair)
                    select CreateAndGetRectangleToDrawBasedOnStringFretPair(stringFretPair, note)).
                Cast<UIElement>().ToList();
        }

        public UIElement CreateAndGetGraphicNoteRepresentation(Point cords)
        {
            var note = GetNote(cords);
            return CreateItemToDrawBasedOnClickedPoint(cords, note);
        }

        public UIElement CreateAndGetGraphicNoteRepresentation(StringFretPair cords, bool isCorrect, double transparency, bool drawLabels)
        {
            var note = GetNote(cords);
            return CreateItemToDrawBasedOnClickedPoint(cords, note, isCorrect, transparency,drawLabels);
        }

        public StringFretPair GetPosition(Point cords)
        {
            try
            {
                var scaledPoint = TryToNormalizeClickedPiont(cords);
                return new StringFretPair(scaledPoint);
            }
            catch (CoordinatesOutsideTheFretBoardException ex)
            {
                throw new FretBoardException("Area outside fretboard clicked", ex);
            }
        }
        
        public Note GetNote(StringFretPair cords)
        {
            return TryToGetNote(cords.AsPoint());
        }

        public Note GetNote(Point cords)
        {
            var scaledPoint = TryToNormalizeClickedPiont(cords);
            return TryToGetNote(scaledPoint);
        }

        private Note TryToGetNote(Point cords)
        {
            try
            {
                return NotesToStringFretBoardMapping.Instance.GetNote(new StringFretPair(cords));
            }
            catch (CoordinatesOutsideTheFretBoardException ex)
            {
                throw new FretBoardException("Area outside fretboard clicked", ex);
            }
        }

        public void DrawNote(UIElement rect)
        {
            DrawArea.Children.Add(rect);
        }

        public void Refresh()
        {
            var points = GetCentrePointCollectionOfAllVisibleGraphicNote();
            RemoveAllVisibleGraphicNotesRepresentation();
            foreach (var note in points.Select(CreateAndGetGraphicNoteRepresentation))
            {
                DrawNote(note);
            }
        }

        public Border CreateAndGetRectangleToDrawBasedOnStringFretPair(StringFretPair stringFretPair, Note note)
        {
            var border = CreateGuiBorder(stringFretPair.AsPoint(), true, transparency: 1.0);
            border.Child = CreateGuiLabelForNote(note);
            ApplyColorForBorder(border, note);
            return border;
        }

        public Border CreateAndGetRectangleToDrawBasedOnStringFretPair(StringFretPair stringFretToFind, string text,
                                                                       bool result = true)
        {
            var border = CreateGuiBorder(stringFretToFind.AsPoint(), result, transparency: 1.0);
            border.Child = CreateGuiLabelForNote(text);
            return border;
        }

        public Border CreateItemToDrawBasedOnClickedPoint(StringFretPair stringFretPair, Note note, bool isCorrect,
                                                          double transparency, bool addLabel = true)
        {
            var border = CreateGuiBorder(stringFretPair.AsPoint(), isCorrect, transparency);
            ApplyColorForBorder(border, note);
            if (addLabel && !HideNoteLabel)
            {
                var labelWithNote = CreateGuiLabelForNote(note);
                border.Child = labelWithNote;
            }
            return border;
        }

        public Border CreateItemToDrawBasedOnClickedPoint(Point point, Note note)
        {
            var normalizedPoint = _boardGuiCalculator.NormalizeClickedPoint(point);
            var border = CreateGuiBorder(normalizedPoint, true, transparency: 1.0);
            var labelWithNote = CreateGuiLabelForNote(note);
            ApplyColorForBorder(border, note);
            border.Child = labelWithNote;
            return border;
        }

        private Border CreateGuiBorder(Point point, bool isCorrect, double transparency)
        {
            var border = _borderStyleCollection.GetBorderStyle(isCorrect, _settings.CorrectRectanglePreset.Value);
            border.Opacity = transparency;
            _boardGuiCalculator.AddPositionAttributesForBorder(point, border);


            /*     var style = new Style();
            style.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(Colors.White)));
            var eventTrigger = new Trigger {Property = Border.IsMouseOverProperty, Value = true};
            eventTrigger.Setters.Add(new Setter(Border.BackgroundProperty, new SolidColorBrush(Colors.BurlyWood)));

            style.Triggers.Add(eventTrigger);
            border.Style = style;
         */
            return border;
        }

        private void ApplyColorForBorder(Border border, Note note)
        {
            if (IgnoreColoring) return;

            if (ApplyColorForNotes)
            {
                border.Background =
                    new SolidColorBrush(NoteColorsMapping.First(x => x.Key.EqualsWithoutOctaveNumber(note)).Value);
            }
            if (ApplyColorForOctaves)
            {
                border.Background =
                    new SolidColorBrush(OctavesColorMapping.First(x => x.Key.Equals(note.OctaveNumber)).Value);
            }
        }

        private Label CreateGuiLabelForNote(Note note)
        {
            var label = CreateLabelObject();
            SetContentOfLabelDependingOnShowSettings(label, note);
            return label;
        }

        private Label CreateGuiLabelForNote(string text)
        {
            var label = CreateLabelObject();
            label.Content = text;
            return label;
        }

        private Label CreateLabelObject()
        {
            return new Label
                       {
                           VerticalAlignment = VerticalAlignment.Center,
                           HorizontalAlignment = HorizontalAlignment.Center,
                           FontFamily = new FontFamily(_settings.FontFamilyName.Value),
                           FontSize = _settings.FontSize.Value,
                           Foreground = new SolidColorBrush(Colors.Black)
                       };
        }


        private void SetContentOfLabelDependingOnShowSettings(Label labelWithNote, Note note)
        {
            switch (_settings.FretBoardOptions.Value.Show)
            {
                case FretBoardShow.Sharps:
                    labelWithNote.Content = note.SharpOrRegularRepresenation;
                    break;
                case FretBoardShow.Bemols:
                    labelWithNote.Content = note.BemolRepresenation;
                    break;
                case FretBoardShow.Mixed:
                    SetContentOfLabelForMixedView(labelWithNote, note);
                    break;
            }
        }

        private void SetContentOfLabelForMixedView(Label labelWithNote, Note note)
        {
            if (note.IsNatural())
            {
                labelWithNote.Content = note.SharpOrRegularRepresenation;
            }
            else
            {
                labelWithNote.Content = note.SharpOrRegularRepresenation + Environment.NewLine;
                labelWithNote.Content += note.BemolRepresenation;
            }
        }

        public void ClearView()
        {
            RemoveAllVisibleGraphicNotesRepresentation();
            DrawNotes(AlwaysRedrawCollection.ToArray(), AlwaysRedrawCollection.Transparency,AlwaysRedrawCollection.DrawLabels);
        }

        public void RemoveAllVisibleGraphicNotesRepresentation()
        {
            DrawArea.RemoveVisual<Border>();
        }

        public void ForceClearView()
        {
            AlwaysRedrawCollection.Clear();
            ClearView();
        }

        public Border GetBorderForCords(StringFretPair stringFretPair)
        {
            var rect = _boardGuiCalculator.GetItemCalculatedDimensions(stringFretPair.AsPoint());
            var visibleItems = DrawArea.Children.OfType<Border>();
            return
                visibleItems.FirstOrDefault(visibleItem => rect.IntersectsWith(visibleItem.GetRectangleFromDimensions()));
        }

        public IEnumerable<Point> GetCentrePointCollectionOfAllVisibleGraphicNote()
        {
            return DrawArea.Children.OfType<Border>().
                Select(_boardGuiCalculator.GetLeftTopOf).
                ToList();
        }


        public Point TryToNormalizeClickedPiont(Point cords)
        {
            return _boardGuiCalculator.TryToNormalizeClickedPiont(cords);
        }

        public IEnumerable<Border> GetAllGraphicNotesRepresentations()
        {
            return DrawArea.Children.OfType<Border>();
        }

        public StringFretPair GetStringFretPosition(Border border)
        {
            var point = _boardGuiCalculator.GetLeftTopOf(border);
            point = TryToNormalizeClickedPiont(point);
            return new StringFretPair(point);
        }

        public StringFretPair[] GetCurrentlyShownPosition()
        {
            return
                GetAllGraphicNotesRepresentations().Select(
                    GetStringFretPosition).ToArray();
        }


        protected Dictionary<int, Color> OctavesColorMapping = new Dictionary<int, Color>
                                                                   {
                                                                       {1, Colors.CadetBlue},
                                                                       {2, Colors.BlueViolet},
                                                                       {3, Colors.Brown}
                                                                   };

        protected Dictionary<Note, Color> NoteColorsMapping = new Dictionary<Note, Color>
        {
                                                                      {new Note("C"), Colors.Wheat},
                                                                      {new Note("C#"), Colors.Thistle},
                                                                      {new Note("D"), Colors.Violet},
                                                                      {new Note("D#"), Colors.Aquamarine},
                                                                      {new Note("E"), Colors.Aqua},
                                                                      {new Note("F"), Colors.BlueViolet},
                                                                      {new Note("F#"), Colors.BurlyWood},
                                                                      {new Note("G"), Colors.Coral},
                                                                      {new Note("G#"), Colors.Chocolate},
                                                                      {new Note("A"), Colors.CornflowerBlue},
                                                                      {new Note("A#"), Colors.DarkCyan},
                                                                      {new Note("B"), Colors.DarkKhaki}
                                                                  };

        public bool ApplyColorForOctaves { get; set; }
        public bool ApplyColorForNotes { get; set; }
        public bool IgnoreColoring { get; set; }
    }
}