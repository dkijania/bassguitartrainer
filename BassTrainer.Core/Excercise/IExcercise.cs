using System.Collections.Generic;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Components.Statistics;
using BassTrainer.Core.Const;

namespace BassTrainer.Core.Excercise
{
    public interface IExcercise
    {
        void Start(IEnumerable<StringFretPair> pairs);
        void Stop();
        void Pause();
        void Skip();
        void Continue(StringFretPair[] pairs);
        void ShowOptions();
        bool IsPaused { get; }
        StatisticRow StatisticData { get; set; }
        ISelectionSetter SelectionSetter { get; }

    }
}