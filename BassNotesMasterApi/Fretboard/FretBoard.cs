using System;
using System.Linq;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Settings;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Fretboard
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
        
        public void Refresh()
        {
           
            FretBoardGuiBuilder.Refresh();
        }
     
        public void ClearView()
        {
            _fretBoardGuiBuilder.ClearView();
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

        
    }
}