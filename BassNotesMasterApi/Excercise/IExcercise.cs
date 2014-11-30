using System.Collections.Generic;
using BassNotesMasterApi.Components.Fretboard;
using BassNotesMasterApi.Statistics;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Excercise
{
    public abstract class AbstractTutorial : IExcercise
    {
        protected AbstractTutorial()
        {
            SelectionSetter = new DefaultSelectionSetter();
        }

        public void Start(IEnumerable<StringFretPair> pairs)
        {
            ManagersLocator.Instance.Mode = ManagerMode.Info;
            Start();
        }

        public void Skip()
        {
        }

        public void Pause()
        {}

        public void Continue(StringFretPair[] pairs)
        {}

        public void ShowOptions()
        {}

        public bool IsPaused { get; private set; }
        public StatisticRow StatisticData { get; set; }
        public ISelectionSetter SelectionSetter { get; private set; }


        public void Stop()
        {
            ManagersLocator.Instance.RemoveAllEvents();
        }

        public void Start()
        {
            BeforeStart();
        }

        public abstract void BeforeStart();
    }


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