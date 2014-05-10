using BassNotesMasterApi.Statistics;

namespace BassNotesMasterApi.Utils.ResultSerializer
{
    public interface IResultSerializer
    {
        void Save(StatisticRow statisticData);
        SerializerRoot Read();
    }
}
