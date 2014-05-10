using System.Collections.Generic;
using System.Linq;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Const
{
    public class ScaleStructureProvider
    {
        private const int WholeStep = 2;
        private const int HalfStep = 1;

        public static int[] MajorScaleIntervals =
            {
                WholeStep, WholeStep, HalfStep, WholeStep, WholeStep, WholeStep,
                HalfStep
            };
        public static int[] MinorScaleIntervals =
            {
                WholeStep, HalfStep,WholeStep,WholeStep, HalfStep,WholeStep,WholeStep
            };
 
        public Note[] GetMajorScale(Note root)
        {
            return GetScale(MajorScaleIntervals, root);
        }

        public Note[] GetMinorScale(Note root)
        {
            return GetScale(MinorScaleIntervals, root);
        }

        //TODO: refactor
        public Note[] GetMaj7Scale(Note root)
        {
            var majorNotes = GetMajorScale(root);
            var indices = new[] {0,2,4,6,7};
           return majorNotes.Select ((f, i) => new {f, i})
                .Where (x => indices.Contains(x.i))
                .Select (x => x.f).ToArray();
        }

        public Note[] GetScale(int[] intervals, Note root)
        {
            var notesInfo = new NotesInfo();
            var outputList = new List<Note>();
            var currentInterval = 0;
            outputList.Add(root);
            foreach (var interval in intervals)
            {
                currentInterval += interval;
                outputList.Add(notesInfo.GetNoteWithDistanceForward(root, currentInterval,onlyNatural:false));
            }
            return outputList.ToArray();
        }
    }
}