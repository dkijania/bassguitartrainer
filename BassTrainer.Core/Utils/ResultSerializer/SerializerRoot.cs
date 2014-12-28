using System;
using System.Collections.Generic;
using System.Linq;
using BassTrainer.Core.Components.Statistics;

namespace BassTrainer.Core.Utils.ResultSerializer
{
    [Serializable]
    public abstract class SerializerRoot
    {
        public abstract List<SerializerExcerciseData> GetData();
        public abstract void AddRow(StatisticRow statisticData);
        public bool HasDataFor(StatisticRow row)
        {
            return GetData().Any(x => string.Equals(x.GetName(), row.Excercise, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}