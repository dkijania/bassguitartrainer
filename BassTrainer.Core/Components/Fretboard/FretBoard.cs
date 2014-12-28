using System;
using System.Linq;
using System.Windows;
using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.Fretboard
{
    public class FretBoard 
    {
        private readonly IFretBoardGuiBuilder _fretBoardGuiBuilder;

        public IFretBoardGuiBuilder FretBoardGuiBuilder
        {
            get { return _fretBoardGuiBuilder; }
        }

        public FretBoard(IFretBoardGuiBuilder fretBoardGuiBuilder)
        {
            _fretBoardGuiBuilder = fretBoardGuiBuilder;
        }
        
        public void DrawAllMatchingNoteWithoutOctaveCheck(Note note)
        {
            DoForAllPositions(delegate(Note noteToDraw, StringFretPair position)
                                  {
                                      if (note.EqualsWithoutOctaveNumber(noteToDraw))
                                      {
                                          _fretBoardGuiBuilder.DrawNote(position);
                                      }
                                  });
        }

        public void DrawAllMatchingNotes(Note note)
        {
            DoForAllPositions(delegate(Note noteToDraw, StringFretPair position)
                                  {
                                      if (note.Equals(noteToDraw))
                                      {
                                          _fretBoardGuiBuilder.DrawNote(position);
                                      }
                                  });
        }

        private void DoForAllPositions(Action<Note, StringFretPair> action)
        {
            foreach (var fret in Enumerable.Range(0, FretBoardOptions.NoOfFret))
            {
                foreach (var stringNo in Enumerable.Range(0, FretBoardOptions.NoOfStrings))
                {
                    var position = new StringFretPair(stringNo, fret);
                    var noteToDraw = _fretBoardGuiBuilder.GetNote(position);
                    action.Invoke(noteToDraw, position);
                }
            }
        }
     
        public AlwaysRedrawCollection AlwaysRedrawCollection
        {
            get { return _fretBoardGuiBuilder.AlwaysRedrawCollection; }
            set { _fretBoardGuiBuilder.AlwaysRedrawCollection = value; }
        }

        public Point TryToNormalizeClickedPiont(Point cords)
        {
            return _fretBoardGuiBuilder.TryToNormalizeClickedPiont(cords);
        }

        public void RemoveAllVisibleGraphicNotesRepresentation()
        {
            _fretBoardGuiBuilder.RemoveAllVisibleGraphicNotesRepresentation();
        }

        public void Refresh()
        {
            _fretBoardGuiBuilder.Refresh();
        }

        public void ClearView()
        {
            _fretBoardGuiBuilder.ClearView();
        }

        public void DrawNote(StringFretPair position, double transparency = 1, bool drawLabel = true)
        {
            _fretBoardGuiBuilder.DrawNote(position, transparency, drawLabel);
        }

        public Note GetNote(StringFretPair position)
        {
            return _fretBoardGuiBuilder.GetNote(position);
        }

        public void DrawAllGraphicNoteRepresentation()
        {
            _fretBoardGuiBuilder.DrawAllGraphicNoteRepresentation();
        }

        public StringFretPair GetPosition(Point clikedPoint)
        {
            return _fretBoardGuiBuilder.GetPosition(clikedPoint);
        }

        public void DrawNotes(StringFretPair[] collection, double transparency = 1, bool drawLabel = true)
        {
            _fretBoardGuiBuilder.DrawNotes(collection, transparency, drawLabel);
        }

        public void RedrawNote(StringFretPair stringFretPair, bool isCorrect, bool drawLabel = true)
        {
            _fretBoardGuiBuilder.RedrawNote(stringFretPair, isCorrect, drawLabel);
        }

        public StringFretPair[] GetCurrentlyShownPosition()
        {
            return _fretBoardGuiBuilder.GetCurrentlyShownPosition();
        }

        public void DrawNoteWithQuestionMark(StringFretPair stringFretToFind)
        {
            _fretBoardGuiBuilder.DrawNoteWithQuestionMark(stringFretToFind);
        }

        public void RedrawNoteWithQuestionMark(StringFretPair stringFretToFind, bool isCorrect = true)
        {
            _fretBoardGuiBuilder.RedrawNoteWithQuestionMark(stringFretToFind, isCorrect);
        }

        public bool IsAlreadyDrawn(StringFretPair stringFretPair)
        {
            return _fretBoardGuiBuilder.IsAlreadyDrawn(stringFretPair);
        }

        public void DrawNotesIfNotExist(StringFretPair[] collectionOfStringFretPair)
        {
            _fretBoardGuiBuilder.DrawNotesIfNotExist(collectionOfStringFretPair);
        }

        public void ForceClearView()
        {
            _fretBoardGuiBuilder.ForceClearView();
        }

        public void RedrawNotes(StringFretPair[] collection)
        {
            _fretBoardGuiBuilder.RedrawNotes(collection);
        }

        public bool ApplyColorForOctaves
        {
            get { return _fretBoardGuiBuilder.ApplyColorForOctaves; }
            set
            {
                _fretBoardGuiBuilder.ApplyColorForOctaves = value;
                _fretBoardGuiBuilder.RedrawNotes();
            }
        }

        public bool ApplyColorForNotes
        {
            get { return _fretBoardGuiBuilder.ApplyColorForNotes; }
            set
            {
                _fretBoardGuiBuilder.ApplyColorForNotes = value;
                _fretBoardGuiBuilder.RedrawNotes();
            }
        }

        public bool IgnoreColoring
        {
            get { return _fretBoardGuiBuilder.IgnoreColoring; }
            set { _fretBoardGuiBuilder.IgnoreColoring = value; }
        }

        public bool HideNoteLabel
        {
            get { return _fretBoardGuiBuilder.HideNoteLabel; }
            set { _fretBoardGuiBuilder.HideNoteLabel = value; }
        }

        public void Reset()
        {
            _fretBoardGuiBuilder.Reset();
        }
    }
}