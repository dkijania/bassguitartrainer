using System.Windows.Controls;
using System.Windows.Input;
using BassNotesMaster.FretBoard.FretBoardView;
using BassNotesMasterApi.Fretboard;

namespace BassNotesMaster.FretBoard
{
    public class WpfFretboardEventHandler : FretboardEventHandler
    {
        public BassNotesMasterApi.Fretboard.SelectionManager.SelectionManager SelectionManager { get; set; }

        public WpfFretboardEventHandler(BassNotesMasterApi.Fretboard.FretBoard fretboard,BassNotesMasterApi.Fretboard.SelectionManager.SelectionManager selectionManager) : base(fretboard)
        {
            SelectionManager = selectionManager;
        }

        private Panel GetContainer()
        {
            var guiBuilder = (FretBoardGuiBuilder)FretBoard.FretBoardGuiBuilder;
            return guiBuilder.Container;
        }

        private Canvas GetCanvas()
        {
            var guiBuilder = (FretBoardGuiBuilder)FretBoard.FretBoardGuiBuilder;
            return guiBuilder.DrawArea;
        }

        public void OnMouseDownForSelectionEvent(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(GetContainer());
            SelectionManager.MouseSelectionManager.StartSelectionMode(point);
        }

        public void OnMouseMoveForSelectionEvent(object sender, MouseEventArgs e)
        {
            var point = e.GetPosition(GetContainer());
            SelectionManager.MouseSelectionManager.OnMouseMove(point);
        }

        public void OnMouseUpForSelectionEvent(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(GetContainer());
            SelectionManager.MouseSelectionManager.FinishSelectionMode(point);
        }

        public override void SubscribeExcerciseHandlerForFretboard()
        {
            GetCanvas().MouseDown += MouseDownOnImageForExcercise;
        }

        public override void UnsubscribeExcerciseHandlerForFretboard()
        {
            GetCanvas().MouseDown -= MouseDownOnImageForExcercise;
        }

        public override void SubscribeInfoHandlerForFretboard()
        {
            GetCanvas().MouseDown += MouseDownOnImageForInfo;
        }

        public override void UnsubscribeInfoHandlerForFretboard()
        {
            GetCanvas().MouseDown -= MouseDownOnImageForInfo;
        }


        public void MouseDownOnImageForExcercise(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var clikedPoint = e.GetPosition(GetCanvas());
                var positionOnfretboard = FretBoard.FretBoardGuiBuilder.GetPosition(clikedPoint);
                RaiseOnMouseClickEvent(positionOnfretboard, FretBoard);
            }
            catch (FretBoardException)
            {
                //pass
            }
        }

        public void MouseDownOnImageForInfo(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var clikedPoint = e.GetPosition(GetCanvas());
                var positionOnfretboard = FretBoard.FretBoardGuiBuilder.GetPosition(clikedPoint);
                RaiseOnMouseClickEvent(positionOnfretboard, FretBoard);
            }
            catch (FretBoardException)
            {
                //pass
            }
        }

        public override void SubscribeSelectionHandlerForFretboard()
        {
            var theGrid = GetContainer();
            theGrid.MouseDown += OnMouseDownForSelectionEvent;
            theGrid.MouseMove += OnMouseMoveForSelectionEvent;
            theGrid.MouseUp += OnMouseUpForSelectionEvent;
        }

        public override void UnsubscribeSelectionHandlerForFretboard()
        {
            var theGrid = GetContainer();
            theGrid.MouseDown -= OnMouseDownForSelectionEvent;
            theGrid.MouseMove -= OnMouseMoveForSelectionEvent;
            theGrid.MouseUp -= OnMouseUpForSelectionEvent;
            SelectionManager.MouseSelectionManager.Prepare();
            FretBoard.FretBoardGuiBuilder.IgnoreColoring = false;
        }
    }
}