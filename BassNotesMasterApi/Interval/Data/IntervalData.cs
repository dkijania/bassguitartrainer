using System;
using System.Collections.Generic;
using System.Linq;

namespace BassNotesMasterApi.Interval.Data
{
    public class IntervalData
    {
        private readonly List<IntervalRow> _intervalRows = new List<IntervalRow>();
        public List<IntervalRow>.Enumerator GetEnumerator()
        {
            return _intervalRows.GetEnumerator();
        }

        public IntervalData()
        {
            _intervalRows.Add(new IntervalRow(0, "1", "unison"));
            _intervalRows.Add(new IntervalRow(1, "b2", "minor second"));
            _intervalRows.Add(new IntervalRow(2, "2", "Major second"));
            _intervalRows.Add(new IntervalRow(3, "b3", "minor third"));
            _intervalRows.Add(new IntervalRow(4, "3", "Major third"));
            _intervalRows.Add(new IntervalRow(5, "4", "Perfect fourth"));
            _intervalRows.Add(new IntervalRow(6, "b5", "diminshed fifth"));
            _intervalRows.Add(new IntervalRow(7, "5", "Perfect fifth"));
            _intervalRows.Add(new IntervalRow(8, "#5", "augmented fifth"));
            _intervalRows.Add(new IntervalRow(9, "6", "Major sixth"));
            _intervalRows.Add(new IntervalRow(10, "b7", "minor seventh"));
            _intervalRows.Add(new IntervalRow(11, "7", "Major seventh"));
            _intervalRows.Add(new IntervalRow(12, "8", "Perfect octave"));
        }

        public IntervalRow ByName(string content)
        {
            return _intervalRows.FirstOrDefault(x=> string.Equals(x.IntervalName,content,StringComparison.OrdinalIgnoreCase));
        }

        public IntervalRow BySemitone(int semitone)
        {
            return _intervalRows.FirstOrDefault(x => Equals(x.Semitone, semitone));
        }
    }
}