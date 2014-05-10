using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BassNotesMaster.FretBoard.FretBoardView;
using BassNotesMasterApi.Fretboard.SelectionManager;
using WpfExtensions;

namespace BassNotesMaster.FretBoard.SelectionManager
{
    public class MouseSelectionManager : IMouseSelectionManager
    {
        private bool _isMouseDown;
        private Point _initialMousePosition;
        private readonly Panel _container;
        private readonly Border _selectionBorder;
        private const String SelectedCanvasName = "SelectionCanvas";
        private readonly BorderStyleCollection _borderStyleCollection = BorderStyleCollection.Instance;
        public readonly List<Border> SelectedBorderList = new List<Border>();
        private readonly FretBoardGuiBuilder _boardGuiBuilder;

        public BassNotesMasterApi.Fretboard.SelectionManager.SelectionManager SelectionManager { get; set; }

        public MouseSelectionManager(BassNotesMasterApi.Fretboard.FretBoard fretBoard,
                                     BorderStyleCollection borderStyleCollection)
        {
            _borderStyleCollection = borderStyleCollection;

            var canvas = new Canvas {Name = SelectedCanvasName};
            var border = _borderStyleCollection.SelectedBorderStyle;
            _boardGuiBuilder = (FretBoardGuiBuilder) fretBoard.FretBoardGuiBuilder;
            canvas.Children.Add(border);
            _container = _boardGuiBuilder.Container;
            _container.Children.Add(canvas);

            _selectionBorder = border;
            HideSelectionBox();
            Prepare();
        }

        public void Clear()
        {
            SelectedBorderList.Clear();
        }

        public void Prepare()
        {
            _boardGuiBuilder.RemoveAllVisibleGraphicNotesRepresentation();
        }

        public void StartSelectionMode(Point initialMousePosition)
        {
            CaptureAndTrackMouse(initialMousePosition);
            SetInitialPlacementOfSelectionBox();
            MakeSelectionBoxVisible();
        }

        private void CaptureAndTrackMouse(Point initialMousePosition)
        {
            _isMouseDown = true;
            _initialMousePosition = initialMousePosition;
            _container.CaptureMouse();
        }

        private void SetInitialPlacementOfSelectionBox()
        {
            Canvas.SetLeft(_selectionBorder, _initialMousePosition.X);
            Canvas.SetTop(_selectionBorder, _initialMousePosition.Y);
            _selectionBorder.Width = 0;
            _selectionBorder.Height = 0;
        }


        private IEnumerable<Border> GetItemsToSelect(Rect selectionRect)
        {
            return (from border in GetAllSelectableItemFromCanvas()
                    let borderRect = border.GetRectangleFromDimensions()
                    where selectionRect.IntersectsWith(borderRect)
                    select border).ToList();
        }

        private void MakeSelectionBoxVisible()
        {
            _selectionBorder.Visibility = Visibility.Visible;
        }

        private bool IsCtrlDown()
        {
            return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
        }

        private bool IsAltDown()
        {
            return Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
        }


        public void OnMouseMove(Point mousePos)
        {
            if (!_isMouseDown) return;
            if (_initialMousePosition.X < mousePos.X)
            {
                Canvas.SetLeft(_selectionBorder, _initialMousePosition.X);
                _selectionBorder.Width = mousePos.X - _initialMousePosition.X;
                _selectionBorder.Width =
                    Math.Min(_container.ActualWidth + _container.Margin.Left - 10 - _initialMousePosition.X,
                             _selectionBorder.Width);
            }
            else
            {
                Canvas.SetLeft(_selectionBorder, Math.Max(mousePos.X, 0));
                _selectionBorder.Width = _initialMousePosition.X - Math.Max(mousePos.X, 0);
            }

            if (_initialMousePosition.Y < mousePos.Y)
            {
                Canvas.SetTop(_selectionBorder, _initialMousePosition.Y);
                _selectionBorder.Height = mousePos.Y - _initialMousePosition.Y;
                _selectionBorder.Height = Math.Min(_container.ActualHeight - _initialMousePosition.Y,
                                                   _selectionBorder.Height);
            }
            else
            {
                Canvas.SetTop(_selectionBorder, Math.Max(mousePos.Y, 0));
                _selectionBorder.Height = _initialMousePosition.Y - Math.Max(mousePos.Y, 0);
            }
        }

        public void FinishSelectionMode(Point mouseUpPos)
        {
            ReleaseMouseCapture();
            HideSelectionBox();
            var selectionRect = ConvertSelectionToRectangle(mouseUpPos);
            var borders = GetItemsToSelect(selectionRect);

            if (IsCtrlDown())
            {
                AddToSelectionOnlyNewlySelectedItems(borders);
            }
            else if (IsAltDown())
            {
                RemoveSelectionFromItems(borders);
            }
            else
            {
                SelectOnlyItemsFromCurrentFetch(borders);
            }
        }

        private void AddToSelectionOnlyNewlySelectedItems(IEnumerable<Border> borders)
        {
            foreach (var border in borders.Where(border => !SelectedBorderList.Contains(border)))
            {
                try
                {
                    var stringFretPair = _boardGuiBuilder.GetStringFretPosition(border);
                    SelectionManager.SelectItems(stringFretPair);
                }
                catch
                {
                }
            }
        }

        private void RemoveSelectionFromItems(IEnumerable<Border> borders)
        {
            UnselectAll(borders);
        }

        private void SelectOnlyItemsFromCurrentFetch(IEnumerable<Border> borders)
        {
            SelectionManager.UnselectAllItems();
            SelectedBorderList.Clear();
            SelectAll(borders);
        }

        private void SelectAll(IEnumerable<Border> borders)
        {
            var stringFretPairs = borders.Select(_boardGuiBuilder.GetStringFretPosition);
            SelectionManager.SelectItems(stringFretPairs.ToArray());
        }

        private void UnselectAll(IEnumerable<Border> borders)
        {
            var stringFretPairs = borders.Select(_boardGuiBuilder.GetStringFretPosition);
            SelectionManager.UnselectItems(stringFretPairs.ToArray());
        }

        private IEnumerable<Border> GetAllSelectableItemFromCanvas()
        {
            return _boardGuiBuilder.GetAllGraphicNotesRepresentations();
        }

        private Rect ConvertSelectionToRectangle(Point mouseUpPoint)
        {
            var size = _initialMousePosition;
            return new Rect(mouseUpPoint, size);
        }

        private void ReleaseMouseCapture()
        {
            _isMouseDown = false;
            _container.ReleaseMouseCapture();
        }

        private void HideSelectionBox()
        {
            _selectionBorder.Visibility = Visibility.Collapsed;
        }
    }
}