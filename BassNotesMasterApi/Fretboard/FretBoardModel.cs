using System.Collections.Generic;
using System.Linq;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Fretboard
{
    public class FretBoardModel
    {
        private readonly NotesToStringFretBoardMapping _notesToStringFretBoardMapping;

        public NotesToStringFretBoardMapping NotesToStringFretBoardMapping
        {
            get { return _notesToStringFretBoardMapping; }
        }

        public FretBoardModel()
        {
            _notesToStringFretBoardMapping = new NotesToStringFretBoardMapping();
        }

        public IEnumerable<StringFretPair> GetAllStringFretPairs()
        {
            return _notesToStringFretBoardMapping.Keys.Cast<StringFretPair>().ToList();
        }
      
        public Note GetNote(StringFretPair stringFretPair)
        {
            return _notesToStringFretBoardMapping.GetNote(stringFretPair);
        }
    }
}