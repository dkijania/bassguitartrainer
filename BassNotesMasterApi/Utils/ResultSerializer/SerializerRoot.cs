﻿using System;
using System.Collections.Generic;
using System.Linq;
using BassNotesMasterApi.Statistics;

namespace BassNotesMasterApi.Utils.ResultSerializer
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