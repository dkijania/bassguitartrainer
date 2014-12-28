using BassTrainer.Core.Components.Statistics;

namespace BassTrainer.Core.Utils.ResultSerializer
{
    public interface IResultSerializer
    {
        void Save(StatisticRow statisticData);
        SerializerRoot Read();
    }
}
