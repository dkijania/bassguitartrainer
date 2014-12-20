using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BassTrainer.Core.Const.BassGuitar.Parameters
{
    public class BassGuitar4StringTuning : ReadOnlyDictionary<BassGuitarTuningId, string>
    {
        public BassGuitar4StringTuning()
            : base(FillDictionary())
        {
        }

        private static Dictionary<BassGuitarTuningId, string> FillDictionary()
        {
            var dict = new Dictionary<BassGuitarTuningId, string> { { BassGuitarTuningId.Standard, "Standard" } };
            return dict;
        }
    }
}