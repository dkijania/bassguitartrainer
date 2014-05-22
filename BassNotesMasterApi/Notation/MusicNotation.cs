using System;
using System.Windows;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Fretboard;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Notation
{
    public class MusicNotation : Manager
    {
        private readonly MusicNotationEventHandler _musicNotationEventHandler;
        private readonly NotesToStringFretBoardMapping _fretBoardMapping = NotesToStringFretBoardMapping.Instance;
        private readonly NotesInfo _notesInfo = new NotesInfo();
        public readonly MusicNotationGraphicObjectsManager GraphicObjectsManager;
        private readonly Settings.Settings _settings;
        private readonly Random _random = new Random();

        public event OnMouseClick OnMouseClickEvent;

        public delegate void OnMouseClick(StringFretPair stringFretPair);

        public NotesInfo.Accidentals Accidentals = NotesInfo.Accidentals.None;

        public override ManagerMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                OnModeChanged(_mode);
            }
        }

        private ManagerMode _mode;


        public override void OnModeChanged(ManagerMode mode)
        {
            _musicNotationEventHandler.UnregisterEventsForButtons();
            _musicNotationEventHandler.UnregisterEventsForCanvas();
            switch (mode)
            {
                case ManagerMode.Info:
                    _musicNotationEventHandler.RegisterEventsForButtons();
                    _musicNotationEventHandler.RegisterEventsForCanvas();
                    break;
            }
        }

        public void ResetGui()
        {
            GraphicObjectsManager.ClearView();
            GraphicObjectsManager.DrawBassStave();
        }


        public MusicNotation(MusicNotationEventHandler musicNotationEventHandler,
                             MusicNotationGraphicObjectsManager wpfMusicNotationGraphicObjectsManager,
                             Settings.Settings settings)
        {
            _musicNotationEventHandler = musicNotationEventHandler;
            musicNotationEventHandler.MusicNotation = this;
            GraphicObjectsManager = wpfMusicNotationGraphicObjectsManager;
            _settings = settings;
        }

        public override void Init()
        {
            ResetGui();
        }

        public void Subscribe(IMusicNotationListener listener)
        {
            OnMouseClickEvent += listener.OnMouseClick;
        }

        public void Unsubscribe(IMusicNotationListener listener)
        {
            OnMouseClickEvent -= listener.OnMouseClick;
        }

        public override void RemoveAllSubscribers()
        {
            _musicNotationEventHandler.ClearAllEvents();
            OnMouseClickEvent = null;
        }


        public void DrawNote(Point clickedPoint)
        {
            var stringFretPosition = GetPositionOnFretboardFor(clickedPoint, Accidentals);
            CanvasDrawNote(stringFretPosition);
            OnMouseClickEvent(stringFretPosition);
            GraphicObjectsManager.DrawTransparentLedgerLines();
        }

        public void RedrawNote(Point clickedPoint)
        {
            ResetGui();
            DrawNote(clickedPoint);
        }

        public void RedrawNote(StringFretPair position, bool result = true)
        {
            GraphicObjectsManager.ClearView();
            GraphicObjectsManager.DrawBassStave();
            CanvasDrawNote(position, result);
        }

        private void CanvasDrawNote(StringFretPair stringFretPair, bool isCorrect = true)
        {
            var note = _fretBoardMapping.GetNote(stringFretPair);
            var accidentals = GetRequestedAccidentals(note);
            var distanceFromBottom = _notesInfo.GetDistanceFromNote(stringFretPair, note, accidentals);
            var stemDirection = GetNoteStemDirection(distanceFromBottom);
            GraphicObjectsManager.DrawNote(distanceFromBottom, accidentals, stemDirection, note, stringFretPair,
                                           isCorrect);
        }

        private NotesInfo.Accidentals GetRequestedAccidentals(Note note)
        {
            if (note.IsNatural())
            {
                return NotesInfo.Accidentals.None;
            }

            if (Accidentals != NotesInfo.Accidentals.None)
            {
                return Accidentals;
            }

            switch (_settings.FretBoardOptions.Value.Show)
            {
                case FretBoardShow.Sharps:
                    return NotesInfo.Accidentals.Sharp;
                case FretBoardShow.Bemols:
                    return NotesInfo.Accidentals.Flat;
                case FretBoardShow.Mixed:
                    return GetNextRandomBoolean() ? NotesInfo.Accidentals.Flat : NotesInfo.Accidentals.Sharp;
                default:
                    return NotesInfo.Accidentals.None;
            }
        }

        private bool GetNextRandomBoolean()
        {
            return _random.Next(1, 3)%2 == 0;
        }

        private NotesInfo.NoteStem GetNoteStemDirection(int distanceFromBottom)
        {
            return distanceFromBottom >= NotesInfo.OctaveSize ? NotesInfo.NoteStem.Downward : NotesInfo.NoteStem.Upward;
        }

        private StringFretPair GetPositionOnFretboardFor(Point clickedPoint, NotesInfo.Accidentals accidentals)
        {
            const int distanceBetweenEandCNote = 2;
            var distanceFromBottom = (int) GraphicObjectsManager.GetDistanceFromBottom(clickedPoint.Y);
            var note = _notesInfo.GetNoteWithDistanceForwardFromLowestNote(
                distanceFromBottom + distanceBetweenEandCNote,
                onlyNatural: true);
            note = AddAccidentalToNoteIfPossible(note, accidentals);
            note = CorrectNoteIfLowerThanE(note);
            return _fretBoardMapping.GetAllMatchingNotes(note)[0];
        }

        private Note CorrectNoteIfLowerThanE(Note note)
        {
            return note.Equals(new Note("Eb1")) ? new Note("E1") : note;
        }

        private Note AddAccidentalToNoteIfPossible(Note note, NotesInfo.Accidentals accidentals)
        {
            return _notesInfo.NoteExists(note, Accidentals) ? new Note(note, accidentals) : note;
        }
    }
}