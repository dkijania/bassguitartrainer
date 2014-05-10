using System.Collections.Generic;
using System.Linq;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Const
{
    public class NotesInfo
    {
        public enum Accidentals
        {
            Sharp,
            Flat,
            None
        }

        public enum NoteStem
        {
            Upward,
            Downward
        }

        public Note[] Order
        {
            get { return _notesOrder.Where(item => item.IsNatural()).ToArray(); }
        }

        public Note[] OrderWithAccidentals
        {
            get { return _notesOrder.ToArray(); }
        }

        private readonly List<Note> _notesOrder;
        public static int OctaveSize;

        public static readonly Note E = new Note("E1");
        public static readonly Note F = new Note("F1");
        public static readonly Note FSharp = new Note("F#1");
        public static readonly Note G = new Note("G1");
        public static readonly Note GSharp = new Note("G#1");
        public static readonly Note A = new Note("A1");
        public static readonly Note ASharp = new Note("A#1");
        public static readonly Note B = new Note("B1");
        public static readonly Note C = new Note("C1");
        public static readonly Note CSharp = new Note("C#1");
        public static readonly Note D = new Note("D1");
        public static readonly Note DSharp = new Note("D#1");


        public NotesInfo()
        {
            _notesOrder = new List<Note>
                              {
                                  C,
                                  CSharp,
                                  D,
                                  DSharp,
                                  E,
                                  F,
                                  FSharp,
                                  G,
                                  GSharp,
                                  A,
                                  ASharp,
                                  B,
                              };
            OctaveSize = Order.Count() - 1;
        }


        public Note GetNoteWithDistanceForwardFromLowestNote(int interval, bool onlyNatural)
        {
            return GetNoteWithDistanceForward(C, interval, onlyNatural);
        }

        public Note GetNoteWithDistanceForward(Note note, int interval, bool onlyNatural)
        {
            var order = onlyNatural ? Order : OrderWithAccidentals;

            var firstOrDefault =
                order.Select((v, ind) => new {n = v, index = ind}).FirstOrDefault(
                    x => x.n.EqualsWithoutOctaveNumber(note));
            if (firstOrDefault != null)
            {
                var i = firstOrDefault.index;
                interval += i;
            }
            var noteToReturn = (Note)order[interval % (order.Count())].Clone();
            noteToReturn.OctaveNumber = interval / (order.Count()) + note.OctaveNumber;
            return noteToReturn;
        }
        
        public int GetDistanceFromNote(StringFretPair stringFretPair, Note note, Accidentals accidental)
        {
            var additionFromAccidental = GetAdditivePositionfromAccidental(accidental);
            var distance = additionFromAccidental;
            switch (stringFretPair.StringName)
            {
                case FretBoardOptions.StringName.E:
                    distance += CalculateDistanceFromNoteDisregardingAccidentals(E, note);
                    break;
                case FretBoardOptions.StringName.A:
                    distance += CalculateDistanceFromNoteDisregardingAccidentals(A, note) + OctaveSize/2;
                    break;
                case FretBoardOptions.StringName.D:
                    distance += CalculateDistanceFromNoteDisregardingAccidentals(D, note) + OctaveSize;
                    break;
                case FretBoardOptions.StringName.G:
                    distance += CalculateDistanceFromNoteDisregardingAccidentals(G, note) + (3*OctaveSize)/2;
                    break;
            }
            return distance;
        }

        public int GetAdditivePositionfromAccidental(Accidentals accidental)
        {
            switch (accidental)
            {
                case Accidentals.Flat:
                    return 1;
                default:
                    return 0;
            }
        }

        private int CalculateDistanceFromNoteDisregardingAccidentals(Note startNote, Note note)
        {
            var array = Order.ToList();
            var i = array.IndexOf(startNote);
            var distance = 0;
            while (true)
            {
                if (i == Order.Count())
                    i = 0;
                if (note.EqualsWithoutAccidentals(Order[i]))
                    break;
                distance++;
                i++;
            }
            return distance;
        }

      

        public int CalculateDistanceFromNote(Note startNote,Note endNote)
        {
            var startIndex = GetPositionInOrder(startNote);
            var endIndex = GetPositionInOrder(endNote);

            if(startIndex == -1 || endIndex == -1)
            {
                return -1;
            }
            
            if(startIndex <=  endIndex)
            {
                return endIndex - startIndex + (endNote.OctaveNumber - startNote.OctaveNumber) * OrderWithAccidentals.Length;
            }
            return (OrderWithAccidentals.Length - startIndex) + (endIndex) +
                   (endNote.OctaveNumber - startNote.OctaveNumber - 1) * OrderWithAccidentals.Length;
        }

        private int GetPositionInOrder(Note startNote)
        {
            var firstOrDefault =
                OrderWithAccidentals.Select((v, ind) => new { n = v, index = ind }).FirstOrDefault(
                    x => x.n.EqualsWithoutOctaveNumber(startNote));
            return firstOrDefault == null ? -1 : firstOrDefault.index;

        }

        public bool NoteExists(Note note, Accidentals accidentals = Accidentals.None)
        {
            var noteToFind = new Note(note, accidentals);
            return OrderWithAccidentals.Any(x => x.EqualsWithoutOctaveNumber(noteToFind));
        }

        public Note GetLowestNote(IEnumerable<Note> notes)
        {
            var enumerable = notes as Note[] ?? notes.ToArray();
             int minimum = int.MaxValue;
            Note noteToReturn = null;
            foreach (var note in enumerable)
            {
                var curentVal = CalculateDistanceFromNote(C, note);
                if (curentVal >= minimum) continue;
                minimum = curentVal;
                noteToReturn = (Note) note.Clone();
            }

            return noteToReturn;
        }
    }
}