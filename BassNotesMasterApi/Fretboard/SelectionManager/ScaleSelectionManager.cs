using System;
using System.Collections.Generic;
using System.Linq;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Const.NotesFinder;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Fretboard.SelectionManager
{
    public class ScaleSelectionManager
    {
        private readonly NotesToStringFretBoardMapping _notesMapping = NotesToStringFretBoardMapping.Instance;
        private readonly ScaleStructureProvider _scaleStructureProvider = new ScaleStructureProvider();
        private readonly ScaleNoteFinder _scaleNoteFinder = new ScaleNoteFinder();
        
        public enum ScaleFingering
        {
            Standard,
            Open,
            Alternative
        }

        public IEnumerable<StringFretPair> SelectScale(ScaleType scaleTypeEnum, Note rootNote, int startFret, int endFret)
        {
            var scaleNotes = GetNotesToFind(scaleTypeEnum, rootNote);
            var outputList = new List<StringFretPair>();
            for (var currentFret = Math.Min(startFret,endFret); currentFret <= Math.Max(startFret,endFret); currentFret++)
            {
                for (var currentString = FretBoardOptions.StringName.G; currentString <= FretBoardOptions.StringName.E; currentString++)
                {
                    var stringFretPair = new StringFretPair(currentString, currentFret);
                    var note = _notesMapping.GetNote(stringFretPair);
                    if(scaleNotes.Any(x => x.EqualsWithoutOctaveNumber(note)))
                    {
                        outputList.Add(stringFretPair);
                    }
                    
                }
            }
            return outputList;
        }
        
        public IEnumerable<StringFretPair> SelectScale(ScaleType scaleType, Note root, int position,
                                                       ScaleFingering scaleFingering)
        {
            var scaleNotes = GetNotesToFind(scaleType, root);
            var positionsRoot = _notesMapping.GetFullOctaveScalesRootPosition(root);
            var positionToSelect = positionsRoot[position - 1];
            return _scaleNoteFinder.FillListWithNotes(positionToSelect, scaleNotes, ConvertToStrategy(scaleFingering));
        }

        private Note[] GetNotesToFind(ScaleType scaleType, Note root)
        {
            switch (scaleType)
            {
                case ScaleType.Major:
                    return _scaleStructureProvider.GetMajorScale(root);
                case ScaleType.Minor:
                    return _scaleStructureProvider.GetMinorScale(root);
                default:
                    return _scaleStructureProvider.GetMaj7Scale(root);
            }
        }

        private ScaleNoteFinder.FindNextNoteStrategy ConvertToStrategy(ScaleFingering scaleFingering)
        {
            switch (scaleFingering)
            {
                case ScaleFingering.Standard:
                    return ScaleNoteFinder.FindNextNoteStrategy.PreferClosestNotes;
                case ScaleFingering.Open:
                    return ScaleNoteFinder.FindNextNoteStrategy.PreferForwardNotes;
                default:
                    return ScaleNoteFinder.FindNextNoteStrategy.PreferBackwardNotes;
            }
        }

        
    }
}