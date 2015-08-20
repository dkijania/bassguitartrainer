using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BassTrainer.Core.Const.BassGuitar.Parameters.Tuning
{
    public class TuningSounds
    {
        public BassGuitarTuningId TuningId { get; private set; }
        public string Name { get; private set; }
        public IEnumerable<Note> Notes { get; private set; }
        private LinkedList<Note> OrderedList { get; set; }

        public TuningSounds(string name, params Note[] sounds)
        {
            Name = name;
            Notes = sounds;
            OrderedList = new LinkedList<Note>(Notes);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, String.Join("-", Notes));
        }

        public Note GetNextTo(Note note)
        {
            var foundNote = GetNode(note);
            return IsLast(foundNote) ? OrderedList.First() : foundNote.Next.Value;
        }

        public Note GetPrevTo(Note note)
        {
            var foundNote = GetNode(note);
            return IsPrev(foundNote) ? OrderedList.Last() : foundNote.Previous.Value;
        }

        private LinkedListNode<Note> GetNode(Note note)
        {
            var foundNote = OrderedList.Find(note);
            CheckIfNull(foundNote, note);
            return foundNote;
        }

        private void CheckIfNull(LinkedListNode<Note> node, Note note)
        {
            if (node == null)
                throw new InvalidDataException(String.Format("Note: {0} should be in collection: {1}", note, Notes));
        }

        private bool IsLast(LinkedListNode<Note> node)
        {
            return node.Next == null;
        }

        private bool IsPrev(LinkedListNode<Note> node)
        {
            return node.Previous == null;
        }
    }
}