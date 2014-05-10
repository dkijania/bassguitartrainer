using System;
using System.Collections.Generic;
using BassNotesMasterApi.Statistics;

namespace BassNotesMasterApi.Utils.ResultSerializer
{
    [Serializable]
    public abstract class SerializerExcerciseData
    {

        public abstract string GetName();

        public abstract List<SerializerStatisticRow> GetData();

    }
}