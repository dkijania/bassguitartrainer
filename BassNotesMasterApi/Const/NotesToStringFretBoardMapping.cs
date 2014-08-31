using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Const
{
    public class NotesToStringFretBoardMapping : IFretBoardMapping
    {
        private static NotesToStringFretBoardMapping _instance;

        public static NotesToStringFretBoardMapping Instance
        {
            get
            {
                return _instance ?? (_instance = new NotesToStringFretBoardMapping());
            }
        }


        protected readonly Dictionary<StringFretPair, Note> FretBoardMapping =
            new Dictionary<StringFretPair, Note>();

        private NotesToStringFretBoardMapping()
        {
            InitializeMapping();
        }

        public ICollection<StringFretPair> Keys
        {
            get { return FretBoardMapping.Keys; }
        }

        public Note[] ValuesDistinct
        {
            get { return FretBoardMapping.Values.Distinct().ToArray(); }
        }

        public StringFretPair[] GetFullOctaveScalesRootPosition(Note root)
        {
            var matches = FretBoardMapping.Where(pair => pair.Value.EqualsWithoutOctaveNumber(root)
                                                         && pair.Key.StringName > FretBoardOptions.StringName.D)
                .Select(pair => pair.Key).ToArray();
            Array.Sort(matches);
            return matches;
        }

        public StringFretPair[] GetFullOctaveScalesRootPosition(Note[] roots)
        {
            var outputList = new List<StringFretPair>();
            foreach (var root in roots)
            {
                outputList.AddRange(GetFullOctaveScalesRootPosition(root));
            }
            return outputList.ToArray();
        }

        public StringFretPair[] GetAllMatchingNotes(Note prototype)
        {
            return FretBoardMapping.Where(pair => pair.Value.Equals(prototype))
                .Select(pair => pair.Key).ToArray();
        }

        public StringFretPair[] GetAllMatchingNotesWithHigherOrEqualOctave(Note prototype)
        {
            return FretBoardMapping.Where(pair => pair.Value.EqualsOrHigherThan(prototype))
                .Select(pair => pair.Key).ToArray();
        }

        protected void InitializeMapping()
        {
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.G, 0), new Note("G2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.G, 1), new Note("G#2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.G, 2), new Note("A2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.G, 3), new Note("A#2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.G, 4), new Note("B2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.G, 5), new Note("C3"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.G, 6), new Note("C#3"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.G, 7), new Note("D3"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.G, 8), new Note("D#3"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.G, 9), new Note("E3"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.G, 10), new Note("F3"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.G, 11), new Note("F#3"));

            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.D, 0), new Note("D2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.D, 1), new Note("D#2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.D, 2), new Note("E2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.D, 3), new Note("F2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.D, 4), new Note("F#2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.D, 5), new Note("G2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.D, 6), new Note("G#2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.D, 7), new Note("A2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.D, 8), new Note("A#2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.D, 9), new Note("B2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.D, 10), new Note("C3"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.D, 11), new Note("C#3"));

            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.A, 0), new Note("A1"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.A, 1), new Note("A#1"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.A, 2), new Note("B1"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.A, 3), new Note("C2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.A, 4), new Note("C#2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.A, 5), new Note("D2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.A, 6), new Note("D#2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.A, 7), new Note("E2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.A, 8), new Note("F2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.A, 9), new Note("F#2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.A, 10), new Note("G2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.A, 11), new Note("G#2"));

            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.E, 0), new Note("E1"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.E, 1), new Note("F1"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.E, 2), new Note("F#1"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.E, 3), new Note("G1"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.E, 4), new Note("G#1"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.E, 5), new Note("A1"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.E, 6), new Note("A#1"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.E, 7), new Note("B1"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.E, 8), new Note("C2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.E, 9), new Note("C#2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.E, 10), new Note("D2"));
            FretBoardMapping.Add(new StringFretPair(FretBoardOptions.StringName.E, 11), new Note("D#2"));
        }

        public Note GetNote(StringFretPair key)
        {
            return FretBoardMapping[key];
        }

        public StringFretPair[] GetAllEquivalentPositions(StringFretPair stringFretPair)
        {
            var note = GetNote(stringFretPair);
            return GetAllMatchingNotes(note);
        }

        public StringFretPair GetMatchingNote(Note noteToSelect, IEnumerable<StringFretPair> possibilities)
        {
            return possibilities.FirstOrDefault(x => GetNote(x).Equals(noteToSelect));
        }

        public StringFretPair GetFirstMatchingPosition(Note note)
        {
            return GetAllMatchingNotes(note).First();
        }

        public StringFretPair[] Convert(Note[] notes)
        {
            return notes.Select(GetFirstMatchingPosition).ToArray();
        }

        public Note[] GetNotes(IEnumerable<StringFretPair> stringFretPairs)
        {
            return stringFretPairs.Select(GetNote).ToArray();
        }

        public StringFretPair[] GetAllPositionsDistinct()
        {
            return FretBoardMapping.Keys.ToArray();
        }
    }
}