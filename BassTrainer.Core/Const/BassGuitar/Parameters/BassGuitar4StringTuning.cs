using System.Collections.Generic;
using BassTrainer.Core.Const.BassGuitar.Parameters.Tuning;

namespace BassTrainer.Core.Const.BassGuitar.Parameters
{
    public class BassGuitar4StringTuning : List<TuningSounds>
    {
        public BassGuitar4StringTuning()
            : base(FillList())
        {
        }

        private static IEnumerable<TuningSounds> FillList()
        {
            return new List<TuningSounds> { new TuningSounds("Standard", new Note("E1"),new Note("A1"),new Note("D2"),new Note("G2")) };
        }
    }
}