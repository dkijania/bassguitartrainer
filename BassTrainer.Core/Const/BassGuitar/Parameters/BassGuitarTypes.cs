using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BassTrainer.Core.Const.BassGuitar.Parameters
{
    public class BassGuitarTypes : ReadOnlyDictionary<BassGuitarTypeId, String>
    {
        public BassGuitarTypes() : base(FillDictionary())
        {
        }

        private static Dictionary<BassGuitarTypeId, String> FillDictionary()
        {
            var dict = new Dictionary<BassGuitarTypeId, String> {{BassGuitarTypeId.FourString, "4 String Bass"}};
            return dict;
        }
    }
}