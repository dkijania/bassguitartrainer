using System;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Components.Interval
{
    public class IntervalCalculator
    {
        private readonly NotesInfo _notesInfo = new NotesInfo();
        private readonly IFretBoardMapping _fretBoardMapping = NotesToStringFretBoardMapping.Instance;


        public Note Calculate(Note root,int semitone)
        {
            return _notesInfo.GetNoteWithDistanceForward(root, semitone, onlyNatural: false);
        }

        public int Calculate(StringFretPair start, StringFretPair end)
        {
            var startNote = _fretBoardMapping.GetNote(start);
            var endNote = _fretBoardMapping.GetNote(end);
            return Math.Abs(_notesInfo.CalculateDistanceFromNote(startNote, endNote));
        }
    }
}