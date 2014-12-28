using System;
using System.Collections.Generic;

namespace BassTrainer.Core.Utils.ResultSerializer
{
    [Serializable]
    public abstract class SerializerExcerciseData
    {

        public abstract string GetName();

        public abstract List<SerializerStatisticRow> GetData();

    }
}