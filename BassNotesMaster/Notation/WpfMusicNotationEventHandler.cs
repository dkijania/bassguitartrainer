using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using BassNotesMasterApi.Components.Notation;
using BassNotesMasterApi.Const;

namespace BassNotesMaster.Notation
{
    public class WpfMusicNotationEventHandler : MusicNotationEventHandler
    {
        private readonly Panel _canvas;
        private readonly ToggleButton _enableSharp;
        private readonly ToggleButton _enableBemol;
        private readonly MusicNotationGraphicObjectsManager _wpfMusicNotationGraphicObjectsManager;
        
        public WpfMusicNotationEventHandler(Panel canvas, ToggleButton enableSharp, ToggleButton enableBemol,MusicNotationGraphicObjectsManager wpfMusicNotationGraphicObjectsManager)
        {
            _canvas = canvas;
            _canvas.Background = new SolidColorBrush(Colors.White);
            _enableSharp = enableSharp;
            _enableBemol = enableBemol;
            _wpfMusicNotationGraphicObjectsManager = wpfMusicNotationGraphicObjectsManager;
        }

        public override void UnregisterEventsForCanvas()
        {
            _canvas.MouseEnter -= CanvasMouseEnter;
            _canvas.MouseLeave -= CanvasMouseLeave;
            _canvas.MouseDown -= CanvasDrawNote;
        }

        public override void RegisterEventsForCanvas()
        {
            _canvas.MouseEnter += CanvasMouseEnter;
            _canvas.MouseLeave += CanvasMouseLeave;
            _canvas.MouseDown += CanvasDrawNote;
        }

        private void CanvasMouseLeave(object sender, MouseEventArgs e)
        {
            _wpfMusicNotationGraphicObjectsManager.RemoveTransparentLedgerLines();
        }

        private void CanvasMouseEnter(object sender, MouseEventArgs e)
        {
            _wpfMusicNotationGraphicObjectsManager.DrawTransparentLedgerLines();
        }

        private void CanvasDrawNote(object sender, MouseButtonEventArgs e)
        {
            var clickedPoint = e.GetPosition(((WpfMusicNotationGraphicObjectsManager)_wpfMusicNotationGraphicObjectsManager).Canvas);
            MusicNotation.RedrawNote(clickedPoint);
        }

        public override void UnregisterEventsForButtons()
        {
            _enableBemol.IsChecked = false;
            _enableSharp.IsChecked = false;
            _enableSharp.Click -= OnAccidentalsButtonClick;
            _enableBemol.Click -= OnAccidentalsButtonClick;
            _enableSharp.IsEnabled = false;
            _enableBemol.IsEnabled = false;
        }
        
        public override void RegisterEventsForButtons()
        {
            _enableBemol.IsChecked = false;
            _enableSharp.IsChecked = false;
            _enableSharp.Click += OnAccidentalsButtonClick;
            _enableBemol.Click += OnAccidentalsButtonClick;
            _enableSharp.IsEnabled = true;
            _enableBemol.IsEnabled = true;
        }

        protected void OnAccidentalsButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender.Equals(_enableSharp) && _enableSharp.IsChecked != null && _enableSharp.IsChecked.Value)
            {
                MusicNotation.Accidentals = NotesInfo.Accidentals.Sharp;
                _enableBemol.IsChecked = false;
            }
            else if (sender.Equals(_enableBemol) && _enableBemol.IsChecked != null && _enableBemol.IsChecked.Value)
            {
                _enableSharp.IsChecked = false;
                MusicNotation.Accidentals = NotesInfo.Accidentals.Flat;
            }
            else
            {
                MusicNotation.Accidentals = NotesInfo.Accidentals.None;
            }
        }

        public override void ClearAllEvents()
        {
            UnregisterEventsForButtons();
            UnregisterEventsForCanvas();
        }
    }
}