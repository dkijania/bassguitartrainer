using System;
using System.Collections.Generic;
using System.Linq;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Const.NotesFinder
{
    public class ScaleNoteFinder
    {
        private readonly NotesToStringFretBoardMapping _fretBoardMapping = NotesToStringFretBoardMapping.Instance;

        public enum FindNextNoteStrategy
        {
            PreferClosestNotes,
            PreferForwardNotes,
            PreferBackwardNotes
        }

        public List<StringFretPair> FillListWithNotes(StringFretPair rootNotePosition, Note[] notes,
                                                      FindNextNoteStrategy nextNoteStrategy)
        {
            var outputList = new List<StringFretPair>{rootNotePosition};
            var rootNote = _fretBoardMapping.GetNote(rootNotePosition);
            foreach (var note1 in notes.Skip(1))
            {
                note1.OctaveNumber = rootNote.OctaveNumber;
                outputList.Add(GetClosestNotePosition(rootNotePosition, note1,
                                                      nextNoteStrategy));
            }
            return outputList;
        }


        public StringFretPair GetClosestNotePosition(StringFretPair position, Note nextNote,
                                                     FindNextNoteStrategy noteStrategy)
        {
            var possiblePositions = _fretBoardMapping.GetAllMatchingNotesWithHigherOrEqualOctave(nextNote);
            var distances = new Dictionary<StringFretPair, int>();
            switch (noteStrategy)
            {
                case FindNextNoteStrategy.PreferClosestNotes:
                    foreach (var possiblePosition in possiblePositions)
                    {
                        distances[possiblePosition] = GetDistanceForPositions(position, possiblePosition);
                    }
                    break;
                case FindNextNoteStrategy.PreferForwardNotes:
                    foreach (var possiblePosition in possiblePositions)
                    {
                        distances[possiblePosition] = GetDistanceForPositions(position, possiblePosition, 4);
                    }
                    break;
                case FindNextNoteStrategy.PreferBackwardNotes:
                    foreach (var possiblePosition in possiblePositions)
                    {
                        distances[possiblePosition] = GetDistanceForPositions(position, possiblePosition, -4, 2, -2);
                    }
                    break;
            }
            return distances.OrderBy(kvp => kvp.Value).Select(x => x.Key).FirstOrDefault();
        }

        private int GetDistanceForPositions(StringFretPair start, StringFretPair end, int stringWage = 2,
                                            int forwardFretWage = 1, int backwardFretWage = 1)
        {
            const int max = 100;

            if (end.StringName > start.StringName)
            {
                return max;
            }
            if (end.StringName >= start.StringName && end.FretNo < start.FretNo)
            {
                return max;
            }
            if (start.Equals(end))
            {
                return max;
            }

            var fretDifference = (end.FretNo - start.FretNo);
            int fretDiffernceFactor = fretDifference < 0 ? backwardFretWage : forwardFretWage;

            var result = Math.Abs(end.StringName - start.StringName)*stringWage +
                         Math.Abs(fretDifference) * fretDiffernceFactor;

            return result;
        }
    }
}