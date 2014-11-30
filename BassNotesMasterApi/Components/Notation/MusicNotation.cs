using System;
using System.Windows;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Components.Notation
{
    public class MusicNotation
    {
        public readonly MusicNotationGraphicObjectsManager GraphicObjectsManager;
        public NotesInfo.Accidentals Accidentals = NotesInfo.Accidentals.None;
        public event OnMouseClick OnMouseClickEvent;
        public delegate void OnMouseClick(StringFretPair stringFretPair);
        
        private readonly NotesToStringFretBoardMapping _fretBoardMapping = NotesToStringFretBoardMapping.Instance;
        private readonly NotesInfo _notesInfo = new NotesInfo();
        private readonly Settings.Settings _settings;
        private readonly Random _random = new Random();
        
        public void ResetGui()
        {
            GraphicObjectsManager.ClearView();
            GraphicObjectsManager.DrawBassStave();
        }
        
        public MusicNotation(MusicNotationGraphicObjectsManager wpfMusicNotationGraphicObjectsManager,
                             Settings.Settings settings)
        {
            GraphicObjectsManager = wpfMusicNotationGraphicObjectsManager;
            _settings = settings;
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

        public void ResetMouseEvent()
        {
            OnMouseClickEvent = null;
        }
    }
}