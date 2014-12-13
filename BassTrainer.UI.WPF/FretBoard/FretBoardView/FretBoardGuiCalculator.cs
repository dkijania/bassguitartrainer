using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Const;
using Image = System.Windows.Controls.Image;
using Point = System.Windows.Point;

namespace BassTrainer.UI.WPF.FretBoard.FretBoardView
{
    public class FretBoardGuiCalculator
    {
        public const double XFretBoardOffset = 4;
        public const double YFretBoardOffset = 0;

        public double PositionHeight;
        public double PositionWidth;

        public Rect OffsetRect;
        private readonly Panel _drawArea;


        public FretBoardGuiCalculator(Panel drawArea)
        {
            _drawArea = drawArea;
        }


        private void GetNewestDrawAreaDimensions()
        {
            var fretBoardImage = GetFretboardImage(_drawArea);
            var actualDrawAreaHeight = fretBoardImage.ActualHeight;
            var actualDrawAreaWidth = fretBoardImage.ActualWidth;

            PositionHeight = CalculatePositionHeight(actualDrawAreaHeight);
            PositionWidth = CalculatePositionWidth(actualDrawAreaWidth);

            OffsetRect = new Rect(6, 5, PositionWidth - 14, PositionHeight - 4);
        }


        private double CalculatePositionHeight(double actualDrawAreaHeight)
        {
            return actualDrawAreaHeight*0.33333 - 14.6;
        }

        private double CalculatePositionWidth(double actualDrawAreaWidth)
        {
            return actualDrawAreaWidth/16 - 1.125;
        }

        public void AddPositionAttributesForBorder(Point point, FrameworkElement border)
        {
            GetNewestDrawAreaDimensions();
            var dimensinos = GetItemCalculatedDimensions(point);
            AddPositionAttributesForBorder(dimensinos, border);
        }

        private void AddPositionAttributesForBorder(Rect dimensions, FrameworkElement border)
        {
            Canvas.SetLeft(border, dimensions.X);
            Canvas.SetTop(border, dimensions.Y);
            border.Width = dimensions.Width;
            border.Height = dimensions.Height;
        }

        public Rect GetItemCalculatedDimensions(Point normalizedPoint)
        {
            GetNewestDrawAreaDimensions();
            var left = (XFretBoardOffset +
                        (normalizedPoint.X)*PositionWidth) +
                       OffsetRect.X;
            var top = (YFretBoardOffset +
                       (normalizedPoint.Y)*PositionHeight) +
                      OffsetRect.Y;
            var width = OffsetRect.Width;
            var height = OffsetRect.Height;
            return new Rect(left, top, width, height);
        }

        private Image GetFretboardImage(Panel drawArea)
        {
            var fretBoardImage = drawArea.Children.OfType<Image>().FirstOrDefault();
            if (fretBoardImage == null)
            {
                throw new NullReferenceException("There is no image of fretboard found");
            }
            return fretBoardImage;
        }

        public Point TryToNormalizeClickedPiont(Point cords)
        {
            var scaledPoint = NormalizeClickedPoint(cords);
            CheckIfIsOutsideFretBoard(scaledPoint);
            return scaledPoint;
        }

        private void CheckIfIsOutsideFretBoard(Point point)
        {
            if (point.Y >= FretBoardOptions.NoOfStrings || point.X < 0 ||
                point.X >= FretBoardOptions.NoOfFret || point.Y < 0)
            {
                throw new CoordinatesOutsideTheFretBoardException();
            }
        }

        public Point NormalizeClickedPoint(Point cords)
        {
            GetNewestDrawAreaDimensions();
            const double xOffset = XFretBoardOffset;
            const double yOffset = YFretBoardOffset;
            double height = PositionHeight;
            double width = PositionWidth;

            var cordsWithOffset = new Point {X = cords.X - xOffset, Y = cords.Y - yOffset};

            var cordsWithScale = new Point {X = (int) (cordsWithOffset.X/width), Y = (int) (cordsWithOffset.Y/height)};
            return cordsWithScale;
        }

        public Point GetLeftTopOf(FrameworkElement element)
        {
            return element.TransformToAncestor(_drawArea).Transform(new Point(0, 0));
        }
    }
}